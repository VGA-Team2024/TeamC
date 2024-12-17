using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary> ボス熊が設置するオブジェクト </summary>
public class BearCotton : MonoBehaviour, ITeleportable, ITimeShorten
{
    [SerializeField, Header("消滅までの時間")] private float _time;
    private bool _isCollided;
    private CancellationTokenSource _tokenSource;
    private CancellationToken _token;
    
    private void Start()
    {
        _tokenSource = new CancellationTokenSource();
        _token = _tokenSource.Token;
        Destroy().Forget();
    }

    private void Update()
    {
        
    }

    public void Teleport(Vector3 position)
    {
        _isCollided = true;
        transform.position = position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !_isCollided) return;
        if (!_isCollided && other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.transform.position =
                new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f);
        }
    }
    
    private async UniTask Destroy()
    {
        await UniTask.Delay((int)_time * 1000, cancellationToken:_token);
        Destroy(gameObject);
    }
    
    private void OnDestroy()
    {
        _tokenSource?.Cancel();
        _tokenSource?.Dispose();
    }

    public void TimeShorten()
    {
        _time -= 0.1f;
    }
}
