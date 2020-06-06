namespace AFSInterview
{
    using UnityEngine;

    public abstract class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed;


        public float Speed => speed;
    }
}