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
        controller.enabled = false;
        estadoJugador.enabled = false;
        Animator animator = estadoJugador.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isWalking", false);
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
            controller.enabled = true;
            estadoJugador.enabled = true;
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
