using UnityEngine;

public class SearchArea : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject; // IPlayerTarget を実装しているクラスがついてるオブジェクト
    private IPlayerTarget[] _playerTargets;

    private void Start()
    {
        _playerTargets = _gameObject.GetComponents<IPlayerTarget>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMove>(out PlayerMove pm))
        {
            foreach (var playerTarget in _playerTargets)
            {
                playerTarget?.GetPlayerMove(pm);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMove>())
        {
            foreach (var playerTarget in _playerTargets)
            {
                playerTarget?.GetPlayerMove(null);
            }
        }
    }
}
