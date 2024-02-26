using UnityEngine;
using System.IO;
using SgLibUnite.Singleton;

namespace TeamC
{
    /// <summary> プレイヤーのセーブデータ管理コンポーネントのSuperクラス </summary>
    public class ClientDataSaverSuperClass : SingletonBaseClass<ClientDataSaverSuperClass>
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
