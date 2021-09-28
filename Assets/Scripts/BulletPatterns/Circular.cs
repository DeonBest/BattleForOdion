using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circular : BulletPattern
{
    private MissileTurret missileTurret;
    private float angle;

    public Circular(MissileTurret missileTurret)
    {
        this.missileTurret = missileTurret;
        pattern = Pattern.Circular;
        angle = 0;
    }

    public override void Fire()
    {
        float startAngle = missileTurret.StartAngle;
        float endAngle = missileTurret.EndAngle;
        int missilesAmount = missileTurret.BulletPerShot;

        float angleStep = (endAngle - startAngle) / missilesAmount;

        if (missileTurret.SpinRate != 0)
        {
            angle += missileTurret.SpinRate;
        }

        float originalAngle = startAngle;
        for (int i = 0; i < missilesAmount + 1; i++)
        {
            float misDirX = missileTurret.transform.position.x + Mathf.Sin(((angle + originalAngle) * Mathf.PI) / 180f);
            float misDirY = missileTurret.transform.position.y + Mathf.Cos(((angle + originalAngle)* Mathf.PI) / 180f);

            Vector3 misMoveVector = new Vector3(misDirX, misDirY, 0f);
            Vector2 misDir = -(misMoveVector - missileTurret.transform.position).normalized;

            GameObject mis = MissilePool.missilePoolInstance.GetMissile();
            mis.GetComponent<MissileController>().Acceleration = missileTurret.Acceleration;
            mis.GetComponent<MissileController>().InitialSpeed = missileTurret.InitialSpeed;
            mis.transform.position = missileTurret.transform.position;
            mis.transform.rotation = missileTurret.transform.rotation;
            mis.SetActive(true);
            mis.GetComponent<MissileController>().SetMoveDirection(misDir);
            originalAngle += angleStep;
        }
    }
}
