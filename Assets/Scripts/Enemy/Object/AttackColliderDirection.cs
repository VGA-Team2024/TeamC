using UnityEngine;

/// <summary> 親オブジェクトとrotationを合わせる </summary>
public class AttackColliderDirection : MonoBehaviour
{
    [SerializeField, Header("親オブジェクト")] private GameObject _obj;
    
    private void OnEnable()
    {
        gameObject.transform.rotation = _obj.gameObject.transform.rotation;
    }
}
