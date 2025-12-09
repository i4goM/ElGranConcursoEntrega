using UnityEngine;
using UnityEngine.SceneManagement;

public class LinkButton : MonoBehaviour
{
    public void OpenLink(string url)
    {
        Application.OpenURL(url);
    }

    public void changeSceneMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}