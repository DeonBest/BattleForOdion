using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTurret : MonoBehaviour
{
    [SerializeField]
    private Pattern pattern = Pattern.Linear;
    [SerializeField]
    private float fireRate = 1 / 2f;
    [SerializeField]
    private int bulletPerShot = 10;
    [SerializeField]
    private int shotsPerBurst = 1;
    [SerializeField]
    private float startAngle = 90f;
    [SerializeField]
    private float endAngle = 180f;
    [SerializeField]
    private float spinRate = 0f;
    [SerializeField]
    private float acceleration = 0f;
    [SerializeField]
    private float initialSpeed = 5f;
    [SerializeField]
    private bool fireOnStart = true;

    private BulletPattern bulletPattern;
    private GameManager gameManager;

    public float EndAngle { get => endAngle; set => endAngle = value; }
    public float StartAngle { get => startAngle; set => startAngle = value; }
    public int BulletPerShot { get => bulletPerShot; set => bulletPerShot = value; }
    public float FireRate { get => fireRate; set => fireRate = value; }
    public float Acceleration { get => acceleration; set => acceleration = value; }
    public float InitialSpeed { get => initialSpeed; set => initialSpeed = value; }
    public int ShotsPerBurst { get => shotsPerBurst; set => shotsPerBurst = value; }
    public float SpinRate { get => spinRate; set => spinRate = value; }

    void Start()
    {
        bulletPattern = CreatePattern(pattern);
        gameManager = GameManager.Instance;
        if (fireOnStart)
        {
            StartCoroutine(Fire());
        }
    }

    public void StartFiring()
    {
        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(1 / FireRate);
        while (true)
        {
            if (pattern != bulletPattern.Pattern)
            {
                bulletPattern = CreatePattern(pattern);
            }

            if (gameManager.GameState == State.PLAYING)
            {
                bulletPattern.Fire();
            }
            yield return new WaitForSeconds(1 / FireRate);
        }
    }

    private BulletPattern CreatePattern(Pattern pattern)
    {
        switch (pattern)
        {
            case Pattern.Linear:
                return new Linear(this);
            case Pattern.LinearBurst:
                return new LinearBurst(this);
            case Pattern.Circular:
                return new Circular(this);
            case Pattern.CircularBurst:
                return new CircularBurst(this);
            default:
                return new Circular(this);
        }
    }

    public void SetPattern(Pattern patternIn)
    {
        pattern = patternIn;
    }

    public void SetEndAngle(float newAngle)
    {
        EndAngle = newAngle;
    }

    public void SetStartAngle(float newAngle)
    {
        StartAngle = newAngle;
    }
}

