using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    private Vector2 moveDirection;
    private float initialSpeed = 5f;
    private float acceleration = 0f;
    private float moveSpeed;

    public float InitialSpeed { get => initialSpeed; set => initialSpeed = value; }
    public float Acceleration { get => acceleration; set => acceleration = value; }

    private void Start()
    {
        moveSpeed = InitialSpeed;
    }

    private void OnEnable()
    {
        moveSpeed = InitialSpeed;
        Invoke("Destroy", 3f);
    }
    
    void Update()
    {
        moveSpeed += Acceleration * Time.deltaTime;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    public void SetMoveDirection(Vector2 direction)     
    {
        moveDirection = direction;
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }

}
