using System;
using UnityEngine;

namespace TeamC
{
    /// <summary> プレイヤーのセーブデータ管理コンポーネントのSuperクラス </summary>
    public class ClientDataSaverSuperClass : MonoBehaviour, IDataSaver
    {
        private Func<ClientDataTemplate> OnReadData; // Action -> Func
        private Action OnSaveData; // Action<ClientDataTemplate> -> Action

        /// <summary> セーブデータの読み込み処理をここへデリデート登録 </summary>
        protected event Func<ClientDataTemplate> EventReadData
        {
            add { OnReadData += value; }
            remove { OnReadData -= value; }
        }

        /// <summary> セーブデータの読み込みをここへデリゲート登録 </summary>
        protected event Action EventSaveData
        {
            add { OnSaveData += value; }
            remove { OnSaveData -= value; }
        }

        public ClientDataTemplate ReadData() // called by GL
        {
            return OnReadData();
        }

        public void SaveData()
        {
            OnSaveData();
        }
    }
}