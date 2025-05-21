using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IA_Enemigo : MonoBehaviour
{
   public NavMeshAgent agenteNavegacion;
    public Transform[] destinos;
    private int i = 0;

    public bool seguirJugador = true;
    private GameObject player;
    private float distanciaJugador;
    public float distanciaMaxima = 10f;

    private float velocidadOriginal;
    private bool velocidadReducida = false;
    public AudioSource audioSource;

    void Start()
    {
        if (destinos.Length > 0)
        {
            agenteNavegacion.destination = destinos[0].position;
        }

        player = GameObject.FindGameObjectWithTag("Player");
        
        int dificultad = PlayerPrefs.GetInt("DificultadSeleccionada", 0);
        float multiplicador = 1f;

        switch (dificultad)
        {
            case 0:
                multiplicador = 1f; // F�cil
                break;
            case 1:
                multiplicador = 2f; // Medio
                break;
            case 2:
                multiplicador = 3f; // Dif�cil
                break;
        }

        // Aplicar velocidad ajustada por dificultad
        velocidadOriginal = agenteNavegacion.speed * multiplicador;
        agenteNavegacion.speed = velocidadOriginal;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (player == null) return;

        distanciaJugador = Vector3.Distance(transform.position, player.transform.position);

        if (distanciaJugador <= distanciaMaxima && seguirJugador)
        {
            agenteNavegacion.SetDestination(player.transform.position);
        }
        else
        {
            MoverPorRuta();
        }
    }

    void MoverPorRuta()
    {
        if (destinos.Length == 0) return;

        if (!agenteNavegacion.pathPending && agenteNavegacion.remainingDistance < 0.5f)
        {
            i = (i + 1) % destinos.Length;
            agenteNavegacion.SetDestination(destinos[i].position);
        }


        if (Vector3.Distance(transform.position, destinos[i].position) < 1f)
        {
            i = (i + 1) % destinos.Length;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioSource != null)
            {
                audioSource.Play();
            }

            VidaPlayer playerLife = other.GetComponent<VidaPlayer>();

            if (playerLife != null)
            {
                bool fueUltimoGolpe = playerLife.ReceiveHit();

                if (fueUltimoGolpe)
                {
                    Destroy(gameObject);
                }
                else
                {
                    if (!velocidadReducida)
                    {
                        StartCoroutine(ReducirVelocidadTemporalmente());
                    }
                }
            }
        }
    }

    IEnumerator ReducirVelocidadTemporalmente()
    {
        velocidadReducida = true;
        agenteNavegacion.speed *= 0.5f;
        yield return new WaitForSeconds(3f);
        agenteNavegacion.speed = velocidadOriginal;
        velocidadReducida = false;
    }
}