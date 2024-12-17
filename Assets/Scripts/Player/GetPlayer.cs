using UnityEngine;

public class GetPlayer : SingletonMonoBehaviour<GetPlayer>
{
    public GameObject Player { get; private set; }

    private new void Awake()
    {
        base.Awake();
        Player = this.gameObject;
    }
}