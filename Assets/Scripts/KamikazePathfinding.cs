using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KamikazePathfinding : MonoBehaviour
{
    NavMeshAgent agent;
    private GameObject player;
    private GameManager gameManager;

    public long points;
    public int health;
    public AudioClip clip;
    public GameObject explosion;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        player = GameObject.Find("PlayerContainer").gameObject;
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        agent.SetDestination(player.transform.position);
        ChangeOrientation();
    }

    void ChangeOrientation() 
    {
        Vector3 ShipToPlayer = player.transform.position - gameObject.transform.position;
        float angleOne = Mathf.Atan2(ShipToPlayer.x, ShipToPlayer.y) * 180 / Mathf.PI;
        Quaternion quaternionOne = Quaternion.AngleAxis(angleOne, -Vector3.forward);
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, quaternionOne, Time.deltaTime * 2f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("PlayerMissile"))
        {
            health--;
            Destroy(other.gameObject);
        }
        if (health <= 0)
        {
            AudioSource.PlayClipAtPoint(clip, new Vector3(0f, 0f, -5f));
            Instantiate(explosion, transform.position, Quaternion.identity);
            gameManager.UpdateScore(points);
            Destroy(gameObject);
        }
    }

}
