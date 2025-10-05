using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    CharacterController controller;

    public float VelocidadeAndar = 5f;
    public float VelocidadeCorrer = 10f;
    public float Gravidade = 9.81f;

    public Transform cameraa;
    public Camera cameraCamera;
    private Animator camAnimator;
    public float cameraFOV = 60;
    public GameObject maoCamera;

    public Transform chaoCheck;
    public float chaoDistancia = 0.4f;
    public LayerMask chaoMask;

    public Transform tetoCheck;
    public float tetoDistancia = 0.4f;
    public LayerMask tetoMask;

    public LayerMask luzesMask;

    public bool correndo;
    public bool agaichado;
    public bool deitado;
    public bool naCam;

    public Transform camDeitadaPos;

    public Transform ela;
    private bixoAndante elaIA;

    public Transform golden;
    private golden goldIA;

    public AudioClip[] audios;
    public AudioSource sons;

    Vector3 quedaVel;
    public bool noChao;
    public bool noTeto;
    public bool naLuz;
    public float velocidadeAtual;
    
    private Vector3 move;
    private MouseLook mouseScript;

    void Start()
    {
        cameraCamera = cameraa.GetComponent<Camera>();
        mouseScript = cameraa.GetComponent<MouseLook>();
        camAnimator = cameraa.GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        elaIA = ela.GetComponent<bixoAndante>();
        goldIA = golden.GetComponent<golden>();
        sons = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        noChao = Physics.CheckSphere(chaoCheck.position, chaoDistancia, chaoMask);
        noTeto = Physics.CheckSphere(tetoCheck.position, tetoDistancia, tetoMask);

        if (noChao && quedaVel.y < 0)
        {
            quedaVel.y = 5;
        }

        float X = Input.GetAxisRaw("Horizontal");
        float Z = Input.GetAxisRaw("Vertical");

        move = transform.right * X + transform.forward * Z;

        cameraCamera.fieldOfView = cameraFOV;

        if(mouseScript.cutscene == false)
        {
            controller.Move(move.normalized * velocidadeAtual * Time.deltaTime);

            correndo = Input.GetKey(KeyCode.LeftShift);

            if (Input.GetKeyDown(KeyCode.LeftControl) && !noTeto)
            {
                agaichado = !agaichado;
                deitado = false;
            }

            if (Input.GetKeyDown(KeyCode.C) && !noTeto)
            {
                deitado = !deitado;
                agaichado = false;
            }

            quedaVel.y += (Gravidade * -1) + Time.deltaTime;

            controller.Move(quedaVel * Time.deltaTime);

            if(deitado)
            {
                cameraa.transform.position = camDeitadaPos.transform.position;
                controller.radius = 0.1f;
            }else{
                cameraa.transform.position = tetoCheck.transform.position;
                controller.radius = 0.4f;
            }

            if(Input.GetKeyDown(KeyCode.R))
            {
                sons.clip = audios[1];
                sons.pitch = Random.Range(0.8f, 1.2f);
                sons.Play();
                if(elaIA.estado != "perseguindo")
                {
                    elaIA.chamarEla(transform);
                }
            }
        }

        if(agaichado)
        {
            controller.height = 0.5f;
            VelocidadeAndar = 2.5f;
        }
        if(deitado)
        {
            controller.height = 0.1f;
            VelocidadeAndar = 3f;
        }
        if (!deitado && !agaichado)
        {
            controller.height = 1.8f;
            VelocidadeAndar = 6f;
        }

        if(mouseScript.cutscene)
        {
            correndo = false;
            agaichado = false;
            deitado = false;
        }
        camAnimator.SetBool("cutscene", mouseScript.cutscene);

        if(elaIA.estado == "te comendo" || naCam || goldIA.sustoTocando)
        {
            camAnimator.enabled = false;
        }else{
            camAnimator.enabled = true;
        }

        if(naCam)
        {
            maoCamera.SetActive(false);
        }else{
            maoCamera.SetActive(true);
        }

        if(elaIA.estado == "te comendo" && naCam)
        {
            naCam = false;
        }
    }

    void FixedUpdate(){

        if(cameraFOV < 60)
        {
            cameraFOV = 60;
        }
        if (cameraFOV > 80)
        {
            cameraFOV = 80;
        }

        if (correndo && !agaichado && !deitado)
        {
            velocidadeAtual = VelocidadeCorrer;
            if(cameraFOV < 80)
            {
                cameraFOV += 2;
            }
        }else{
            velocidadeAtual = VelocidadeAndar;
            if (cameraFOV > 60)
            {
                cameraFOV -= 2;
            }
        }
    }
}
