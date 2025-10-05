using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimentoMonito : MonoBehaviour
{
    public bool naCamer;
    public bool emAnim;
    public Transform player;
    private PlayerMove playScrip;
    public Transform playerCam;
    private MouseLook mouseScrip;

    private Animator anim;
    void Start()
    {
        playScrip = player.GetComponent<PlayerMove>();
        mouseScrip = playerCam.GetComponent<MouseLook>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetBool("naCamer", naCamer);
        
        if (anim.GetCurrentAnimatorStateInfo(0).length < anim.GetCurrentAnimatorStateInfo(0).normalizedTime)
        {
            emAnim = false;
        }else{
            emAnim = true;
        }
    }
    void entrarNaCam()
    {
        mouseScrip.cutscene = true;
        playScrip.naCam = true;
    }

    void saiuDaCam()
    {
        playScrip.naCam = false;
        mouseScrip.cutscene = false;
    }
}
