using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManagerVisorInformes : MonoBehaviour
{
    public void changeSceneHome()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void changeSceneSelector()
    {
        SceneManager.LoadScene("SelectorInformes");
    }
}
