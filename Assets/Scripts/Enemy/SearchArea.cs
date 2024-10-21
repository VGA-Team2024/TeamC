using UnityEngine;

public class SearchArea : MonoBehaviour
{
    private IPlayerTarget _playerTarget;

    private void Start()
    {
        _playerTarget = transform.parent.gameObject.GetComponent<IPlayerTarget>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMove>(out PlayerMove pm))
        {
            _playerTarget?.GetPlayerMove(pm);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMove>())
        {
            _playerTarget?.GetPlayerMove(null);
        }
    }
}
