using UnityEngine;

public class EnemyBreath : MonoBehaviour
{
    [SerializeField] private GameObject _damageFloor;
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;

    private void Update()
    {
        transform.Translate(Vector3.right * (Time.deltaTime * _speed));
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.TryGetComponent(out IDamageable dmg))
        {
            dmg.TakeDamage(_damage);
            Destroy(gameObject);
        }

        if (LayerMask.LayerToName(other.gameObject.layer) == "Ground")
        {
            Destroy(gameObject);
            if (_damageFloor) Instantiate(_damageFloor, transform.position, Quaternion.identity);
        }
    }
}
