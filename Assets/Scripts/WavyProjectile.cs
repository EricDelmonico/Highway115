using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavyProjectile : Projectile
{
    public Vector3 waveDirection = Vector3.zero;
    protected override void Move()
    {
        transform.position += waveDirection;
        waveDirection = new Vector3(-waveDirection.x, -waveDirection.y, waveDirection.z);
        base.Move();
    }
}
