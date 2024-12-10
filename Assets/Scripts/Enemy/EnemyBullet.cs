using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    private const int DeleteTime = 5;
    private CancellationTokenSource _tokenSource;
    
    private void Start()
    {
        _tokenSource = new CancellationTokenSource();
        Destroy().Forget();
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

    private async UniTask Destroy()
    {
        await UniTask.Delay(DeleteTime * 1000, cancellationToken : _tokenSource.Token);
        Destroy(gameObject);
    }
    
    private void OnDestroy()
    {
        _tokenSource?.Cancel();
        _tokenSource?.Dispose();
    }
}
