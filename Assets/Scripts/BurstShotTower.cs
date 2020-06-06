
namespace AFSInterview
{
    using System.Collections;
    using UnityEngine;

    public class BurstShotTower : SimpleTower
    {
        [SerializeField]
        private int burstSize;

        [SerializeField]
        private float shotInterval;

        [SerializeField]
        private bool preventRotate = false;

        protected override void Update()
        {
            Enemy targetEnemy = base.FindClosestEnemy();
            if (targetEnemy != null)
            {
                var predictedPosition = PredictShootPosition(targetEnemy);

                if (!preventRotate)
                {
                    base.RotateToPosition(predictedPosition);
                }

                if (fireTimer <= 0f)
                {
                    if (base.IsTowerLooksOnEnemy(targetEnemy))
                    {
                        StartCoroutine(BurstShot(predictedPosition));
                        fireTimer = FiringRate + (shotInterval * burstSize);
                    }
                }
            }

            fireTimer -= Time.deltaTime;
        }

        private Vector3 PredictShootPosition(Enemy targetEnemy)
        {
            var distance = Vector3.Distance(targetEnemy.transform.position, transform.position);
            var time = distance / base.BulletPrefab.Speed;
            Vector3 newPosition = targetEnemy.PredictPlayerPosition(time);

            distance = Vector3.Distance(newPosition, transform.position);
            time = distance / base.BulletPrefab.Speed;
            Vector3 predictedPosition = targetEnemy.PredictPlayerPosition(time);

            // here I know i have a mistake. Velocity of arrow is not constant - is increased during time
            //...

            return predictedPosition;
        }

        private IEnumerator BurstShot(Vector3 targetPosition)
        {
            preventRotate = true;
            int spawnedBullets = 0;

            while (spawnedBullets < burstSize)
            {
                Arrow bullet = GetPooledObjectOrCreateNew();
                
                bullet.Initialize(targetPosition);
                spawnedBullets++;
                yield return new WaitForSeconds(shotInterval);
            }

            preventRotate = false;
        }

        private Arrow GetPooledObjectOrCreateNew()
        {
            var bullet = ObjectPooler.SharedInstance.GetPooledObject(base.BulletPrefab.tag)?.GetComponent<Arrow>();
            if (bullet != null)
            {
                bullet.transform.position = base.BulletSpawnPoint.position;
                bullet.transform.rotation = Quaternion.identity;
                bullet.gameObject.SetActive(true);
            }
            else
            {
                bullet = Instantiate(base.BulletPrefab, base.BulletSpawnPoint.position, Quaternion.identity) as Arrow;
            }

            return bullet;
        }
    }
}