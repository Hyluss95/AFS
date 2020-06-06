using System.Linq;

namespace AFSInterview
{
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    public class GameplayManager : MonoBehaviour
    {
        [Header("Prefabs")] 
        [SerializeField] private GameObject enemyPrefab;
       
        [SerializeField] private TowerTypeList towerTypeList;

        [Header("Settings")] 
        [SerializeField] private Vector2 boundsMin;
        [SerializeField] private Vector2 boundsMax;
        [SerializeField] private float enemySpawnRate;

        [Header("UI")] 
        [SerializeField] private TextMeshProUGUI enemiesCountText;
        [SerializeField] private TextMeshProUGUI scoreText;
        
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

            SpawnTower(towerPrefab,spawnPosition);
        }

        private void SpawnEnemy()
        {
            var position = new Vector3(Random.Range(boundsMin.x, boundsMax.x), enemyPrefab.transform.position.y, Random.Range(boundsMin.y, boundsMax.y));
            
            var enemy = Instantiate(enemyPrefab, position, Quaternion.identity).GetComponent<Enemy>();
            enemy.OnEnemyDied += Enemy_OnEnemyDied;
            enemy.Initialize(boundsMin, boundsMax);

            enemies.Add(enemy);
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