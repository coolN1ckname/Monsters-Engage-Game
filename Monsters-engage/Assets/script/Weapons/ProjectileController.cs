using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : WeaponController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedProjectile = Instantiate(weaponData.Prefab);
        spawnedProjectile.transform.position = transform.position;
        spawnedProjectile.GetComponent<ProjectileBehaviour>().DirectionChecker(pm.lastMovedVector);
    }
}
