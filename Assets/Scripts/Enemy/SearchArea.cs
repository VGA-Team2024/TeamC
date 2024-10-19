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
        _playerTarget?.GetPlayerMove(other?.GetComponent<PlayerMove>());
    }
}
