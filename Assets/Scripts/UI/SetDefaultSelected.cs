using UnityEngine;
using UnityEngine.EventSystems;

public class SetDefaultSelected : MonoBehaviour
{
    [SerializeField] private GameObject _defaultSelectedObject;
    [SerializeField] private EventSystem _eventSystem;

    private void OnEnable()
    {
        _eventSystem.SetSelectedGameObject(_defaultSelectedObject);
    }
}
