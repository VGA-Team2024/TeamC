using UnityEngine;
using System.IO;
using TeamC;

[System.Serializable]
public class GameData
{
    public int _savePlayerLevel;
    public int _savePlayerGold;
    public int _saveWizardLevel;
    public int _saveWarriorLevel;
    public int _saveThiefLevel;
    public int _savePoetLevel;
    public int _saveHermitLevel;
    public int _saveCurrentFloor;
    public int _saveCurrentBossHP;

    public GameData(int playerLevel, int playerGold, int wizardLevel,int warriorLevel,
        int thiefLevel,int poetLevel,int hermitLevel,int currentFloor, int bossHP)
    {
        this._savePlayerLevel = playerLevel;
        this._savePlayerGold = playerGold;
        this._saveWizardLevel = wizardLevel;
        this._saveWarriorLevel = warriorLevel;
        this._saveThiefLevel = thiefLevel;
        this._savePoetLevel = poetLevel;
        this._saveHermitLevel = hermitLevel;
        this._saveCurrentFloor = currentFloor;
        this._saveCurrentBossHP = bossHP;
    }
}

public class AutoSave : MonoBehaviour
{
    private float saveInterval = 60f; // 自動保存間隔（秒）
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= saveInterval)
        {
            SaveGame(); // 自動保存を実行
            timer = 0f;
        }
    }

    private void SaveGame()
    {
        // プレイヤーの状態を取得
        int playerLevel = PlayerSuperClass.level;
        int playerGold = PlayerSuperClass.gold;
        //各NPCの状態を取得
        int wizardLevel = Wizard.level;
        int warriorLevel = Warrior.level;
        int thiefLevel = Thief.level;
        int poetLevel = Poet.level;
        int hermitLevel = Hermit.level;
        //現在フロアの階層を取得
        int currentFloor = GameManager.currentFloor;
        // ボスのHPを取得
        int bossHP = BossSuperClas.bossHp;


        // GameDataオブジェクトを作成
        GameData gameData = 
            new GameData( playerLevel, playerGold, wizardLevel, warriorLevel,
                          thiefLevel, poetLevel, hermitLevel, currentFloor, bossHP);

        // JSON形式に変換して保存
        string jsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + "/saveData.json"; // ファイルパス
        File.WriteAllText(filePath, jsonData);

        Debug.Log("Game saved!"); // 保存されたことをログに出力
    }
}
