using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    [SerializeField] GameObject _player;
    Vector3 playerVector;
    [SerializeField] float _playerSizeX = 1;
    [SerializeField] float _playerSizeY = 1;
    [SerializeField] float _effectLost = 3;
    // Start is called before the first frame update
    void Start()
    {
        playerVector = _player.transform.position;
        this.transform.DOLocalMove(playerVector, 5f).SetEase(Ease.Linear);
    }

    private void Update()
    {       
        if (playerVector.x - transform.position.x <= _playerSizeX && playerVector.y - transform.position.y <= _playerSizeY)
        { Destroy(this.gameObject,_effectLost); }
    }
}
