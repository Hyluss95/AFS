namespace AFSInterview
{
    using UnityEngine;

    public class Arrow : Bullet
    {
        private Rigidbody rigidbody;

        public void Initialize(Vector3 targetPosition)
        {
            Vector3 vo = CalculateVelocity(targetPosition, transform.position);
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.velocity = vo;
        }

        void OnCollisionEnter(Collision collision)
        {
            var enemy = collision.collider.GetComponent<Enemy>();
            if (enemy)
            {
                Destroy(enemy.gameObject);
            }
            Destroy(gameObject);
        }

        public static Vector3 CalculateVelocity(Vector3 target, Vector3 origin)
        {
            Vector3 distance = target - origin;
            Vector3 distanceXz = distance;
            distanceXz.y = 0f;

            float sY = distance.y;
            float sXz = distanceXz.magnitude;

            float Vxz = sXz;
            float Vy = (sY) + (0.5f * Mathf.Abs(Physics.gravity.y));

            Vector3 result = distanceXz.normalized;
            result *= Vxz;
            result.y = Vy;

            return result;
        }
    }
}
