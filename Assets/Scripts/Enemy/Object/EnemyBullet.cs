using UnityEngine;

public class EnemyBullet : MonoBehaviour, IPlayerTarget
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    private const int DeleteTime = 5;
    private Vector2 _playerPos;
    
    private void Start()
    {
        Direction();
        Destroy(gameObject, DeleteTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * (Time.deltaTime * _speed));
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(gameObject.tag)) return;
        if (other.gameObject.CompareTag("Player") && other.gameObject.TryGetComponent(out IDamageable damage))
        {
            damage.TakeDamage(_damage);
        }
        Destroy(gameObject);
    }

    private void Direction()
    {
        var dir = _playerPos - (Vector2)transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public void GetPlayerPos(Vector2 playerPos)
    {
        _playerPos = playerPos;
    }
}
