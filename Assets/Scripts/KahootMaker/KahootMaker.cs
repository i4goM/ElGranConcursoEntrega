using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class KahootMakerSimple : MonoBehaviour
{
    public TMP_InputField titleInput;
    public TMP_InputField descInput;
    public TMP_InputField questionInput;
    public TMP_InputField[] answerInputs = new TMP_InputField[4]; // A,B,C,D
    public TMP_Dropdown correctDropdown;
    public TMP_InputField timeInput;

    public Button addButton;
    public Button saveButton;
    public TMP_Text counterText;

    private List<Question> questions = new List<Question>();
    private int questionCount = 0;

    void Start()
    {
        // Asignar listeners SOLO a las funciones correctas
        addButton.onClick.AddListener(AddQuestionToList);
        saveButton.onClick.AddListener(SaveKahoot);

        UpdateCounter();
    }

    void AddQuestionToList()
    {
        // Validar que haya pregunta
        if (string.IsNullOrEmpty(questionInput.text))
        {
            Debug.Log("Escribe una pregunta primero");
            return;
        }

        // Validar respuestas
        foreach (var input in answerInputs)
        {
            if (string.IsNullOrEmpty(input.text))
            {
                Debug.Log("Completa todas las respuestas");
                return;
            }
        }

        // Crear objeto Answer
        Answer ans = new Answer
        {
            a = answerInputs[0].text,
            b = answerInputs[1].text,
            c = answerInputs[2].text,
            d = answerInputs[3].text
        };

        // Crear objeto Question
        Question q = new Question
        {
            statement = questionInput.text,
            answers = new List<Answer> { ans },
            rightAnswer = GetCorrectLetter(),
            duration = int.Parse(timeInput.text)
        };

        // Añadir a lista
        questions.Add(q);
        questionCount++;
        UpdateCounter();

        // Limpiar campos para nueva pregunta
        questionInput.text = "";
        foreach (var input in answerInputs)
            input.text = "";
        correctDropdown.value = 0;
        timeInput.text = "30";

        // Poner foco en pregunta
        questionInput.Select();
        questionInput.ActivateInputField();
    }

    string GetCorrectLetter()
    {
        return correctDropdown.value switch
        {
            0 => "a",
            1 => "b",
            2 => "c",
            3 => "d",
            _ => "a"
        };
    }

    void UpdateCounter()
    {
        counterText.text = "Preguntas añadidas: " + questionCount;
    }

    void SaveKahoot()
    {
        // Validar título
        if (string.IsNullOrEmpty(titleInput.text))
        {
            Debug.Log("¡Pon un título al kahoot!");
            return;
        }

        // Validar que haya preguntas
        if (questions.Count == 0)
        {
            Debug.Log("¡Añade al menos una pregunta!");
            return;
        }

        // Crear objeto Kahoot
        KahootData kahoot = new KahootData
        {
            title = titleInput.text,
            description = descInput.text,
            questions = questions
        };

        // Convertir a JSON
        string json = JsonUtility.ToJson(kahoot, true);

        // Crear carpeta si no existe
        string folder = Path.Combine(Application.dataPath, "Kahoots");
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        // Guardar archivo
        string filePath = Path.Combine(folder, titleInput.text + ".json");
        File.WriteAllText(filePath, json);

        Debug.Log("¡Kahoot guardado! " + filePath);

        // ESPERAR un momento y luego volver al menú
        StartCoroutine(ReturnToMenu());
    }

    System.Collections.IEnumerator ReturnToMenu()
    {
        // Esperar 0.5 segundos para ver el mensaje
        yield return new WaitForSeconds(0.5f);

        // SOLO AQUÍ se carga MainMenu
        SceneManager.LoadScene("MainMenu");
    }
}