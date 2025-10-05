using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveNew : MonoBehaviour
{
    public float moveSpeed;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    
    public bool correndo;
    public bool agaichado;
    public bool deitado;

    public AudioClip[] audios;
    public AudioSource sons;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.freezeRotation = true;
    }

    void Update()
    {
        MeuInput();
    }

    void FixedUpdate()
    {
        moverPlayer();
    }

    void MeuInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    void moverPlayer()
    {
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }
}
