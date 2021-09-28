using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    private Material material;
    private Vector2 offset;

    public float velocity = 1f;
    
    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset += new Vector2(0f, velocity / 10) * Time.deltaTime;
    }

    public void StopScrolling()
    {
        velocity = 0;
    }
}
