using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class golden : MonoBehaviour
{
    public Transform pos;
    Vector3 posDirection;
    public float posDistance;
    Vector2 posDistanceMinMax = new Vector2(0.5f, 4.4f);
    public float distancia;

    public GameObject goldenMesh;

    public Transform player;
    public Transform playerCam;
    public Transform ataqueMira;
    public Transform playerOlha;
    public Transform camSustoPos;

    public bool visivel = false;
    public int animAtual = 0;
    public float ataque = 0f;
    public bool olhouPraEla;
	public LayerMask mask;
    public Transform olhos;
    public float raiva = 0f;
    public bool atacando = false;
    public float velocidadeAtaque = 10f;
    public GameObject emiNormal;
    public float prenderOlhar = 0f;
    public bool sustoTocando = false;
    public AudioClip jumpasSom;
    public Transform maoCam;
    private Camera maoCamCam;
    public LayerMask camMask;

    private NavMeshAgent agent;
    private Animator anim;
    private AudioSource somSusto;
    public AudioSource[] ambient;

    private Camera playerCamCam;
    private MouseLook mouseScript;
    
    Plane[] cameraFrustum;

    Collider collide;
    
    void Start()
    {
        setarAtaque();

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        somSusto = GetComponent<AudioSource>();
        playerCamCam = playerCam.GetComponent<Camera>();
        collide = GetComponent<Collider>();
        mouseScript = playerCam.GetComponent<MouseLook>();
        maoCamCam = maoCam.GetComponent<Camera>();

        posDirection = pos.localPosition.normalized;
        posDistance = posDistanceMinMax.y;
    }

    void Update()
    {
        anim.SetBool("taVisivel", visivel);
        anim.SetInteger("poseAtual", animAtual);

        reposicionarPos(pos);

        if(!atacando)
        {
            RaycastHit hit;

            cameraFrustum = GeometryUtility.CalculateFrustumPlanes(playerCamCam);
            var bounds = collide.bounds;
            if(GeometryUtility.TestPlanesAABB(cameraFrustum, bounds))
            {
                if(visivel)
                {
                    raiva += 1f * Time.deltaTime;
                    if(!olhouPraEla)
                    {
                        prenderOlhar = 4.5f;
                        olhouPraEla = true;
                        somSusto.Play();
                    }
                }
            }else{
                transform.LookAt(player.position);
                if(olhouPraEla == true)
                {
                    setarAtaque();
                }
            }

            if(prenderOlhar > 0)
            {
                player.LookAt(new Vector3(playerOlha.transform.position.x, player.position.y, playerOlha.transform.position.z));
                playerCam.LookAt(new Vector3(playerOlha.transform.position.x, playerOlha.transform.position.y, playerOlha.transform.position.z));
                prenderOlhar -= 0.2f;
            }

            if(ataque < 0)
            {
                visivel = true;
            }

            goldenMesh.SetActive(visivel);
            olhos.LookAt(playerCam.transform.position);

                if(!visivel)
                {
                    distancia = Vector3.Distance(player.position, pos.position);
                    transform.LookAt(player.position);
                    agent.SetDestination(pos.transform.position);
                    animAtual = Random.Range(0, 11);
                    agent.isStopped = false;
                }else{
                    agent.isStopped = true;
                    distancia = Vector3.Distance(player.position, transform.position);
                    if(Physics.Raycast(olhos.transform.position, olhos.transform.forward, out hit, 999, mask))
                    {
                        Debug.Log (hit.transform.name);
                        if(hit.transform.tag != "Player")
                        {
                            setarAtaque();
                        }
                    }
                    if(distancia <= 2f)
                    {
                        if(!olhouPraEla)
                        {
                            setarAtaque();
                        }else{

                        }
                    }
                }
        }

        if(raiva >= 1.4f)
        {
            atacarPlayer();
        }

        if(atacando && !sustoTocando)
        {
            distancia = Vector3.Distance(transform.position, player.position);
            transform.LookAt(ataqueMira);
            transform.position += transform.forward * velocidadeAtaque * Time.deltaTime;
            if(distancia <= 1.5 && !sustoTocando)
            {
                fazerSusto();
            }
        }

        if(sustoTocando)
        {
            playerCam.transform.position = camSustoPos.transform.position;
            playerCam.transform.rotation = camSustoPos.transform.rotation;
            maoCamCam.cullingMask = camMask;
        }
    }

    void FixedUpdate()
    {
        if(distancia > 2f && !visivel)
        {
            ataque -= 1;
        }
    }

    void reposicionarPos(Transform pos)
    {
        Vector3 desiredPos = player.TransformPoint(posDirection * posDistanceMinMax.y);

        RaycastHit posHit;
        if(Physics.Linecast(player.position, desiredPos, out posHit, mask))
        {
            posDistance = Mathf.Clamp(posHit.distance, posDistanceMinMax.x, posDistanceMinMax.y);
        }else{
            posDistance = posDistanceMinMax.y;
        }

        pos.localPosition = posDirection * posDistance;
    }

    void setarAtaque()
    {
        if(!atacando)
        {
            visivel = false;
            olhouPraEla = false;
            raiva = 0;
            ataque = Random.Range(50, 500);
        }
    }

    void atacarPlayer()
    {
        atacando = true;
        agent.enabled = false;
        anim.SetBool("atacando", true);
    }

    void fazerSusto()
    {
        sustoTocando = true;
        anim.SetBool("susto", true);
        mouseScript.cutscene = true;
        somSusto.clip = jumpasSom;
        somSusto.Play();
        emiNormal.SetActive(false);
        foreach(AudioSource numer in ambient)
        {
            numer.Stop();
        }
    }

    void reiniciarCena()
    {
        sustoTocando = false;
        SceneManager.LoadScene(Application.loadedLevel);
    }
}
