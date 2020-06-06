using System.Collections;
using System.Collections.Generic;
using AFSInterview;
using UnityEngine;

public class BurstShotTower : SimpleTower
{
    public GameObject test;

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
            var distance = Vector3.Distance(targetEnemy.transform.position, transform.position);
            var t = distance / base.BulletPrefab.Speed;
            Vector3 newPosition = targetEnemy.PredictPlayerPosition(t);

            distance = Vector3.Distance(newPosition, transform.position);
            var t2 = distance / base.BulletPrefab.Speed;

            Debug.Log(t2);

            Vector3 offsetPosition = targetEnemy.PredictPlayerPosition(t2);

            var x = Instantiate(test);
            x.transform.position = offsetPosition;

            if (!preventRotate)
            {
                base.RotateToPosition(offsetPosition);
            }

            if (fireTimer <= 0f)
            {
                if (base.IsTowerLooksOnEnemy(targetEnemy))
                {
                    StartCoroutine(BurstShot(offsetPosition));
                    fireTimer = FiringRate + (shotInterval* burstSize);
                }
            }
        }

        fireTimer -= Time.deltaTime;
    }

    private IEnumerator BurstShot(Vector3 targetPosition)
    {
        preventRotate = true;
        int spawnedBullets = 0;

        while (spawnedBullets < burstSize)
        {
            var bullet = Instantiate(base.BulletPrefab, base.BulletSpawnPoint.position, Quaternion.identity) as Arrow;
       //     bullet.Initialize(targetEnemy, offsetPosition);
            bullet.Initialize(targetPosition);
            spawnedBullets++;
            yield return new WaitForSeconds(shotInterval);
        }

        preventRotate = false;
    }

}

