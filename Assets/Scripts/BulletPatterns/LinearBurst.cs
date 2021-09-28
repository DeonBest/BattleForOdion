using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
    Variation of the linear pattern. missileTurret.ShotsPerBurst are fired linearly in intervals of 0.1s.
    This pattern is better used with missileTurret.ShotsPerBurst * missileTurret.Firerate < 10 condition is met. 
 */

public class LinearBurst : BulletPattern
{
    private MissileTurret missileTurret;
    private float angle;

    public LinearBurst(MissileTurret missileTurret)
    {
        this.missileTurret = missileTurret;
        pattern = Pattern.Linear;
        angle = missileTurret.StartAngle;
    }

    public override void Fire()
    {
        missileTurret.StartCoroutine(BurstFire());
    }

    private IEnumerator BurstFire()
    {
        float burstRate = 10f;
        int remainingBursts = missileTurret.ShotsPerBurst;

        if (missileTurret.SpinRate != 0)
        {
            angle += missileTurret.SpinRate;
        }

        while (remainingBursts > 0)
        {
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
            remainingBursts--;
            yield return new WaitForSeconds(1 / burstRate);
        }
    }
}
