using UnityEngine;
using System;
using System.IO;

public static class ErrorLogger
{
    private static string logFolder = Path.Combine(Application.persistentDataPath, "Logs");

    public static void LogException(Exception ex, string context = "General")
    {
        try
        {
            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);

            string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "_" + SanitizeFileName(context) + ".txt";
            string filePath = Path.Combine(logFolder, fileName);

            string content =
                "=== EXCEPCIÓN REGISTRADA ===\n" +
                "Fecha: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n" +
                "Contexto: " + context + "\n\n" +
                "Mensaje:\n" + ex.Message + "\n\n" +
                "StackTrace:\n" + ex.StackTrace;

            File.WriteAllText(filePath, content);

            Debug.LogError("Excepción registrada en: " + filePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Error escribiendo log: " + e.Message);
        }
    }

    private static string SanitizeFileName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');
        return name;
    }
}
