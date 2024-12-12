using UnityEngine;

public class GetPlayer : SingletonMonoBehaviour<GetPlayer>
{
    public GameObject Player { get; private set; }

    private void Awake()
    {
        base.Awake();
        Player = this.gameObject;
    }
}