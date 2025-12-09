using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerKahootSelector : MonoBehaviour
{
    // **** UI **** //
    public GameObject cardPrefab;      // prefab de tarjeta
    public Transform cardContainer;    // contenedor con Vertical Layout Group

    // *** JSON *** //
    public string folderName = "Kahoots";  // Carpeta dentro de Assets/Kahoots

    public void changeSceneLeaderBoard()
    {
        SceneManager.LoadScene("LeaderBoard");
    }

    public void changeSceneAbout()
    {
        SceneManager.LoadScene("About");
    }

    void Start()
    {
        LoadKahootCards();
    }

    void LoadKahootCards()
    {
        try
        {
            string path = Path.Combine(Application.dataPath, folderName);

            if (!Directory.Exists(path))
            {
                Debug.LogError("La carpeta no existe: " + path);
                return;
            }

            string[] files = Directory.GetFiles(path, "*.json");

            foreach (string file in files)
            {
                string json = File.ReadAllText(file);
                KahootData data = JsonUtility.FromJson<KahootData>(json);
                CreateCard(data, file);
            }
        }
        catch (System.Exception ex)
        {
            ErrorLogger.LogException(ex, "Cargar Kahoots en Selector");
        }
    }

    void CreateCard(KahootData data, string filePath)
    {
        GameObject card = Instantiate(cardPrefab, cardContainer);
        KahootCard ui = card.GetComponent<KahootCard>();

        if (ui != null)
        {
            ui.SetInfo(data.title, data.description, filePath);
        }
    }


}
