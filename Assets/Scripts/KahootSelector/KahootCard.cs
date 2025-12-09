using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KahootCard : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Button button; // el que cubre la targeta para redirigir

    private string jsonPath;

    public void SetInfo(string title, string description, string filePath)
    {

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickCard);

        titleText.text = title;
        descriptionText.text = description;

        jsonPath = filePath;

    }

    void OnClickCard()
    {
        // Guardamos la ruta para la siguiente escena
        PlayerPrefs.SetString("SelectedKahootPath", jsonPath);
        PlayerPrefs.Save();

        // Cargamos la escena del juego
        SceneManager.LoadScene("KahootGame");
    }

}
