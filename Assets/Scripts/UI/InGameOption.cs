using UnityEngine;

public class InGameOption : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TitleBack();
        }
    }

    private void TitleBack()
    {
        SceneLoader.LoadSceneSimple(_sceneName);
    }
}