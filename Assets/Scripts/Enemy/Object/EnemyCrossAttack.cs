using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyCrossAttack : MonoBehaviour
{
    [SerializeField] private GameObject _needle;
    [SerializeField, Header("どの位置から攻撃を出すか(順番に)")] private GameObject[] _points;
    [SerializeField, Header("どのくらいの間隔で出すか")] private int _interval;

    private void OnEnable()
    {
        Attack().Forget();
    }

    private async UniTask Attack()
    {
        foreach (var point in _points)
        {
            Instantiate(_needle, point.transform.position, gameObject.transform.rotation);
            await UniTask.Delay(_interval * 1000, cancellationToken : this.GetCancellationTokenOnDestroy());
        }
        gameObject.SetActive(false);
    }
}
