using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linear : BulletPattern
{
    private MissileTurret missileTurret;
    private float angle;

    public Linear(MissileTurret missileTurret)
    {
        this.missileTurret = missileTurret;
        pattern = Pattern.Linear;
        angle = missileTurret.StartAngle;
    }
    
    public override void Fire()
    {
        if (missileTurret.SpinRate != 0) 
        {
            angle += missileTurret.SpinRate;
        }

        float misDirX = missileTurret.transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
        float misDirY = missileTurret.transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

        Vector3 misMoveVector = new Vector3(misDirX, misDirY, 0f);
        Vector2 misDir = -(misMoveVector - missileTurret.transform.position).normalized;

        GameObject mis = MissilePool.missilePoolInstance.GetMissile();
        mis.GetComponent<MissileController>().Acceleration = missileTurret.Acceleration;
        mis.GetComponent<MissileController>().InitialSpeed = missileTurret.InitialSpeed;
        mis.transform.position = missileTurret.transform.position;
        mis.transform.rotation = missileTurret.transform.rotation;
        mis.SetActive(true);
        mis.GetComponent<MissileController>().SetMoveDirection(misDir);
    }
}
