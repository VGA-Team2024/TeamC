using UnityEngine;

public class SearchArea : MonoBehaviour
{
    [SerializeField, Header("Playerにつけるタグの名前")] private string _playerTag;
    private IPlayerTarget _playerTarget;

    private void Start()
    {
        _playerTarget = transform.parent.gameObject.GetComponent<IPlayerTarget>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_playerTag)) { _playerTarget?.GetPlayerTransform(other.gameObject.transform); }
    }
}
