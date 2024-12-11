using DG.Tweening;
using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    [SerializeField] GameObject Player;
    Vector3 PlayerVector;
    Vector3 WantVector;
    // Start is called before the first frame update
    void Start()
    {
        PlayerVector = Player.transform.position;
        WantVector = Player.transform.position - transform.parent.position;
        this.transform.DOLocalMove(WantVector, 5f).SetEase(Ease.Linear);
    }
}
