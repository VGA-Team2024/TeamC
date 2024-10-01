
using System.Collections.Generic;
using Unity.VisualScripting;

public class GameSettings
{
    public class SceneSetting
    {
        public string BaseSceneName;
        public List<string> AdditiveSceneName = new List<string>();
    }

    static public string MasterDataAPIURI => "https://script.google.com/macros/s/AKfycbw6J_mqIsEjQUq1iThp7mnul7UiWhZYDyil3jIr75WR0QK1h2DKgsmnPva9aXDYqvYX/exec";

    /// <summary>
    /// チームで個別に設定してください
    /// </summary>
    static Dictionary<string, SceneSetting> _sceneDic = new Dictionary<string, SceneSetting>()
    {
        {
            "Title" ,
            new SceneSetting(){
                BaseSceneName = "Title"
            }
        },
        {
            "Stage1" ,
            new SceneSetting(){
                BaseSceneName = "Stage1",
                AdditiveSceneName = new List<string>(){
                    "IngameSystem"
                }
            }
        },
        {
            "Stage2" ,
            new SceneSetting(){
                BaseSceneName = "Stage2",
                AdditiveSceneName = new List<string>(){
                    "IngameSystem"
                }
            }
        },
        {
            "Result" ,
            new SceneSetting(){
                BaseSceneName = "Result"
            }
        }
    };

    public static SceneSetting GetSetting(string key) => _sceneDic.ContainsKey(key) ? _sceneDic[key] : throw new KeyNotFoundException("SceneSettingのキーがありません");
}
