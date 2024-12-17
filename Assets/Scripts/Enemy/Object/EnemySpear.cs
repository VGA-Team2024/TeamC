using UnityEngine;

public class EnemySpear : MonoBehaviour, IPlayerTarget
{
    [SerializeField] private int _damage;
    [SerializeField, Header("床にぶつかってから消えるまでの時間")] private int _time;
    [SerializeField, Header("射出角度"), Range(0, 90)] private float _deg;
    private const float RotateSpeed = 3f;
    private Vector2 _playerPos;
    private Rigidbody _rb;
    
    private void Start()
    {
        Vector3 velocity = CalculateVelocity(transform.position, _playerPos, _deg);
        
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(velocity * _rb.mass, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        Vector3 velocity = _rb.velocity;

        if (velocity.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(velocity.x, velocity.y) * Mathf.Rad2Deg;
            Quaternion targetRot = Quaternion.Euler(0, 0, -angle);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.fixedDeltaTime * RotateSpeed);
        }
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
            Destroy(gameObject, _time);
        }
    }
    
    private Vector3 CalculateVelocity(Vector3 pointA, Vector3 player, float angle)
    {
        float rad = angle * Mathf.PI / 180;
        float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(player.x, player.z));
        float y = pointA.y - player.y;
        float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed)) return Vector3.zero;
        return (new Vector3(player.x - pointA.x, x * Mathf.Tan(rad), player.z - pointA.z).normalized * speed);
    }

    public void GetPlayerPos(Vector2 playerPos)
    {
        _playerPos = playerPos;
    }
}
