using UnityEngine;

public class EnemyBlowAway : MonoBehaviour, IBlowable
{
    [SerializeField, Header("吹き飛ぶ量")] private float _blow;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    public void BlownAway(Vector3 pos)
    {
        var _dir = transform.position - pos;
        _dir.z = 0;
        _rb.AddForce(_dir * _blow, ForceMode.Impulse);
    }
}
