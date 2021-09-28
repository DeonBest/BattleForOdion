using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Pattern { 
    Linear,
    LinearBurst,
    Circular,
    CircularBurst
}

public abstract class BulletPattern
{
    protected Pattern pattern;
    abstract public void Fire();
    public Pattern Pattern { get => pattern; }
}
