using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    public SoundController soundController;
    public void PlayAlarm()
    {
        soundController.PlayAudio("alarm");
    }
}
