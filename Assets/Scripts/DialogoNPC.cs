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
    
    void Update()
    {
        if (jugadorEnZona && Input.GetKeyDown(KeyCode.E))
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
        Time.timeScale = 0f; //Esto desactiva absolutamente todo el movimiento de la escena
                             /*modificar para activar la animación de idle*/
        StartCoroutine(ShowLine());
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
            Time.timeScale = 1f;
        }
    }
    private IEnumerator ShowLine()
    {
        textoDialogo.text = string.Empty;
        foreach (char ch in lineasDialogo[lineasIndex])
        {
            textoDialogo.text += ch;
            yield return new WaitForSecondsRealtime(0.05f); //velocidad del diálogo
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
