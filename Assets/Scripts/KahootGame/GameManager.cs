using System.Collections;
using System.IO;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Serialization;

public class GameManager : MonoBehaviour
{
    // *** TIME *** //
    [SerializeField] float tiempo;

    // *** BUTTONS *** //
    public Button[] botones;
    public TMP_Text[] textosBotones;
    private int indicePregunta = 0;

    // *** TEXT *** //
    [SerializeField] TMP_Text tiempoText;
    [SerializeField] TMP_Text PreguntaText;
    private KahootData kahoot;
    private Question pregunta;

    private int puntuacionFinal = 0;
    private float tiempoTotal = 0f;

    private void Start()
    {
        CargarKahoot();
        CargarPregunta(indicePregunta);
    }

    private void Update()
    {
        tiempo -= Time.deltaTime;
        tiempoTotal += Time.deltaTime;

        int mins = Mathf.FloorToInt(tiempo / 60);
        int sec = Mathf.FloorToInt(tiempo % 60);

        tiempoText.text = string.Format("{0:00}:{1:00}", mins, sec);
    }

    void CargarKahoot()
    {
        try // esto es para los informes
        {
            string path = PlayerPrefs.GetString("SelectedKahootPath");
            string json = File.ReadAllText(path);

            kahoot = JsonUtility.FromJson<KahootData>(json);
        }
        catch (System.Exception ex)
        {
            ErrorLogger.LogException(ex, "Cargar Kahoot en Game");
        }
    }

    void CargarPregunta(int index)
    {
        pregunta = kahoot.questions[index];
        indicePregunta = index;

        PreguntaText.text = pregunta.statement;
        tiempo = pregunta.duration;

        // Limpiar listeners
        foreach (var btn in botones)
            btn.onClick.RemoveAllListeners();

        // Obtener TODAS las respuestas
        if (pregunta.answers.Count > 0)
        {
            Answer respuestas = pregunta.answers[0];

            // Asignar cada respuesta a un botón
            textosBotones[0].text = !string.IsNullOrEmpty(respuestas.a) ? respuestas.a : "";
            textosBotones[1].text = !string.IsNullOrEmpty(respuestas.b) ? respuestas.b : "";
            textosBotones[2].text = !string.IsNullOrEmpty(respuestas.c) ? respuestas.c : "";
            textosBotones[3].text = !string.IsNullOrEmpty(respuestas.d) ? respuestas.d : "";

            // Asignar listeners
            botones[0].onClick.AddListener(() => ComprobarRespuesta("a"));
            botones[1].onClick.AddListener(() => ComprobarRespuesta("b"));
            botones[2].onClick.AddListener(() => ComprobarRespuesta("c"));
            botones[3].onClick.AddListener(() => ComprobarRespuesta("d"));
        }

    }

    private (string id, string text) ExtraerRespuesta(Answer ans)
    {
        if (!string.IsNullOrEmpty(ans.a)) return ("a", ans.a);
        if (!string.IsNullOrEmpty(ans.b)) return ("b", ans.b);
        if (!string.IsNullOrEmpty(ans.c)) return ("c", ans.c);
        if (!string.IsNullOrEmpty(ans.d)) return ("d", ans.d);
        return ("", "");
    }

    void ComprobarRespuesta(string idSeleccionado)
    {
        if (idSeleccionado == pregunta.rightAnswer)
        {
            Debug.Log("CORRECTA");
            puntuacionFinal += 10; // si la pregunta es correcta, es sumen 10 punts
            StartCoroutine(MostrarResultadoYSeguir("CORRECTO!"));
        }
        else
        {
            Debug.Log("INCORRECTA");
            puntuacionFinal += 0;
            StartCoroutine(MostrarResultadoYSeguir("INCORRECTO!"));
        }
    }

    IEnumerator MostrarResultadoTemporal(string mensaje)
    {
        // Guardem el text original
        string textoOriginal = PreguntaText.text;

        // Mostrem el resultat
        PreguntaText.text = mensaje;

        yield return new WaitForSeconds(2f);

        PreguntaText.text = textoOriginal;

        CargarPregunta(indicePregunta + 1);
    }

    IEnumerator MostrarResultadoYSeguir(string mensaje)
    {
        // Mostrem el text de correcte/incorrecte
        string textoOriginal = PreguntaText.text;
        PreguntaText.text = mensaje;

        foreach (var btn in botones)
        {
            btn.interactable = false;
        }

        yield return new WaitForSeconds(2f);

        foreach (var btn in botones)
        {
            btn.interactable = true;
        }

        // Comprovem si hi ha més preguntes
        if (indicePregunta + 1 < kahoot.questions.Count)
        {
            // Anar a la següent pregunta
            CargarPregunta(indicePregunta + 1);
        }
        else
        {
            PlayerPrefs.SetInt("PlayerScore", puntuacionFinal);
            PlayerPrefs.SetFloat("PlayerTime", tiempoTotal);
            PlayerPrefs.SetString("SelectedKahootName", kahoot.title);
            PlayerPrefs.Save();

            // Si no, anar a LeaderBoard i guardar els resultats
            UnityEngine.SceneManagement.SceneManager.LoadScene("LeaderBoard");
            GuardarResultado();
        }

        void GuardarResultado()
        {
            try
            {
                string kahootName = kahoot.title;
                string folder = Path.Combine(Application.persistentDataPath, "Leaderboard");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string filePath = Path.Combine(folder, kahootName + ".xml");

                LeaderboardData data;

                // Si ja existeix, el carreguem
                if (File.Exists(filePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(LeaderboardData));
                    using (FileStream stream = new FileStream(filePath, FileMode.Open))
                    {
                        data = serializer.Deserialize(stream) as LeaderboardData;
                    }
                }
                else
                {
                    data = new LeaderboardData();
                }

                // agafar nom de jugador (si no tens encara, posaré temporal)
                string playerName = PlayerPrefs.GetString("PlayerName", "Jugador");

                // crear entrada
                LeaderboardEntry entry = new LeaderboardEntry(playerName, puntuacionFinal, tiempoTotal);
                data.entries.Add(entry);

                // ordenar per puntuació descendent
                data.entries.Sort((a, b) => b.Score.CompareTo(a.Score));

                // guardar XML
                XmlSerializer serializer2 = new XmlSerializer(typeof(LeaderboardData));
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    serializer2.Serialize(stream, data);
                }
            }
            catch (System.Exception ex)
            {
                ErrorLogger.LogException(ex, "Guardar Resultado XML");
                Debug.LogError("Error guardando XML: " + ex.Message);
            }
        }
    }

}
