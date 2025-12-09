using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CartaInformes : MonoBehaviour
{
    public TMP_Text titleText;
    public Button button;

    private string filePath;

    public void SetInfo(string title, string path)
    {
        titleText.text = title;
        filePath = path;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickCard);
    }

    void OnClickCard()
    {
        PlayerPrefs.SetString("SelectedLogFile", filePath);
        PlayerPrefs.Save();

        UnityEngine.SceneManagement.SceneManager.LoadScene("VisorInformes");
    }
}
