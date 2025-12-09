using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class VisorInformesManager : MonoBehaviour
{
    public TMP_Text logText;

    void Start()
    {
        string path = PlayerPrefs.GetString("SelectedLogFile", "");

        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            logText.text = "No se pudo cargar el informe.";
            return;
        }

        try
        {
            string content = File.ReadAllText(path);
            logText.text = content;
        }
        catch (System.Exception ex)
        {
            logText.text = "Error leyendo informe:\n" + ex.Message;
        }
    }
}

