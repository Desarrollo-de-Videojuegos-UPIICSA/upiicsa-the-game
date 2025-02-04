using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DireccionarFlecha : MonoBehaviour
{
    public Transform jugador;
    public Transform objetivo;

     public float fixedYRotation = 0f;

    // Update is called once per frame
    void Update()
    {
        Vector3 direccion = (objetivo.position - transform.position).normalized;

        transform.up = direccion;
        
        
    }


    void LateUpdate() {
        
        Vector3 jugadorPos = jugador.position;
        Vector3 posicionNueva = new Vector3(jugadorPos.x, jugadorPos.y + 2.5f, jugadorPos.z);
        transform.position = posicionNueva;
    }
}
