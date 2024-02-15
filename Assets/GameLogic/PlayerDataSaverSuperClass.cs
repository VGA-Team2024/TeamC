using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamC
{
    /// <summary> プレイヤーのセーブデータ管理コンポーネントのSuperクラス </summary>
    public class PlayerDataSaverSuperClass : MonoBehaviour, IDataSaver
    {
        private Action<ClientDataTemplate> OnReadData;
        private Action<ClientDataTemplate> OnSaveData;

        /// <summary> セーブデータの読み込み処理をここへデリデート登録 </summary>
        protected event Action<ClientDataTemplate> EventReadData
        {
            add { OnReadData += value; }
            remove { OnReadData -= value; }
        }

        /// <summary> セーブデータの読み込みをここへデリゲート登録 </summary>
        protected event Action<ClientDataTemplate> EventSaveData
        {
            add { OnSaveData += value; }
            remove { OnSaveData -= value; }
        }

        public void ReadData(ClientDataTemplate clientData) // called by GL
        {
            OnReadData(clientData);
        }

        public void SaveData(ClientDataTemplate clientData) // called by GL
        {
            OnSaveData(clientData);
        }
    }
}