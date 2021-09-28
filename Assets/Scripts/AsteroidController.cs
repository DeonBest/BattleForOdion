using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public int health;
    public Vector3 initialVelocity;
    public GameObject explosion;
    public AudioClip clip;
    public long points;
    
    private GameManager gameManager;

    public Vector3 InitialVelocity { get => initialVelocity; 
        set
        {
            initialVelocity = value;
            GetComponent<Rigidbody2D>().velocity = value;
        }
    }

    private Rigidbody2D rb;
    
    private void Start()
    {
        gameManager = GameManager.Instance;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = initialVelocity;
    }

    void Update()
    {
        //moveSpeed += Acceleration * Time.deltaTime;
        //transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerMissile"))
        {
            Destroy(other.gameObject);
            health--;
        }

        if (health <= 0)
        {
            AudioSource.PlayClipAtPoint(clip, new Vector3(0f, 0f, -5f));
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
            gameManager.UpdateScore(points);
        }
    }
}