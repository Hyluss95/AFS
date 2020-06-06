namespace AFSInterview
{
    using System.Collections.Generic;
    using UnityEngine;

    public class SimpleTower : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletSpawnPoint;
        [SerializeField] private float firingRate;
        [SerializeField] private float firingRange;

        private float fireTimer;
        private Enemy targetEnemy;

        private IReadOnlyList<Enemy> enemies;

        public void Initialize(ref IReadOnlyList<Enemy> enemies)
        {
            this.enemies = enemies;
            fireTimer = firingRate;
        }

        private void Update()
        {
            targetEnemy = FindClosestEnemy();
            if (targetEnemy != null)
            {
                var lookRotation = Quaternion.LookRotation(targetEnemy.transform.position - transform.position);
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }

            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                if (targetEnemy != null)
                {
                    var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity).GetComponent<Bullet>();
                    bullet.Initialize(targetEnemy.gameObject);
                }

                fireTimer = firingRate;
            }
        }

        private Enemy FindClosestEnemy()
        {
            Enemy closestEnemy = null;
            var closestDistance = float.MaxValue;

            // todo check thoroughly foreach - after destroy enemies

            foreach (var enemy in enemies)
            {
                var distance = Vector3.Distance(enemy.transform.position, transform.position);
                if (distance > firingRange)
                {
                    continue;
                }

                if (distance > closestDistance)
                {
                    continue;
                }

                closestEnemy = enemy;
                closestDistance = distance;
            }

            return closestEnemy;
        }
    }
}
