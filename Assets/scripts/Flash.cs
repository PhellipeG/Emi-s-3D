using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour {

	public bool taLigada = false;
	public Transform luz;
	private Light luzLight;

	public Transform player;
	private PlayerMove playScrip;

	public Transform ela;
	public bixoAndante elaIA;

	public Transform cam;
	public Enteragir interact;
	public MouseLook camLook;
	
	public Transform lanternaPos;

	public float energia = 1.00f;
	public float gastoDeEnergia = 0.01f;
	public Transform particulas;
	private ParticleSystem particulaSys;

    public float bounceFactor = 20;
    public float wobbleFactor = 10;
 
    public float maxTranslation = 0.05f;
    public float maxRotationDegrees = 5;
 
    private Vector3 oldBoneWorldPosition;
    private Quaternion oldBoneWorldRotation;
    private Vector3 animatedBoneWorldPosition;
    private Quaternion animatedBoneWorldRotation;
    private Quaternion goalRotation;
    private Vector3 goalPosition;

	// Use this for initialization
	void Start () {

		luzLight = luz.GetComponent<Light> ();
		playScrip = player.GetComponent<PlayerMove>();
		elaIA = ela.GetComponent<bixoAndante>();
		interact = cam.GetComponent<Enteragir>();
		camLook = cam.GetComponent<MouseLook>();
		particulaSys = particulas.GetComponent<ParticleSystem>();
		
	}
	
	void Awake()
    {
        oldBoneWorldPosition = lanternaPos.transform.position;
        oldBoneWorldRotation = lanternaPos.transform.rotation;
    }
	
	// Update is called once per frame
	void Update () {

		float distancia = Vector3.Distance(transform.position, ela.transform.position);

		luzLight.intensity = energia;
		
		if(Input.GetKeyDown(KeyCode.F) && elaIA.estado != "te comendo" && !camLook.cutscene)
		{
			taLigada = !taLigada;
			playScrip.sons.clip = playScrip.audios[0];
			playScrip.sons.pitch = 1f;
			playScrip.sons.Play();
			if(distancia < 10f && elaIA.estado != "perseguindo")
			{
				elaIA.chamarEla(player.transform);
			}
		}
		
		if(taLigada && !playScrip.naCam)
		{
			luzLight.enabled = true;
		}else{
			luzLight.enabled = false;
		}

		if(energia > 1f)
		{
			energia = 1f;
		}

		if(energia > 0 && taLigada)
		{
			energia -= gastoDeEnergia * Time.deltaTime;
		}

		if(interact.acao == "recarregouLantern")
		{
			energia = 1f;
			//elaIA.chamarEla();
			particulaSys.Play();
			interact.acao = "";
		}
	}
	
	void LateUpdate()
    {
        //LanternUpdate();
    }
	void LanternUpdate()
    {
        //Mesh has just been animated
        animatedBoneWorldPosition = lanternaPos.transform.position;
        animatedBoneWorldRotation = lanternaPos.transform.rotation;
        goalPosition = Vector3.Slerp(oldBoneWorldPosition, transform.position, Time.deltaTime * bounceFactor);
        goalRotation = Quaternion.Slerp(oldBoneWorldRotation, transform.rotation, Time.deltaTime * wobbleFactor);
 
        transform.rotation = Quaternion.RotateTowards(animatedBoneWorldRotation, goalRotation, maxRotationDegrees);
        transform.position = Vector3.MoveTowards(animatedBoneWorldPosition, goalPosition, maxTranslation);
 
        oldBoneWorldPosition = transform.position;
        oldBoneWorldRotation = transform.rotation;
    }
}
