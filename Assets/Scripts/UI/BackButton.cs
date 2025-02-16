using UnityEngine;

public class BackButton : MonoBehaviour
{
   [SerializeField] private UIButton _backButton;
   [SerializeField,InspectorVariantName("非表示にしたいオブジェクト")] private GameObject _object;
   [SerializeField,InspectorVariantName("表示したいオブジェクト")] private GameObject _activeObject;

   private void Start()
   {
      Initialize();
   }

   private void Initialize()
   {
      _backButton.OnClickAddListener(OnClickBack);
   }

   private void OnClickBack()
   {
      _object.SetActive(false);
      
      if(_activeObject)
         _activeObject.SetActive(true);
   }
}
