using UniRx;
using UnityEngine;

public class StageStateManager : MonoBehaviour
{
    [SerializeField] private GameObject _optionCanvas;
    private OnTriggerEvent _triggerEvent;
    private ReactiveProperty<StageEnum> _currentStageState = new ReactiveProperty<StageEnum>();

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_optionCanvas.activeSelf)
        {
            _optionCanvas.SetActive(true);
        }
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
