using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

public class EnemyFallNeedle : MonoBehaviour
{
    [SerializeField, Header("針のオブジェクト")] private GameObject _needle;
    [SerializeField, Header("落とすポイント")] private GameObject[] _points;
    [SerializeField, Header("落とす間隔")] private int _interval;
    [SerializeField, Header("落とす数(ポイントより少なく)")] private int _num;
    
    private void OnEnable()
    {
        Attack().Forget();
    }

    private async UniTask Attack()
    {
        var random = new Random();
        for (int i = 0; i < _num; i++)
        {
            var rnd = random.Next(i, _points.Length);
            Instantiate(_needle, _points[rnd].transform.position, _needle.transform.rotation);
            (_points[i], _points[rnd]) = (_points[rnd], _points[i]); // スワップ
            await UniTask.Delay(_interval * 1000, cancellationToken : this.GetCancellationTokenOnDestroy());
        }
        gameObject.SetActive(false);
    }
}
