using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStateController : MonoBehaviour
{

    Animator animator;
    int isWalkingHash;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        float forwardPressed = Input.GetAxisRaw("Horizontal");
        float sidesPressed = Input.GetAxisRaw("Vertical");

        bool isPressed = forwardPressed != 0 || sidesPressed != 0;

        if (!isWalking && isPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }
        if(isWalking && !isPressed)
        {
            animator.SetBool(isWalkingHash, false);
        }
    }
}
