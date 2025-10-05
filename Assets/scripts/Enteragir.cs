using System;
using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class Enteragir : MonoBehaviour
{
    public Transform player;
    public GameObject mira;

    public String acao;

    void Start()
    {
        acao = "";
    }

    void Update()
    {
		RaycastHit hit;
        if (Physics.Raycast (transform.position, transform.forward, out hit, 3f)) 
		{
			//Debug.Log (hit.transform.name);
			if (hit.transform.tag == "Interativo") 
			{
                mira.SetActive(true);
                if(Input.GetKeyDown(KeyCode.E))
                {
                    if(hit.transform.name == "bateria")
                    {
                        acao = "recarregouLantern";
                        Debug.Log(acao);
                    }
                    if(hit.transform.name == "chave")
                    {
                        acao = "pegouChave";
                    }
                    if(hit.transform.name == "mesaEscritorio")
                    {
                        acao = "arrumouChave";
                    }
                    if(hit.transform.name == "portaDosFundos")
                    {
                        acao = "tentouPorta";
                    }
                    if(hit.transform.name == "monitorCam")
                    {
                        acao = "entrouCam";
                    }
                }

                if(Input.GetKeyDown(KeyCode.E))
                {
                    if(hit.transform.name == "preparadorDeChave")
                    {
                        acao = "preparouChave";
                        Debug.Log(acao);
                    }
                }

                if(!Input.GetKey(KeyCode.E) && acao == "preparouChave")
                {
                    acao = "";
                }

			} else {
            mira.SetActive(false);
            }
		} else {
            mira.SetActive(false);
        }
    }
}
