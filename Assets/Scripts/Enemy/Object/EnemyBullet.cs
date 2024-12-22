using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    private const int DeleteTime = 10;
    
    private void Start()
    {
        Destroy(gameObject, DeleteTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * (Time.deltaTime * _speed));
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(gameObject.tag)) return;
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.TryGetComponent(out IDamageable damage) && other.gameObject.TryGetComponent(out IBlowable blo))
            {
                blo.BlownAway(transform.position);
                damage.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
        if (LayerMask.LayerToName(other.gameObject.layer) == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
