using System;
using UnityEngine;
using System.IO;

namespace TeamC
{
    /// <summary> プレイヤーのセーブデータ管理コンポーネントのSuperクラス </summary>
    public class ClientDataSaverSuperClass : MonoBehaviour, IDataSaver
    {
        string filePath = Application.dataPath + "/saveData.json"; // ファイルパス
        public ClientDataTemplate ReadData() // called by GL
        {
            string dataStr = "";
            try { dataStr = File.ReadAllText(filePath); }
            catch (FileNotFoundException)
            {
                File.WriteAllText(filePath, string.Empty);
            }

            var data = JsonUtility.FromJson<ClientDataTemplate>(dataStr);
            return data;
        }

        public void SaveData()
        {
            ClientDataTemplate data = new();

            // プレイヤーの状態を取得
            var player = GameObject.FindFirstObjectByType<Player>();
            int playerThroughtFloor = player.GetClearedFloorAmount();
            decimal playerGold = player.GetCurrentGold();

            //各NPCの状態を取得
            //Wizard
            var wizard = GameObject.FindFirstObjectByType<Wizard>();
            int wizardLevel = wizard.GetCurrentLevel();
            //Warrior
            var warrior = GameObject.FindFirstObjectByType<Warrior>();
            int warriorLevel = warrior.GetCurrentLevel();
            //Thief
            var thief = GameObject.FindFirstObjectByType<Thief>();
            int thiefLevel = thief.GetCurrentLevel();
            //Poet
            var poet = GameObject.FindFirstObjectByType<Poet>();
            int poetLevel = poet.GetCurrentLevel();
            //Hermit
            var hermit = GameObject.FindFirstObjectByType<Hermit>();
            int hermitLevel = hermit.GetCurrentLevel();
            // ボスのHPを取得
            var boss = GameObject.FindFirstObjectByType<Boss>();
            decimal bossHP = boss.GetHP;


            // Dataの値を初期化
            data._savePlayerThroughtFloor = playerThroughtFloor;
            data._savePlayerGold = playerGold;
            data._saveWizardLevel = wizardLevel;
            data._saveWarriorLevel = warriorLevel;
            data._saveThiefLevel = thiefLevel;
            data._savePoetLevel = poetLevel;
            data._saveHermitLevel = hermitLevel;
            data._saveCurrentBossHP = bossHP;


            // JSON形式に変換して保存
            string jsonData = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, jsonData);

            Debug.Log("Game saved!"); // 保存されたことをログに出力
        }
    }
}