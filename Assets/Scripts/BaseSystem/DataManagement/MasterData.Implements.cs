using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using static Network.WebRequest;
using Cysharp.Threading.Tasks;


namespace DataManagement
{
    /// <summary>
    /// マスターデータ管理クラス
    /// </summary>
    public partial class MasterData
    {
        //設定系
        const string DataPrefix = "DataAsset/MasterData";


        //マスターデータ読み込みリスト
        public static TextMaster TextMaster { get; private set; }
        public static EnemyMaster EnemyMaster { get; private set; }


        async UniTask MasterDataLoad()
        {
            //マスタ読み込み
            TextMaster = new TextMaster();
            EnemyMaster = new EnemyMaster();

            await UniTask.WhenAll(new List<UniTask>()
            {
                TextMaster.Marshal(),
                EnemyMaster.Marshal(),
            });
        }
    }
}