using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissileController : MonoBehaviour
{
        public float speed = 2f;
        public Vector3 velocity;

        void Update()
        {
            if (velocity != null)
                transform.Translate(velocity * Time.deltaTime * speed);

            // this will make more sense after we discuss vectors and 3D
            float dist = (transform.position - Camera.main.transform.position).z;
            float leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
            float rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
            float bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
            float topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;

            //destroy bullet when it leaves the screen
            if (transform.position.y > topBorder ||
                transform.position.y < bottomBorder ||
                transform.position.x > rightBorder ||
                transform.position.x < leftBorder)
            {
                Destroy(gameObject);
            }

        }

        public void InitPosition(Vector3 p, Vector3 v)
        {
            transform.position = p;
            velocity = v;
        }
}
