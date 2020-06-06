using System.Collections;
using System.Collections.Generic;
using AFSInterview;
using UnityEngine;

public class BurstShotTower : SimpleTower
{
    [SerializeField]
    private int burstSize;

    [SerializeField]
    private float shotInterval;

    [SerializeField]
    private bool preventRotate=false;

    protected override void Update()
    {
        Enemy targetEnemy = base.FindClosestEnemy();
        if (targetEnemy != null)
        {
            if (!preventRotate)
            {
                base.RotateToEnemy(targetEnemy);
            }

            if (fireTimer <= 0f)
            {
                if (base.IsTowerLooksOnEnemy(targetEnemy))
                {
                    StartCoroutine(BurstShot(targetEnemy));
                    fireTimer = FiringRate + (shotInterval* burstSize);
                }
            }
        }

        fireTimer -= Time.deltaTime;
    }

    private IEnumerator BurstShot(Enemy targetEnemy)
    {
        preventRotate = true;
        int spawnedBullets = 0;
        Vector3 targetPosition = targetEnemy.transform.position;

        while (spawnedBullets < burstSize)
        {
            var bullet = Instantiate(base.BulletPrefab, base.BulletSpawnPoint.position, Quaternion.identity);
            bullet.Initialize(targetEnemy, targetPosition);
            spawnedBullets++;
            yield return new WaitForSeconds(shotInterval);
        }

        preventRotate = false;
    }

}

