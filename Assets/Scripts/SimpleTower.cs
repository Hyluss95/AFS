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

        [SerializeField]
        private float rotationSpeed;

        [SerializeField]
        private float angleOfView;

        private float fireTimer;

        private IReadOnlyList<Enemy> enemies;

        public void Initialize(ref List<Enemy> enemies)
        {
            this.enemies = enemies;
            fireTimer = firingRate;
        }

        private void Update()
        {
            Enemy targetEnemy = FindClosestEnemy();
            if (targetEnemy != null)
            {
                RotateToEnemy(targetEnemy);

                if (fireTimer <= 0f)
                {
                    if (IsTowerLooksOnEnemy(targetEnemy))
                    {
                        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity)
                            .GetComponent<Bullet>();
                        bullet.Initialize(targetEnemy.gameObject);
                        fireTimer = firingRate;
                    }
                }
            }

            fireTimer -= Time.deltaTime;
            
        }

        private bool IsTowerLooksOnEnemy(Enemy targetEnemy)
        {
            var direction = targetEnemy.transform.position - transform.position;
            Debug.Log(Vector3.Angle(direction, transform.forward));

            return (Vector3.Angle(direction, transform.forward)) < angleOfView;
        }

        private void RotateToEnemy(Enemy enemy)
        {
            Vector3 targetDirection = enemy.transform.position - transform.position;
            float singleStep = rotationSpeed * Time.deltaTime;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            var lookRotation = Quaternion.LookRotation(newDirection);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, transform.rotation.eulerAngles.z);

            Debug.DrawRay(transform.position, newDirection, Color.red);
        }

        private Enemy FindClosestEnemy()
        {
            Enemy closestEnemy = null;
            var closestDistance = float.MaxValue;

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
