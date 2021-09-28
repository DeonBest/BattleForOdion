using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

enum PlayerState
{
    VULNERABLE,
    INVULNERABLE
}

enum AnimationState
{
    ENTERING,
    TRANSITIONING,
    WIN
}

public class PlayerController : MonoBehaviour
{

    private Vector3 velocity;
    private float shipWidth;
    private float shipHeight;
    private PlayerState state;
    private SpriteRenderer rend;
    private GameManager gameManager;
    private bool canFire;
    //private AudioSource audioSource;

    public SceneController levelController;
    public SoundController soundController;
    public Animator animator;
    public Sprite normalSprite;
    public Sprite turningSprite;
    public GameObject missilePlayer;
    public GameObject explosion;

    public float speed;
    public float bulletsPerSecond;

    void Start()
    {
        gameManager = GameManager.Instance;
        state = PlayerState.INVULNERABLE;
        velocity = new Vector3(0f, 0f, 0f);
        rend = transform.GetChild(0).GetComponent<SpriteRenderer>();
        //audioSource = transform.GetChild(0).GetComponent<AudioSource>();
        shipWidth = rend.bounds.size.x;
        shipHeight = rend.bounds.size.y;
        canFire = true;
    }

    private void FixedUpdate()
    {
        if (gameManager.GameState == State.PLAYING)
        {
            ApplyMovement();
            ApplyFire();
        }
    }

    public void Move()
    {
        velocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
        if (velocity.x > 0)
        {
            rend.sprite = turningSprite;
            rend.flipX = true;
        }
        else if (velocity.x < 0)
        {
            rend.sprite = turningSprite;
            rend.flipX = false;
        }
        else
        {
            rend.sprite = normalSprite;
        }
    }

    private void ApplyMovement()
    {
        Move();

        var dist = (transform.position - Camera.main.transform.position).z;
        var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
        var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;

        Transform shipTransform = transform.GetChild(0);
        if ((shipTransform.position.x <= leftBorder + shipWidth / 2.0) && velocity.x < 0f)
        {
            velocity = new Vector3(0f, velocity.y, 0f);
        }
        if ((shipTransform.position.x >= rightBorder - shipWidth / 2.0) && velocity.x > 0f)
        {
            velocity = new Vector3(0f, velocity.y, 0f);
        }
        if ((shipTransform.position.y <= bottomBorder + shipHeight / 2.0) && velocity.y < 0f)
        {
            velocity = new Vector3(velocity.x, 0f, 0f);
        }
        if ((shipTransform.position.y >= topBorder - shipHeight / 2.0) && velocity.y > 0f)
        {
            velocity = new Vector3(velocity.x, 0f, 0f);
        }
        transform.position = transform.position + velocity * Time.deltaTime * speed;
    }

    private void ApplyFire() 
    {
        bool firing = Input.GetButton("Fire1");
        if (gameManager.GameState == State.PLAYING && firing && canFire)
        {
            Transform shipTransform = transform.GetChild(0);
            Vector3 offset1 = new Vector3(0.25f, 0.5f, 0f);
            Vector3 offset2 = new Vector3(-0.25f, 0.5f, 0f);

            //create a bullet pointing in its natural direction 
            GameObject b1 = Instantiate(missilePlayer, new Vector3(0f, 0f, 0f), Quaternion.identity);
            GameObject b2 = Instantiate(missilePlayer, new Vector3(0f, 0f, 0f), Quaternion.identity);

            b1.GetComponent<PlayerMissileController>().InitPosition(shipTransform.position + offset1, new Vector3(0f, 3.5f, 0f));
            b2.GetComponent<PlayerMissileController>().InitPosition(shipTransform.position + offset2, new Vector3(0f, 3.5f, 0f));
            canFire = false;

            StartCoroutine(PlayerCanFireAgain());
        }
    }

    IEnumerator PlayerCanFireAgain()
    {
        yield return new WaitForSeconds(1 / bulletsPerSecond);
        canFire = true;
    }

    public void NextLife()
    {
        gameObject.transform.position = new Vector3(0f, -2.5f, -0f);
        animator.SetInteger("AnimationState", (int)AnimationState.ENTERING);
    }

    public void MakeVulnerable()
    {
        state = PlayerState.VULNERABLE;
    }

    public void Restart()
    {
        gameObject.SetActive(true);
        canFire = true;
        NextLife();
    }

    public void WinMovement()
    {
        rend.sprite = normalSprite;
        gameManager.GameState = State.UI;
        animator.SetInteger("AnimationState", (int)AnimationState.WIN);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("AlienMissile") || other.gameObject.CompareTag("Asteroid") || other.gameObject.CompareTag("AlienShip")) && state == PlayerState.VULNERABLE)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            soundController.PlayAudio("playerExplosion");
            gameManager.DecreaseLife();
            animator.SetInteger("AnimationState", (int) AnimationState.TRANSITIONING);
            state = PlayerState.INVULNERABLE;
        }

        if (gameManager.GetLife() <= 0)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            levelController.Lost();
            gameObject.SetActive(false);
        }
    }
}

