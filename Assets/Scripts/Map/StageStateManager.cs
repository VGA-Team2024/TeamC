using UniRx;
using UnityEngine;

public class StageStateManager : MonoBehaviour
{
    private OnTriggerEvent _triggerEvent;
    private ReactiveProperty<StageEnum> _currentStageState = new ReactiveProperty<StageEnum>(StageEnum.Forest);

    public IReadOnlyReactiveProperty<StageEnum> CurrentStageState => _currentStageState;

    private void Start()
    {
        _triggerEvent = FindObjectOfType<OnTriggerEvent>();
        
        _triggerEvent.OnTriggerEnterAsObservable
            .Where(x => x.CompareTag("StageArea"))
            .DistinctUntilChanged()
            .Subscribe(collder =>
            {
                var stageStateChanger = collder.gameObject.GetComponent<StageStateSetter>();
                SetStageState(stageStateChanger.StageEnum);
            }).AddTo(this);
    }
    
    private void SetStageState(StageEnum newState)
    {
        _currentStageState.Value = newState;
    }
}

public enum StageEnum
{
    OutGame,
    Forest,
    Castle,
    Moriss,
    Doora,
    Nancy,
    Ending
}
