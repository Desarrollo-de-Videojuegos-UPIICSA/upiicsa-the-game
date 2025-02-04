using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{   //Variables de movimiento
    public CharacterController player; //Movimiento con componente CharacterController

    private Rigidbody rigidbody; //Fisicas
    private float hMove, vMove;
    private Vector3 playerInput;

    private Vector3 playerDirection;
    public float speed;
    public float gravity = 10f;
    public float fallingVel;

    public Camera mainCam;
    private Vector3 camForward;
    private Vector3 camRight;


    void Start()
    {
        //rigidbody = GetComponent<Rigidbody>();
        player = GetComponent<CharacterController>(); //Componente characterController
    }

    private void Update()
    {
        hMove = Input.GetAxis("Horizontal");
        vMove = Input.GetAxis("Vertical");
        speed = 7.44f; //le puse esta velocidad  considerando que el muñeco está caminando
        playerInput = new Vector3(hMove, 0, vMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1); //Magnitud de movimiento limitada a 1 (movimiento diagonal)

        camDirection(); //Obtener direccion de la camara horizontal

        playerDirection = playerInput.x * camRight + playerInput.z * camForward;

        player.transform.LookAt(player.transform.position + playerDirection);

        applyGravity();

        player.Move(playerDirection * speed * Time.deltaTime); //Movimiento con componente characterController

        /* //Salto con rigidbody
        if (Input.GetButtonDown("Jump")) {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }*/
    }
    void FixedUpdate()
    {
        //rigidbody.AddForce(playerDirection * speed); //Movimiento con addforce

    }

    void camDirection()
    {
        camForward = mainCam.transform.forward;
        camRight = mainCam.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    void applyGravity()
    {
        if (player.isGrounded)
        {
            fallingVel = -gravity * Time.deltaTime;
            playerDirection.y = fallingVel;
        }
        else
        {
            fallingVel -= gravity * Time.deltaTime;
            playerDirection.y = fallingVel; 
        }
    }
}
