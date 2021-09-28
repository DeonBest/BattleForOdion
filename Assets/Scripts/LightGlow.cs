using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightGlow : MonoBehaviour
{
    public float cycleTime = 2f;
    public float startIntensity = 1f;
    public float endIntensity = 2f;
    private Light2D light;
    private float startTime;
    private bool increasing;

    void Start()
    {
        light = GetComponent<Light2D>();
        startTime = Time.timeSinceLevelLoad;
        increasing = true;
    }


    void Update()
    {
        float time = (Time.time - startTime) / cycleTime;

        if (increasing)
        {
            light.intensity = Mathf.Lerp(startIntensity, endIntensity, time);
            if (light.intensity >= endIntensity - 0.025) 
            {
                increasing = false;
                startTime = Time.time;
            }
        }
        else
        {
            light.intensity = Mathf.Lerp(endIntensity, startIntensity, time);
            if (light.intensity <= startIntensity + 0.025)
            {
                increasing = true;
                startTime = Time.time;
            }
        }
    }
}
