using UnityEngine;

public class SoundSearchArea : MonoBehaviour, ISoundSearch
{
    private IPlayerTarget _playerTarget;

    private void Start()
    {
        _playerTarget = transform.parent.gameObject.GetComponent<IPlayerTarget>();
    }

    public void CanMove(PlayerMove playerMove)
    {
        _playerTarget?.GetPlayerMove(playerMove);
    }
}
