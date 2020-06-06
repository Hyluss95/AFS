namespace AFSInterview
{
    using System.Linq;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    public class GameplayManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField]
        private Enemy enemyPrefab;

        [SerializeField]
        private TowerTypeList towerTypeList;

        [Header("Settings")]
        [SerializeField]
        private Vector2 boundsMin;

        [SerializeField]
        private Vector2 boundsMax;

        [SerializeField]
        private float enemySpawnRate;

        [Header("UI")]
        [SerializeField]
        private TextMeshProUGUI enemiesCountText;

        [SerializeField]
        private TextMeshProUGUI scoreText;

        private List<Enemy> enemies = new List<Enemy>();
        private float enemySpawnTimer;
        private int score;

        private static LayerMask groundMask = 8;


        private void Update()
        {
            enemySpawnTimer -= Time.deltaTime;

            if (enemySpawnTimer <= 0f)
            {
                SpawnEnemy();
                enemySpawnTimer = enemySpawnRate;
            }

            if (Input.GetMouseButtonDown(0))
            {
                TrySpawnTower(TowerType.Simple);
            }

            if (Input.GetMouseButtonDown(1))
            {
                TrySpawnTower(TowerType.Burst);
            }

            scoreText.text = "Score: " + score;
            enemiesCountText.text = "Enemies: " + enemies.Count;
        }

        private void TrySpawnTower(TowerType type)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit, Mathf.Infinity))
            {
                return;
            }

            if (hit.collider.gameObject.layer != groundMask.value)
            {
                return;
            }

            var towerPrefab = towerTypeList.TowerList.Single(t => t.Type == type).TowerPrefab;
            var spawnPosition = hit.point;
            spawnPosition.y = towerPrefab.transform.position.y;

            SpawnTower(towerPrefab, spawnPosition);
        }

        private void SpawnEnemy()
        {
            var position = new Vector3(Random.Range(boundsMin.x, boundsMax.x), enemyPrefab.transform.position.y,
                Random.Range(boundsMin.y, boundsMax.y));

            var enemy = GetPooledObjectOrCreateNew(position);
            enemy.Initialize(boundsMin, boundsMax);

            enemies.Add(enemy);
        }

        private Enemy GetPooledObjectOrCreateNew(Vector3 position)
        {
            var poolItem = ObjectPooler.SharedInstance.GetPooledObject(enemyPrefab.tag);
            Enemy enemy;
            if (poolItem)
            {
                enemy = poolItem.GetComponent<Enemy>();
                enemy.transform.position = position;
                enemy.transform.rotation = Quaternion.identity;
                enemy.gameObject.SetActive(true);
            }
            else
            {
                enemy = Instantiate(enemyPrefab, position, Quaternion.identity) as Enemy;
                enemy.OnEnemyDied += Enemy_OnEnemyDied;
            }

            return enemy;
        }

        private void Enemy_OnEnemyDied(Enemy enemy)
        {
            enemies.Remove(enemy);
            score++;
        }

        private void SpawnTower(SimpleTower towerPrefab, Vector3 position)
        {
            var tower = Instantiate(towerPrefab, position, Quaternion.identity).GetComponent<SimpleTower>();
            tower.Initialize(ref enemies);
        }
    }
}