using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private UIButton _changeSceneButton;
    [SerializeField] private string _sceneName;

    public void Start()
    {
        _changeSceneButton.OnClickAddListener(SceneChange);
    }

    private void SceneChange()
    {
        SceneManager.LoadScene(_sceneName);
    }
}
