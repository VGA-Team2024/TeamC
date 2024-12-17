using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private GameObject _obj;
    private SpriteRenderer _sr;
    private float _objHalfSize;
    
    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _objHalfSize = _obj.GetComponent<BoxCollider>().size.y / 2;
    }

    private void Update()
    {
        if (_obj)
        {
            var size = _sr.size;
            size.y = transform.position.y - _obj.transform.position.y - _objHalfSize;
            _sr.size = size;
        }
    }
}