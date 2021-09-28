using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource playerExplosion;
    private AudioSource alienExplosion;
    private AudioSource alarm;
    private AudioSource win;
    private AudioSource charge;
    private AudioSource laserFire;

    void Awake()
    {
        var aSources = GetComponents<AudioSource>();
        playerExplosion = aSources[0];
        alienExplosion = aSources[1];
        alarm = aSources[2];
        win = aSources[3];
        charge = aSources[4];
        laserFire = aSources[5];
    }
    
    public void PlayAudio(string audio)
    {
        switch (audio)
        {
            case "playerExplosion":
                playerExplosion.Play();
                break;
            case "alienExplosion":
                alienExplosion.Play();
                break;
            case "alarm":
                alarm.Play();
                break;
            case "win":
                win.Play();
                break;
            case "charge":
                charge.Play();
                break;
            case "laserFire":
                laserFire.Play();
                break;
            default:
                break;
        }
    }
}
