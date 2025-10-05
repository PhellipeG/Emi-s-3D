using UnityEngine;
using UnityEngine.SceneManagement;


public class emiAtaque : MonoBehaviour
{
    public Transform cam;
    public Transform camPos;
    public Transform luz;
    public Transform musiquinha;
    public Transform maoCam;
    public LayerMask camMask;
    public Transform olhos;

    public Animator anim;

    public bool ativado = false;

    public float distancia;

    public LayerMask mask;

    public Transform player;
    public PlayerMove playScrip;
    public GameObject golden;

    public AudioClip sustoSom;
    public AudioSource[] ambient;

    private MouseLook mouseScript;
	private AudioSource jumpas;
    private Light luzLuz;
    private Camera maoCamera;
    private bixoAndante IA;

    void Start()
    {
        mouseScript = cam.GetComponent<MouseLook>();
        //anim = GetComponent<Animator>();
        jumpas = player.GetComponent<AudioSource>();
        luzLuz = luz.GetComponent<Light>();
        maoCamera = maoCam.GetComponent<Camera>();
        IA = GetComponent<bixoAndante>();
        playScrip = player.GetComponent<PlayerMove>();
    }

    void Update()
    {
        distancia = Vector3.Distance(transform.position, cam.transform.position);

        RaycastHit hit;

        if(!ativado)
        {
            if(distancia <= 2.5f && !playScrip.deitado)
            {
                if(Physics.Raycast(olhos.transform.position, olhos.transform.forward, out hit, 5f, mask))
                {
                    if(hit.transform.tag == "Player")
                    {
                        fazerSusto();
                    }
                }
            }
        }

        if(ativado)
        {
            cam.transform.position = camPos.transform.position;
            cam.transform.rotation = camPos.transform.rotation;
            maoCamera.cullingMask = camMask;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log(collider.gameObject.tag);
        if(collider.gameObject.tag == "Player" && !ativado)
        {
            //fazerSusto();
        }
    }

    void fazerSusto()
    {
        anim.SetBool("Assustando", true);
        jumpas.clip = sustoSom;
        jumpas.pitch = 1f;
        jumpas.bypassReverbZones = true;
        jumpas.Play();
        ativado = true;
        mouseScript.cutscene = true;
        luzLuz.enabled = true;
        golden.SetActive(false);
        //ambient[].Stop();
        foreach(AudioSource numer in ambient)
        {
            numer.Stop();
        }
    }

    void reiniciarCena()
    {
        SceneManager.LoadScene(Application.loadedLevel);
    }
}
