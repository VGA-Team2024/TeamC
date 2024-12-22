using DG.Tweening;
using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    Player _player;
    Vector3 playerVector;
    [SerializeField] float _playerSizeX = 1;
    [SerializeField] float _playerSizeY = 1;
    [SerializeField] float _effectLost = 3;
    [SerializeField] int _fairyNums = 1;
    private void Start()
    {
        _player = GameObject.FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        playerVector = _player.transform.position;
        this.transform.DOLocalMove(playerVector, 1f).SetEase(Ease.Linear);
        if (playerVector.x - transform.position.x <= _playerSizeX && playerVector.y - transform.position.y <= _playerSizeY)
        {
            if(TryGetComponent<IFairyAddable>(out IFairyAddable fairyAddable)) 
            {
                fairyAddable.AddFairy(_fairyNums);
            }
            Destroy(this.gameObject, _effectLost);
        }
    }
}
