using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    public void ChangeScene(string _SceneName)
    {
        SceneManager.LoadScene(_SceneName);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
