using System;
using UnityEngine;

[Serializable]
public class HitboxConfig
{
    public bool drawHitbox = false;
    public Vector3 size = Vector3.one;
    public Vector3 offset = Vector3.zero;
    public bool followAttacker = false;
}
