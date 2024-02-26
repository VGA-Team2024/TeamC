using UnityEngine;
using UnityEngine.Events;

namespace TeamC
{
    public class NPCCharClass : MonoBehaviour, INPC
    {
        
        public bool IsActive { get; }
        public UnityEvent GetNPCEffect { get; }
    }
}