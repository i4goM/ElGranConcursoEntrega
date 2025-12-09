using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Xml.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeaderboardManager : MonoBehaviour
{
    public Transform container;
    public TMP_Text entryTemplate;
    public GameObject namePanel;
    public TMP_InputField inputName;
    public Button buttonConfirm;

    void Start()
    {
        // Limpiar sin destruir template
        LimpiarContainer();

        namePanel.SetActive(true);
        buttonConfirm.onClick.AddListener(ConfirmarNombre);
    }

    void LimpiarContainer()
    {
        // Destruir solo hijos que NO son el template
        List<GameObject> hijosADestruir = new List<GameObject>();

        foreach (Transform child in container)
        {
            if (child.gameObject != entryTemplate.gameObject)
            {
                hijosADestruir.Add(child.gameObject);
            }
        }

        foreach (GameObject hijo in hijosADestruir)
        {
            Destroy(hijo);
        }
    }

    void ConfirmarNombre()
    {
        string nomJugador = inputName.text.Trim();

        if (string.IsNullOrEmpty(nomJugador))
            return;

        PlayerPrefs.SetString("PlayerName", nomJugador);
        PlayerPrefs.Save();

        namePanel.SetActive(false);

        AñadirJugadorAlLeaderboard(nomJugador);
        LoadLeaderboard();
    }

    void AñadirJugadorAlLeaderboard(string nombreJugador)
    {
        try // Informes
        {
            string kahootName = PlayerPrefs.GetString("SelectedKahootName");
            string folder = Path.Combine(Application.persistentDataPath, "Leaderboard");
            string filePath = Path.Combine(folder, kahootName + ".xml");

            // Lineas para saber la ruta a los xml
            Debug.Log("=== RUTA LEADERBOARD ===");
            Debug.Log("Carpeta: " + folder);
            Debug.Log("Archivo: " + filePath);
            Debug.Log("Persistent Data Path: " + Application.persistentDataPath);

            LeaderboardData datos;

            if (File.Exists(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(LeaderboardData));
                using (FileStream stream = new FileStream(filePath, FileMode.Open))
                {
                    datos = serializer.Deserialize(stream) as LeaderboardData;
                }
            }
            else
            {
                datos = new LeaderboardData();
                datos.entries = new List<LeaderboardEntry>();
            }

            LeaderboardEntry nueva = new LeaderboardEntry();
            nueva.Name = nombreJugador;
            nueva.Score = PlayerPrefs.GetInt("PlayerScore", 0);
            nueva.Time = PlayerPrefs.GetFloat("PlayerTime", 0f);

            datos.entries.Add(nueva);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            XmlSerializer serializer2 = new XmlSerializer(typeof(LeaderboardData));
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                serializer2.Serialize(stream, datos);
            }
        }
        catch (System.Exception ex)
        {
            ErrorLogger.LogException(ex, "Añadir Jugador a Leaderboard");
            CrearTexto("Error guardando datos");
        }
    }

    void LoadLeaderboard()
    {
        try
        {
            LimpiarContainer(); // Usar función segura

            string kahootName = PlayerPrefs.GetString("SelectedKahootName");
            string folder = Path.Combine(Application.persistentDataPath, "Leaderboard");
            string filePath = Path.Combine(folder, kahootName + ".xml");

            if (!File.Exists(filePath))
            {
                CrearTexto("No hi ha dades encara.");
                return;
            }

            try
            {
                LeaderboardData data;
                XmlSerializer serializer = new XmlSerializer(typeof(LeaderboardData));
                using (FileStream stream = new FileStream(filePath, FileMode.Open))
                {
                    data = serializer.Deserialize(stream) as LeaderboardData;
                }

                if (data == null || data.entries == null || data.entries.Count == 0)
                {
                    CrearTexto("No hi ha dades encara.");
                    return;
                }

                data.entries.Sort((a, b) => b.Score.CompareTo(a.Score));

                int posicio = 1;
                foreach (var e in data.entries)
                {
                    TMP_Text entry = Instantiate(entryTemplate, container);
                    entry.text = $"{posicio}. {e.Name}  -  {e.Score} punts  -  {e.Time:0.0}s";
                    entry.gameObject.SetActive(true);
                    posicio++;
                }
            }
            catch (System.Exception ex)
            {
                ErrorLogger.LogException(ex, "Cargar Datos Leaderboard");
                CrearTexto("Error carregant dades");
            }
        }
        catch (System.Exception ex)
        {
            ErrorLogger.LogException(ex, "Cargar Leaderboard");
            CrearTexto("Error cargando datos");
        }
    }

    void CrearTexto(string mensaje)
    {
        if (entryTemplate == null) return; // Protección extra

        // Asegurar que el template esté activo
        if (!entryTemplate.gameObject.activeSelf)
        {
            entryTemplate.gameObject.SetActive(true);
        }

        TMP_Text t = Instantiate(entryTemplate, container);
        t.text = mensaje;
        t.gameObject.SetActive(true);
    }

    public void changeSceneHome()
    {
        SceneManager.LoadScene("MainMenu");
    }
}