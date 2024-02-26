using System;
using UnityEngine;

namespace TeamC
{
    [Serializable]
    /// <summary> クライアントのデータのひな形 </summary>
    public class ClientDataTemplate : MonoBehaviour, IClientData
    {
        public decimal BossHP { get; set; }
        public int CurrentFloor { get; set; }
        public decimal CurrentGold { get; set; }
    }
}
