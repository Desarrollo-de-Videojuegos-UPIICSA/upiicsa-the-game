using System.Collections;
using UnityEngine;
using TMPro;

public class DialogoNPC : MonoBehaviour
{
    private bool jugadorEnZona;
    private bool dialogoIniciado;
    private int lineasIndex;
    private Vector3 playerPositionDuringDialogue;
    [SerializeField] private GameObject marcaDialogo;
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text textoDialogo;
    [SerializeField, TextArea(1, 8)] private string[] lineasDialogo;
    [SerializeField] private playerController controller;
    [SerializeField] private playerStateController estadoJugador;
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

        // Almacena la posición actual del jugador
        playerPositionDuringDialogue = controller.transform.position;

        // Desactiva el movimiento y las animaciones
        controller.enabled = false;
        estadoJugador.enabled = false;

        // Detienen la animación de caminar
        Animator animator = estadoJugador.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isWalking", false);
        }
    }
    private void LateUpdate()
    {
        if (dialogoIniciado)
        {
            controller.transform.position = playerPositionDuringDialogue;
        }
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

            // Reactiva el movimiento y las animaciones
            controller.enabled = true;
            estadoJugador.enabled = true;

            // Congela al jugador en una posición válida
            controller.GetComponent<CharacterController>().Move(Vector3.zero);
        }
    }
    private IEnumerator ShowLine()
    {
        textoDialogo.text = string.Empty;
        foreach (char ch in lineasDialogo[lineasIndex])
        {
            textoDialogo.text += ch;
            yield return new WaitForSeconds(0.05f); //velocidad del diálogo
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
