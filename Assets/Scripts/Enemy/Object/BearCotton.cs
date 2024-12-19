using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary> ボス熊が設置するオブジェクト </summary>
public class BearCotton : MonoBehaviour, ITeleportable, ITimeShorten
{
    [SerializeField, Header("消滅までの時間")] private float _deleteTime;
    [SerializeField, Header("効果時間")] private float _effectTime;
    private bool _isCollided;
    private CancellationTokenSource _tokenSource;
    private EnemyHp _hp;
    
    private void Start()
    {
        _hp = GetComponent<EnemyHp>();
        StartDestroyTask();
    }

    private void Update()
    {
        if (_hp.CurrentHp <= 0)
        {
            Destroy(gameObject);
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !_isCollided) return;
        if (!_isCollided && other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            _deleteTime = _effectTime;
            StartDestroyTask();
        }
    }

    private void StartDestroyTask()
    {
        _tokenSource?.Cancel();
        _tokenSource = new CancellationTokenSource();
        Destroy(_tokenSource.Token).Forget();
    }
    
    private async UniTask Destroy(CancellationToken token)
    {
        await UniTask.Delay((int)_deleteTime * 1000, cancellationToken : token);
        Destroy(gameObject);
    }
    
    private void OnDestroy()
    {
        _tokenSource?.Cancel();
        _tokenSource?.Dispose();
    }

    public void TimeShorten()
    {
        _deleteTime -= 0.1f;
    }
}
