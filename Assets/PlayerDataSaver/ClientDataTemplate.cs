using System;
using UnityEngine;

namespace TeamC
{
    [Serializable]
    /// <summary> クライアントのデータのひな形 </summary>
    public class ClientDataTemplate
    {
        public int _savePlayerThroughtFloor;
        public string _savePlayerGold;
        public int _saveWizardLevel;
        public int _saveWarriorLevel;
        public int _saveThiefLevel;
        public int _savePoetLevel;
        public int _saveHermitLevel;
        public string _saveCurrentBossHP;
        public string _playerDamage;

        public ClientDataTemplate() { }

        public ClientDataTemplate(int playerThroughtFloor, decimal playerGold, int wizardLevel, int warriorLevel,
        int thiefLevel, int poetLevel, int hermitLevel, decimal bossHP, decimal dmg) 
        {
            this. _savePlayerThroughtFloor = playerThroughtFloor;
            this._savePlayerGold = playerGold.ToString();
            this._saveWizardLevel = wizardLevel;
            this._saveWarriorLevel = warriorLevel;
            this._saveThiefLevel = thiefLevel;
            this._savePoetLevel = poetLevel;
            this._saveHermitLevel = hermitLevel;
            this._saveCurrentBossHP = bossHP.ToString();
            this._playerDamage = dmg.ToString();
        }
    }
}
