using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene("01_Title");
    }
}
