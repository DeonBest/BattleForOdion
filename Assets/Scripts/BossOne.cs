using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum BossOneState
{
    INCOMING,
    FIRING
}


public class BossOne : MonoBehaviour
{

    private Vector3 velocity;
    // public GameObject missile;
    private SpriteRenderer rend;
    //private bool canFire = true;
    private GameManager gameManager;
    private float yOffsetLeft;
    private BossOneState aiState;

    public float yOffset;
    public float speed = 2.0f;
    public long points;
    public int health = 3;
    public AudioClip clip;
    public GameObject explosion;
    public int maxHealth;
    public Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        velocity = new Vector3(Random.Range(0, 1) == 0 ? -1 : 1, 3f, 0f);
        rend = GetComponent<SpriteRenderer>();
        maxHealth = 150;
        health = 150;
        healthBar.value = health / (float)maxHealth;
        yOffsetLeft = yOffset;
        aiState = BossOneState.INCOMING;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GameState == State.PLAYING)
        {
            // calculate location of screen borders
            // this will make more sense after we discuss vectors and 3D
            var dist = (transform.position - Camera.main.transform.position).z;
            var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
            var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;

            //get the width of the object
            float width = rend.bounds.size.x;
            float height = rend.bounds.size.y;

            //1% of the time, switch the direction: 
            int change = Random.Range(0, 800);
            if (change == 0)
            {
                velocity = new Vector3(-velocity.x, velocity.y, 0);
            }

            //make sure the obect is inside the borders... if edge is hit reverse direction
            if ((transform.position.x <= leftBorder + width / 2.0) && velocity.x < 0f)
            {
                velocity = new Vector3(1f, velocity.y, 0f);
            }
            if ((transform.position.x >= rightBorder - width / 2.0) && velocity.x > 0f)
            {
                velocity = new Vector3(-1f, velocity.y, 0f);
            }

            if (yOffsetLeft <= 0 && aiState == BossOneState.INCOMING)
            {
                velocity = new Vector3(velocity.x, 0f, 0f);
                aiState = BossOneState.FIRING;
                GetComponent<MissileTurret>().StartFiring();
            }
            else
            {
                float yDistance = velocity.y * Time.deltaTime;
                yOffsetLeft -= yDistance;
                transform.position = new Vector3(transform.position.x, transform.position.y - yDistance, 0f);
            }

            transform.position = new Vector3(transform.position.x + velocity.x * Time.deltaTime * speed, transform.position.y, 0f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("PlayerMissile"))
        {
            health--;
            gameManager.UpdateScore(10);
            healthBar.value = health / (float)maxHealth;
            Destroy(other.gameObject);
        }
        if (health <= 0)
        {
            AudioSource.PlayClipAtPoint(clip, new Vector3(0f, 0f, -5f));
            Instantiate(explosion, transform.position, Quaternion.identity);
            gameManager.UpdateScore(points);
            Destroy(gameObject);
            healthBar.gameObject.SetActive(false);
            GameObject.Find("MissilePool").GetComponent<MissilePool>().HideAll();
        }
    }
}
