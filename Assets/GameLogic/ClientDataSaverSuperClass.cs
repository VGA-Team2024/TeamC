using System;
using UnityEngine;

namespace TeamC
{
    /// <summary> プレイヤーのセーブデータ管理コンポーネントのSuperクラス </summary>
    public class ClientDataSaverSuperClass : MonoBehaviour, IDataSaver
    {
        public ClientDataTemplate ReadData() // called by GL
        {
        }

        public void SaveData()
        {
        }
    }
}