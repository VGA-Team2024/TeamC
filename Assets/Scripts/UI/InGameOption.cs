using UnityEngine;

public class InGameOption : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TitleBack();
        }
    }

    private void TitleBack()
    {
        SceneLoader.LoadSceneSimple("Stage1_FairyForest");
    }
}
