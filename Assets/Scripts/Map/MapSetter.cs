using UnityEngine;
using UnityEngine.UI;

public class MapSetter : MonoBehaviour
{
    [SerializeField, InspectorVariantName("移動先のポータル")] private Collider _portal;
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private MapUpdate _mapUpdate;
    //[SerializeField] private GameObject _mapObject;
    [SerializeField] private Button _button;

    private void Start()
    {
        _mapManager = FindObjectOfType<MapManager>();
        _mapUpdate = FindObjectOfType<MapUpdate>();
        _button = gameObject.GetComponent<Button>();
        _button.onClick.AddListener(SkipMap);
    }

    // MapManagerからMap移動の機能を呼び出す
    private void SkipMap()
    {
        _mapUpdate.SetMapFromPortal(_portal);
    }
}
