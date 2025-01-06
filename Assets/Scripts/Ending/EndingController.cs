using UnityEngine;

namespace Ending
{
    public class EndingController : MonoBehaviour
    {
        [SerializeField] private GameObject _nancyObject;
        
        private EndingSequence _endingSequence;
        
        private void Awake()
        {
            if (!_endingSequence)
                _endingSequence = FindAnyObjectByType<EndingSequence>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _nancyObject.SetActive(false);
                if (_endingSequence == null)
                {
                    Debug.LogError("Ending Sequence is null");
                }
                else
                {
                    Show();
                }
            }
        }

        private async void Show()
        {
            CRIAudioManager.BGM.Stop();
           await _endingSequence.PlayEnding();
        }
    }
}