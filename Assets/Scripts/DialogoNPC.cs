using System.Collections;
using UnityEngine;
using TMPro;

public class DialogoNPC : MonoBehaviour
{
    private bool jugadorEnZona;
    private bool dialogoIniciado;
    private int lineasIndex;

    [SerializeField] private GameObject marcaDialogo;
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text textoDialogo;
    [SerializeField, TextArea(1, 8)] private string[] lineasDialogo;
    [SerializeField] private playerController controller;
    [SerializeField] private playerStateController estadoJugador;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (jugadorEnZona && Input.GetButtonDown("Fire1"))
        {
            if (!dialogoIniciado)
            {
                EmpezarDialogo();
            }
            else if (textoDialogo.text == lineasDialogo[lineasIndex])
            {
                SiguienteLinea();
            }
            else
            {
                StopAllCoroutines();
                textoDialogo.text = lineasDialogo[lineasIndex];
            }
        }
    }

    private void EmpezarDialogo()
    {
        dialogoIniciado = true;
        panel.SetActive(true);
        marcaDialogo.SetActive(false);
        lineasIndex = 0;
        StartCoroutine(ShowLine());

        // Ignora los inputs de movimiento en el controlador
        controller.isInDialogue = true;

        // Desactiva el script de estado del jugador
        estadoJugador.enabled = false;

        // Detienen la animación de caminar
        Animator animatorPlayer = estadoJugador.GetComponent<Animator>();
        if (animatorPlayer != null)
        {
            animatorPlayer.SetBool("isWalking", false);
        }

        // Animación "Hablando" de secretaria
        animator.SetBool("isTalking", true);
    }

    private void SiguienteLinea()
    {
        lineasIndex++;
        if (lineasIndex < lineasDialogo.Length)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            dialogoIniciado = false;
            panel.SetActive(false);
            marcaDialogo.SetActive(true);

            // Reactiva los inputs de movimiento en el controlador
            controller.isInDialogue = false;

            // Reactiva el script de estado del jugador
            estadoJugador.enabled = true;

            // Congela al jugador en una posición válida
            controller.GetComponent<CharacterController>().Move(Vector3.zero);
            animator.SetBool("isTalking", false);
        }
    }

    private IEnumerator ShowLine()
    {
        textoDialogo.text = string.Empty;
        foreach (char ch in lineasDialogo[lineasIndex])
        {
            textoDialogo.text += ch;
            yield return new WaitForSeconds(0.05f); // Velocidad del diálogo
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            jugadorEnZona = true;
            marcaDialogo.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            jugadorEnZona = false;
            marcaDialogo.SetActive(false);
        }
    }
}