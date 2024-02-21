using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TeamC
{
    /// <summary> プレイヤーのセーブデータ管理の機能を提供する </summary>
    public class ClientDataSaver : ClientDataSaverSuperClass
    {
        // ここのクラスで
        // EventReadData、EventSaveData
        // の処理の本体を実装する。
        // base.EventReadData,base.EventSaveData
        // へ機能の本体の関数をデリゲート登録すればよい
        // JSON形式のファイルをセーブする処理を書けばよい
        // Application.dataPath + セーブデータファイル名.json
        // でファイル名を指定するとよい。Application.dataPathはエディタ上でのＡｓｓｅｔｓディレクトリ
        // 直下のファイルパス

        private void EventSaveData(ClientDataTemplate data)
        {
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
            string filePath = Application.persistentDataPath + "/saveData.json"; // ファイルパス
            File.WriteAllText(filePath, jsonData);

            Debug.Log("Game saved!"); // 保存されたことをログに出力
        }
    }
}
