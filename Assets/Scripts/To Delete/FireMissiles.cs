using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMissiles : MonoBehaviour
{
    //[SerializedField]
    private int missilesAmount = 10;

    //[SerializedField]
    private float startAngle = 90f, endAngle = 270f;

    private Vector2 missileMoveDirection;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Fire", 0f, 2f);
    }

    private void Fire()
    {
        float angleStep = (endAngle - startAngle) / missilesAmount;
        float angle = startAngle;

        for (int i=0; i< missilesAmount + 1; i++)
        {
            float misDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI)/180f);
            float misDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 misMoveVector = new Vector3(misDirX, misDirY, 0f);
            Vector2 misDir = -(misMoveVector - transform.position).normalized;

            GameObject mis = MissilePool.missilePoolInstance.GetMissile();
            mis.transform.position = transform.position;
            mis.transform.rotation = transform.rotation;
            mis.SetActive(true);
            mis.GetComponent<MissileController>().SetMoveDirection(misDir);
            //mis.SetMoveDirection(misDir);

            angle += angleStep;
        }
    }

}
