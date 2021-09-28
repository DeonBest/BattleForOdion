using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField]
    private GameObject laser;

    public SoundController soundController;

    public void StartFire()
    {
        StartCoroutine(Fire());
    }

    private IEnumerator Fire() 
    {
        while (true)
        {
            ParticleSystem charge = GetComponentInChildren<ParticleSystem>();
            charge.Play();
            soundController.PlayAudio("charge");

            yield return new WaitForSeconds(4);

            Quaternion cannonRotation = gameObject.transform.rotation;
            Vector3 offset1 = new Vector3(0f, 0f, 0f);
            GameObject projectile = Instantiate(laser, new Vector3(0f, 0f, 0f), cannonRotation);
            projectile.GetComponent<PlayerMissileController>().InitPosition(gameObject.transform.position + offset1, new Vector3(0f, 3.5f, 0f));
            charge.Stop();
            soundController.PlayAudio("laserFire");

            yield return new WaitForSeconds(1);
        }
    }
}
