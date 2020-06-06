namespace AFSInterview
{
    using System.Collections.Generic;
    using UnityEngine;

    public class SimpleTower : MonoBehaviour
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform bulletSpawnPoint;
        [SerializeField] private float firingRate;
        [SerializeField] private float firingRange;

        [SerializeField]
        private float rotationSpeed;

        [SerializeField]
        private float angleOfView;

        protected float fireTimer;

        private IReadOnlyList<Enemy> enemies;

        protected Bullet BulletPrefab => bulletPrefab;

        protected Transform BulletSpawnPoint => bulletSpawnPoint;

        protected float FiringRate => firingRate;

        public void Initialize(ref List<Enemy> enemies)
        {
            this.enemies = enemies;
            fireTimer = firingRate;
        }

        protected virtual void Update()
        {
            Enemy targetEnemy = FindClosestEnemy();
            if (targetEnemy != null)
            {
                RotateToPosition(targetEnemy.transform.position);

                if (fireTimer <= 0f)
                {
                    if (IsTowerLooksOnEnemy(targetEnemy))
                    {
                        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity) as SimpleBullet;
                        bullet.Initialize(targetEnemy, targetEnemy.transform.position);
                        fireTimer = firingRate;
                    }
                }
            }

            fireTimer -=Time.deltaTime;
        }

        protected bool IsTowerLooksOnEnemy(Enemy targetEnemy)
        {
            var direction = targetEnemy.transform.position - transform.position;
            return (Vector3.Angle(direction, transform.forward)) < angleOfView;
        }

        protected void RotateToPosition(Vector3 targetPosition)
        {
            Vector3 targetDirection = targetPosition - transform.position;
            float singleStep = rotationSpeed * Time.deltaTime;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            var lookRotation = Quaternion.LookRotation(newDirection);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, transform.rotation.eulerAngles.z);

            Debug.DrawRay(transform.position, newDirection, Color.red);
        }

        protected Enemy FindClosestEnemy()
        {
            Enemy closestEnemy = null;
            var closestDistance = float.MaxValue;

            for (var index = 0; index < enemies.Count; index++)
            {
                var enemy = enemies[index];
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
