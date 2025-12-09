using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SelectorInformesManager : MonoBehaviour
{
    public Transform container;  // Vertical Layout Group
    public GameObject logCardPrefab; // Prefab de LogCard

    void Start()
    {
        LoadLogs();
    }

    void LoadLogs()
    {
        string logFolder = Path.Combine(Application.persistentDataPath, "Logs");

        if (!Directory.Exists(logFolder))
        {
            Debug.Log("No hay informes todavía.");
            return;
        }

        string[] files = Directory.GetFiles(logFolder, "*.txt");

        // Ordenar: el + reciente arriba
        System.Array.Sort(files);
        System.Array.Reverse(files);

        foreach (string file in files)
        {
            GameObject card = Instantiate(logCardPrefab, container);
            CartaInformes ui = card.GetComponent<CartaInformes>();

            string fileName = Path.GetFileName(file);

            ui.SetInfo(fileName, file);
        }
    }

}
