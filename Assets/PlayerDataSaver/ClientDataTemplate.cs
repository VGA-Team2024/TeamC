using System;
using UnityEngine;

namespace TeamC
{
    [Serializable]
    /// <summary> クライアントのデータのひな形 </summary>
    public class ClientDataTemplate : MonoBehaviour
    {
        public int _savePlayerThroughtFloor;
        public decimal _savePlayerGold;
        public int _saveWizardLevel;
        public int _saveWarriorLevel;
        public int _saveThiefLevel;
        public int _savePoetLevel;
        public int _saveHermitLevel;
        public decimal _saveCurrentBossHP;

        public ClientDataTemplate() { }

        public ClientDataTemplate(int playerThroughtFloor, int playerGold, int wizardLevel, int warriorLevel,
        int thiefLevel, int poetLevel, int hermitLevel, int bossHP)
        {
            this. _savePlayerThroughtFloor = playerThroughtFloor;
            this._savePlayerGold = playerGold;
            this._saveWizardLevel = wizardLevel;
            this._saveWarriorLevel = warriorLevel;
            this._saveThiefLevel = thiefLevel;
            this._savePoetLevel = poetLevel;
            this._saveHermitLevel = hermitLevel;
            this._saveCurrentBossHP = bossHP;
        }
    }
}
