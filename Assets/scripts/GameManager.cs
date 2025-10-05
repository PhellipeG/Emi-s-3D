using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public Transform player;
    public Transform playerCam;
    public Transform lanternaObj;

    public Transform Ela;
    public Transform Golden;

    public GameObject chave;
    public GameObject preparadorChave;
    public GameObject mesa;
    public AudioSource somNaMesa;
    public Transform monitorPos;

    public GameObject canvas;
    public Text objetivoTexto;
    public GameObject porta;
    public Text portaTrancadaTexto;
    public GameObject barraDeProgresso;

    public Transform audioCamera;
    public GameObject botaoCamera;

    public String objetivoAtual;

    private int etapaAtual = 0;
    private Camera playerCamCamera;
    private PlayerMove playerScrip;
    private Animator camAnimator;
    private movimentoMonito monitorPosMove;
    private MouseLook cameraLook;
    private Flash lanterna;
    private Enteragir interact;
    private bixoAndante elaIA;
    private golden goldIA;
    private float textoTrancadaAlpha = 0f;
    private float progressoChave = 0f;
    private float tempoPraChave = 100f;
    private Slider progressoSlider;
    private AudioSource audioCamSource;

    void Start()
    {
        playerScrip = player.GetComponent<PlayerMove>();
        interact = playerCam.GetComponent<Enteragir>();
        cameraLook = playerCam.GetComponent<MouseLook>();
        camAnimator = playerCam.GetComponent<Animator>();
        lanterna = lanternaObj.GetComponent<Flash>();
        elaIA = Ela.GetComponent<bixoAndante>();
        goldIA = Golden.GetComponent<golden>();
        progressoSlider = barraDeProgresso.GetComponent<Slider>();
        progressoSlider.maxValue = tempoPraChave;
        monitorPosMove = monitorPos.GetComponent<movimentoMonito>();
        audioCamSource = audioCamera.GetComponent<AudioSource>();

        objetivoAtual = "Encontre a chave da porta dos fundos";
    }

    void Update()
    {
        if(interact.acao == "pegouChave")
        {
            interact.acao = "";
            if(etapaAtual == 0)
            {
                etapaAtual = 1;
                objetivoAtual = "Prepare a chave no lado do palco";
                Destroy(chave);
                preparadorChave.tag = "Interativo";
            }
        }

        if(progressoChave > tempoPraChave)
        {
            if(etapaAtual == 1)
            {
                interact.acao = "";
                etapaAtual = 2;
                objetivoAtual = "Vá pro escritório e arrume a chave";
                preparadorChave.tag = "Untagged";
                mesa.tag = "Interativo";
            }
        }

        if(interact.acao == "arrumouChave")
        {
            interact.acao = "";
            if(etapaAtual == 2)
            {
                etapaAtual = 3;
                somNaMesa.Play();
                //elaIA.chamarEla();
                objetivoAtual = "Ache e destranque a porta dos fundos";
                mesa.tag = "Untagged";
            }
        }

        if(interact.acao == "tentouPorta")
        {
            interact.acao = "";
            if(etapaAtual < 3)
            {
                textoTrancadaAlpha = 2f;
            }
            if(etapaAtual == 3)
            {
                etapaAtual = 3;
                Destroy(porta);
                objetivoAtual = "";
            }
        }

        if(elaIA.estado == "te comendo" || goldIA.sustoTocando)
        {
            Destroy(canvas);
            lanterna.taLigada = true;
            lanterna.energia = 1f;
            cameraLook.cutscene = true;
        }

        objetivoTexto.text = "Objetivo: " + objetivoAtual;

        portaTrancadaTexto.color = new Color(portaTrancadaTexto.color.r, portaTrancadaTexto.color.g, portaTrancadaTexto.color.b, textoTrancadaAlpha);

        if(textoTrancadaAlpha > 0)
        {
            textoTrancadaAlpha -= 0.01f;
        }

        if(elaIA.estado != "te comendo" && interact.acao != "preparouChave" && !playerScrip.naCam && !goldIA.sustoTocando)
        {
            cameraLook.cutscene = false;
        }

        if(interact.acao == "preparouChave")
        {
            barraDeProgresso.SetActive(true);
        }else{
            barraDeProgresso.SetActive(false);
        }
        
        progressoSlider.value = progressoChave;

        if(interact.acao == "entrouCam")
        {
            interact.acao = "";
            if(!monitorPosMove.emAnim)
            {
                monitorPosMove.naCamer = !monitorPosMove.naCamer;
            }
        }

        if(playerScrip.naCam)
        {
            cameraLook.cutscene = true;
            playerCam.transform.SetPositionAndRotation(monitorPos.position, monitorPos.rotation);
        }

        if(playerScrip.naCam && !monitorPosMove.emAnim)
        {
            botaoCamera.SetActive(true);
        }else{
            botaoCamera.SetActive(false);
        }
    }

    public void audioCam()
    {
        Debug.Log("chamouCam");
        audioCamSource.Play();
        elaIA.chamarEla(audioCamera.transform);
    }

    void FixedUpdate()
    {
        if(interact.acao == "preparouChave" && etapaAtual == 1)
        {
            progressoChave += 0.1f;
            cameraLook.cutscene = true;
        }
    }
}
