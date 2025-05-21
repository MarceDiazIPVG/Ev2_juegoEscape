using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ScriptMenu : MonoBehaviour
{
    public void iniciarJuego()
    {
        SceneManager.LoadScene("JUEGO");
    }

    public void salir()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif

        Debug.Log("Saliendo del juego");
    }


    public void volverAlMenu()
    {
        SceneManager.LoadScene("MenuEV2"); 
    }
}
