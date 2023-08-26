using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBuilding : Building
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float bulletLifetime = 10f;

    void Start()
    {
        InvokeRepeating("ShootBullet", 0f, fireRate);
    }

    void ShootBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
    }
}
