using UnityEngine;

namespace Ending
{
    public class EndingController : MonoBehaviour
    {
        [SerializeField] private EndingSequence _endingSequence;
        private async void OnEnable()
        {
           await _endingSequence.PlayEnding();
        }
    }
}