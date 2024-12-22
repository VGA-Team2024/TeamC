using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyWaveNeedleAttack : MonoBehaviour
{
    [SerializeField] private GameObject _parentObj;
    [SerializeField, Header("床のY座標")] private float _groundY;
    [SerializeField, Header("ステージの右はじ")] private GameObject _rightSide;
    [SerializeField, Header("ステージの左はじ")] private GameObject _leftSide;
    [SerializeField, Header("針オブジェクト")] private GameObject _waveNeedle;
    [SerializeField, Header("針が出る間隔")] private float _distance;
    [SerializeField] private float _interval;
    private Queue<GameObject> _pool = new Queue<GameObject>();

    private void OnEnable()
    {
        Wave().Forget();
    }

    private async UniTask Wave()
    {
        if (_parentObj.transform.rotation.y == 0)
        {
            for (float x = gameObject.transform.position.x; x < _rightSide.transform.position.x; x += _distance)
            {
                GetObject(new Vector3(x, _groundY));
                await UniTask.Delay(Mathf.RoundToInt(_interval * 1000), cancellationToken : this.GetCancellationTokenOnDestroy());
            }
        }
        else
        {

            for (float x = gameObject.transform.position.x; x > _leftSide.transform.position.x; x -= _distance)
            {
                GetObject(new Vector3(x, _groundY));
                await UniTask.Delay(Mathf.RoundToInt(_interval * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
            }
        }

        gameObject.SetActive(false);
    }

    private void GetObject(Vector3 pos)
    {
        GameObject obj;

        if (_pool.Count > 0)
        {
            obj = _pool.Dequeue();
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(_waveNeedle);
            var waveNeedle = obj.GetComponent<EnemyWaveNeedle>();
            waveNeedle?.GetWaveAttack(this);
        }

        obj.transform.position = pos;
    }
    
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        _pool.Enqueue(obj);
    }
}
