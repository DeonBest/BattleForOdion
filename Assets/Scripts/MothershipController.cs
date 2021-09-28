using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum MothershipState
{
    INCOMING,
    INVULNERABLE,
    ACTIVATE_CANNONS,
    EXTEND_CARRIERS,
    DEPLOY_KAMIKAZES
}

public class MothershipController : MonoBehaviour
{
    private Vector3 velocity;
    private SpriteRenderer rend;
    private GameManager gameManager;
    private float yOffsetLeft;
    private int health;
    private float startTime;
    private MothershipState aiState;

    public int maxHealth;
    public float yOffset;
    public float speed;
    public long points;
    public AudioClip clip;
    public GameObject explosion;
    public GameObject bigExplosion;
    public Slider healthBar;

    public GameObject cannonOne;
    public GameObject cannonTwo;
    public GameObject carrierOne;
    public GameObject carrierTwo;
    public GameObject kamikazeFighter;
    public Transform kmkPositionOne;
    public Transform kmkPositionTwo;
    public Transform player;

    void Start()
    {
        gameManager = GameManager.Instance;
        velocity = new Vector3(0f, 3f, 0f);
        rend = GetComponent<SpriteRenderer>();
        yOffsetLeft = yOffset;
        health = maxHealth;
        healthBar.value = health / (float) maxHealth;
        startTime = Time.time;
        aiState = MothershipState.INCOMING;
    }

    void Update()
    {
        if (gameManager.GameState == State.PLAYING)
        {
            float healthPercent = health / (float)maxHealth;
            if (healthPercent < 0.75 && aiState == MothershipState.ACTIVATE_CANNONS) 
            {
                ExtendCarriers();
            }

            if (healthPercent < 0.5 && aiState == MothershipState.EXTEND_CARRIERS)
            {
                DeactivateCannons();
                DeployKamikazes();
            }

            var dist = (transform.position - Camera.main.transform.position).z;
            var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
            var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
            float width = rend.bounds.size.x;

            int change = Random.Range(0, 800);
            if (change == 0)
            {
                velocity = new Vector3(-velocity.x, velocity.y, 0);
            }

            if ((transform.position.x <= leftBorder + width / 2.0) && velocity.x < 0f)
            {
                velocity = new Vector3(1f, velocity.y, 0f);
            }
            if ((transform.position.x >= rightBorder - width / 2.0) && velocity.x > 0f)
            {
                velocity = new Vector3(-1f, velocity.y, 0f);
            }

            if (yOffsetLeft <= 0 && aiState == MothershipState.INCOMING)
            {
                velocity = new Vector3(velocity.x, 0f, 0f);
                StartCoroutine(ActivateCannons());
            }
            else
            {
                float yDistance = velocity.y * Time.deltaTime;
                yOffsetLeft -= yDistance;
                transform.position = new Vector3(transform.position.x, transform.position.y - yDistance, 0f);
            }

            transform.position = new Vector3(transform.position.x + velocity.x * Time.deltaTime * speed, transform.position.y, 0f);

            if (aiState == MothershipState.ACTIVATE_CANNONS) 
            {
                AimAtPlayer();
            }

            if (aiState == MothershipState.EXTEND_CARRIERS)
            {
                AimAtPlayer();
                AnimateCarriers();
            }
        }
    }

    public IEnumerator ActivateCannons()
    {
        aiState = MothershipState.INVULNERABLE;
        yield return new WaitForSeconds(4);
        aiState = MothershipState.ACTIVATE_CANNONS;
        cannonOne.GetComponent<CannonController>().StartFire();
        cannonTwo.GetComponent<CannonController>().StartFire();
        GetComponent<MissileTurret>().StartFiring();
    }

    public void DeactivateCannons()
    {
        aiState = MothershipState.ACTIVATE_CANNONS;
        Instantiate(explosion, cannonOne.transform.position, Quaternion.identity);
        Instantiate(explosion, cannonTwo.transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(clip, new Vector3(0f, 0f, -5f));
        Destroy(cannonOne.gameObject);
        Destroy(cannonTwo.gameObject);
    }

    public void ExtendCarriers()
    {
        aiState = MothershipState.EXTEND_CARRIERS;
        startTime = Time.time;
    }

    public void DeployKamikazes()
    {
        aiState = MothershipState.DEPLOY_KAMIKAZES;
        StartCoroutine(CreateKamikazes());
    }

    private void AimAtPlayer()
    {
        Vector3 cannonOneToPlayer = player.position - cannonOne.transform.position;
        float angleOne = Mathf.Atan2(cannonOneToPlayer.x, cannonOneToPlayer.y) * 180 / Mathf.PI;
        Quaternion quaternionOne = Quaternion.AngleAxis(angleOne, -Vector3.forward);
        if (-200 < angleOne && angleOne < -90) 
        {
            cannonOne.transform.rotation = Quaternion.Slerp(cannonOne.transform.rotation, quaternionOne, Time.deltaTime * 3f);
        }


        Vector3 cannonTwoToPlayer = player.position - cannonTwo.transform.position;
        float angleTwo = Mathf.Atan2(cannonTwoToPlayer.x, cannonTwoToPlayer.y) * 180 / Mathf.PI;
        Quaternion quaternionTwo = Quaternion.AngleAxis(angleTwo, -Vector3.forward);
        if (90 < angleTwo && angleTwo < 200)
        {
            cannonTwo.transform.rotation = Quaternion.Slerp(cannonTwo.transform.rotation, quaternionTwo, Time.deltaTime * 3f);
        }
    }

    private void AnimateCarriers() 
    {
        float time = (Time.time - startTime) / 10f;
        carrierOne.transform.localPosition = new Vector3(Mathf.Lerp(0.2f, 1f, time), carrierOne.transform.localPosition.y, 0f);
        carrierTwo.transform.localPosition = new Vector3(Mathf.Lerp(-0.2f, -1f, time), carrierTwo.transform.localPosition.y, 0f);
    }


    private IEnumerator CreateKamikazes()
    {
        while (true) 
        {
            yield return new WaitForSeconds(2);
            if (GameObject.Find("PlayerContainer") != null)
            {
                Instantiate(kamikazeFighter, kmkPositionOne.position, Quaternion.identity);
                Instantiate(kamikazeFighter, kmkPositionTwo.position, Quaternion.identity);
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("PlayerMissile"))
        {
            if (aiState != MothershipState.INVULNERABLE && aiState != MothershipState.INCOMING) 
            {
                health--;
                healthBar.value = health / (float)maxHealth;
                gameManager.UpdateScore(10);
            }
            Destroy(other.gameObject);
        }
        if (health <= 0)
        {
            AudioSource.PlayClipAtPoint(clip, new Vector3(0f, 0f, -5f));
            Instantiate(bigExplosion, transform.position, Quaternion.identity);
            gameManager.UpdateScore(points);
            Destroy(gameObject);
            healthBar.gameObject.SetActive(false);
            GameObject.Find("MissilePool").GetComponent<MissilePool>().HideAll();
        }
    }
}
