namespace AFSInterview
{
    using UnityEngine;

    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed;

        private Enemy targetEnemy;
        private Vector3 targetPosition;

        public void Initialize(Enemy target, Vector3 targetPosition)
        {
            targetEnemy = target;
            this.targetPosition = targetPosition;
        }

        private void Update()
        {
            var direction = (targetPosition - transform.position).normalized;

            transform.position += direction * speed * Time.deltaTime;

            if ((transform.position - targetPosition).sqrMagnitude <= 0.04f)
            {
                Destroy(gameObject);
                if (targetEnemy !=null)
                {
                    Destroy(targetEnemy.gameObject);
                }
            }
        }
    }
}