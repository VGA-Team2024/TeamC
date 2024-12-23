using UnityEngine;

public class StageStateSetter : MonoBehaviour
{
    [SerializeField, InspectorVariantName("このエリアのステート")] private StageEnum _stageEnum;

    public StageEnum StageEnum => _stageEnum;
}