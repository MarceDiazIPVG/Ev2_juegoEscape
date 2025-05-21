using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Registros : MonoBehaviour
{
    public TMPro.TMP_InputField input;
    public TMPro.TMP_Dropdown dropdown;
    public Slider slider;
    public AudioSource audioSource;

    private static Registros instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);  
        }
    }

    public void getdropdown()
    {
        int valor = dropdown.value;
        string dificultad = dropdown.options[valor].text;

        PlayerPrefs.SetInt("DificultadSeleccionada", valor);
        PlayerPrefs.Save();

        Debug.Log("Dificultad guardada: " + dificultad + " (Índice: " + valor + ")");
    }

    public void GuardarNombre()
    {
        string nombre = input.text;
        PlayerPrefs.SetString("NombreJugador", nombre);
        PlayerPrefs.Save();
        Debug.Log("Nombre guardado: " + nombre);
    }

    public void setAudio()
    {
        float volumen;
        volumen = slider.value;
        audioSource.volume = volumen;
        Debug.Log("El volumen es: " + volumen);
    }

    public void IniciarJuego()
    {
        string nombre = input.text;

        if (string.IsNullOrEmpty(nombre))
        {
            Debug.LogWarning("El nombre del jugador no puede estar vacío");
            return;
        }

        PlayerPrefs.SetString("NombreJugador", nombre);
        PlayerPrefs.Save();

        getdropdown();

        SceneManager.LoadScene("JUEGO");
    }
}
