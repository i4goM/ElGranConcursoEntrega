using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonsMainMenu : MonoBehaviour
{

    public void changeSceneKahootSelector()
    {
        SceneManager.LoadScene("KahootSelector");
    }

    public void changeSceneLeaderBoard()
    {
        SceneManager.LoadScene("LeaderBoard");
    }

    public void changeSceneAbout()
    {
        SceneManager.LoadScene("About");
    }
    
    public void changeSceneExit()
    {
        Application.Quit();
    }
}
