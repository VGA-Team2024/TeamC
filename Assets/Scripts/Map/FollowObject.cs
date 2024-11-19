using UnityEngine;

/// <summary> オブジェクトのx座標を追跡する </summary>
public class FollowObject : MonoBehaviour
{
    [SerializeField] private Transform _followObj;
    private Vector3 _pos;
    
    private void Start()
    {
        _pos = transform.position;
    }

    private void FixedUpdate()
    {
        _pos.x = _followObj.transform.position.x;
        transform.position = _pos;
    }
}
