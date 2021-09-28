using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePool : MonoBehaviour
{

    public static MissilePool missilePoolInstance;

    [SerializeField]
    private GameObject pooledMissile;
    private bool notEnoughMissilesInPool = true;

    private List<GameObject> missiles;

    private void Awake()
    {
        missilePoolInstance = this;
    }

    void Start()
    {
        missiles = new List<GameObject>();
    }

    public GameObject GetMissile()
    {
        if (missiles.Count > 0)
        {
            for (int i=0; i < missiles.Count; i++)
            {
                if (!missiles[i].activeInHierarchy)
                {
                    return missiles[i];
                }
            }
        }

        if (notEnoughMissilesInPool)
        {
            GameObject mis = Instantiate(pooledMissile);
            mis.SetActive(false);
            missiles.Add(mis);
            return mis;
        }

        return null;
    }

    public void HideAll() 
    {
        for (int i = 0; i < missiles.Count; i++)
        {
            missiles[i].SetActive(false);
        }
    }
}
