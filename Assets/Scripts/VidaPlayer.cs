using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class VidaPlayer : MonoBehaviour
{
    private int hitCount = 0;
    private bool isInvulnerable = false;
    private bool juegoTerminado = false;

    [Header("Tiempo")]
    [SerializeField] private TMP_Text textoTiempo;
    [SerializeField] private float tiempoTurno = 15f;
    private float tiempoRestante;

    [Header("Partículas")]
    [SerializeField] private GameObject particulasGolpe;

    [Header("UI Panels")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoriaPanel;
    [SerializeField] private TMP_Text textoFinalTiempo;

    void Start()
    {
        Time.timeScale = 1f;  // Asegura que el tiempo corre normalmente
        tiempoRestante = tiempoTurno;
        gameOverPanel.SetActive(false);
        victoriaPanel.SetActive(false);
    }

    void Update()
    {
        if (juegoTerminado) return;

        tiempoRestante -= Time.deltaTime;
        textoTiempo.text = "Tiempo: " + Mathf.CeilToInt(tiempoRestante).ToString();

        if (tiempoRestante <= 0f)
        {
            GameOverPorTiempo();
        }
    }

    public bool ReceiveHit()
    {
        if (isInvulnerable || juegoTerminado) return false;

        hitCount++;

        if (hitCount == 1)
        {
            Debug.Log("Primer golpe recibido");
            InstanciarParticulas();
            StartCoroutine(TemporarilyInvulnerable());
            return false;
        }
        else if (hitCount >= 2)
        {
            Debug.Log("Segundo golpe. GameOver");
            GameOver();
            return true;
        }

        return false;
    }

    void InstanciarParticulas()
    {
        if (particulasGolpe != null)
        {
            GameObject efecto = Instantiate(particulasGolpe, transform.position, Quaternion.identity);
            ParticleSystem ps = efecto.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
                Destroy(efecto, ps.main.duration + ps.main.startLifetime.constant);
            }
            else
            {
                Destroy(efecto, 2f);
            }
        }
    }

    IEnumerator TemporarilyInvulnerable()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(1.5f);
        isInvulnerable = false;
    }

    void GameOver()
    {
        juegoTerminado = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("Game Over por kamikaze");
    }

    void GameOverPorTiempo()
    {
        juegoTerminado = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("Game Over por tiempo agotado");
    }

    public void ReiniciarJuego()
    {
        Debug.Log("Reiniciando juego...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolverAlMenu()
    {
        Debug.Log("Volviendo al menú...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuEV2");  // Nombre exacto de la escena menú
    }

    void OnTriggerEnter(Collider other)
    {
        if (juegoTerminado) return;

        if (other.CompareTag("Meta"))
        {
            LlegarAMeta();
        }
    }

    void LlegarAMeta()
    {
        juegoTerminado = true;
        victoriaPanel.SetActive(true);
        Time.timeScale = 0f;

        float tiempoUsado = tiempoTurno - tiempoRestante;
        string nombreJugador = PlayerPrefs.GetString("NombreJugador", "Jugador");

        textoFinalTiempo.text = $"¡Has llegado a la meta!\nTiempo usado: {Mathf.CeilToInt(tiempoUsado)}s\nJugador: {nombreJugador}";

        Debug.Log("Victoria - Tiempo usado: " + tiempoUsado + " - Jugador: " + nombreJugador);
    }
}
