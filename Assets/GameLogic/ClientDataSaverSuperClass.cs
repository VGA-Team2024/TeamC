using UnityEngine;
using System.IO;
using SgLibUnite.Singleton;

namespace TeamC
{
    /// <summary> プレイヤーのセーブデータ管理コンポーネントのSuperクラス </summary>
    public class ClientDataSaverSuperClass : SingletonBaseClass<ClientDataSaverSuperClass>, IDataSaver
    {
        string filePath = Application.dataPath + "/saveData.json"; // ファイルパス

        public ClientDataTemplate ReadData() // called by GL
        {
            string dataStr = "";
            try
            {
                dataStr = File.ReadAllText(filePath);
                
                StreamReader sr = new StreamReader(filePath);
                dataStr = sr.ReadToEnd();
                sr.Close();
                return JsonUtility.FromJson<ClientDataTemplate>(dataStr);
            }
            catch (FileNotFoundException)
            {
                var data = new ClientDataTemplate();
                data = default;
                return data;
            }
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
            string jsonStr = JsonUtility.ToJson(data);

            StreamWriter sw = new StreamWriter(filePath, false);
            sw.WriteLine(jsonStr);
            sw.Flush();
            sw.Close();
            Debug.Log("Game saved!"); // 保存されたことをログに出力
        }

        protected override void ToDoAtAwakeSingleton()
        {
        }
    }
}
