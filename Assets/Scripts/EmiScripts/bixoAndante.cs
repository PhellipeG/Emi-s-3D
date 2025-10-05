using UnityEngine;
using UnityEngine.AI;

public class bixoAndante : MonoBehaviour {

	public Transform player;
	public Transform playerCam;
	public Transform miraNoPlayer;
	public Transform olhos;
	public Transform lanterna;
	public float alcance = 50f;
	public float velocidadeAndar = 3f;
	public float velocidadeCorrer = 6f;
	public string estado = "procurando";
	public float cooldown;
	public int vezesPraParar = 10;
	public float tempoParada;
	public Transform cone;
	public LayerMask mask;
	public Transform emiMesh;
	public LayerMask luzesMask;
	public bool PlayerNaLuz;
	public Material[] materiais;
	public AudioClip[] sons;

	Renderer rend;

	private NavMeshAgent agent;
	private emiAtaque jumpas;
	private checkMia mia;
	private Flash flashlight;
	private Animator anim;
	private PlayerMove ScripPlayer;
	private AudioSource somSouce;

	void Start () {

		agent = GetComponent<NavMeshAgent> ();
		jumpas = GetComponent<emiAtaque>();
		mia = cone.GetComponent<checkMia>();
		flashlight = lanterna.GetComponent<Flash>();
		anim = GetComponent<Animator>();
		rend = emiMesh.GetComponent<Renderer>();
		somSouce = GetComponent<AudioSource>();

		ScripPlayer = player.GetComponent<PlayerMove>();

		vezesPraParar = Random.Range(3, 10);
	}
	
	void Update () {
		if(jumpas.ativado)
		{
			estado = "te comendo";
			agent.isStopped = true;
			agent.baseOffset = 1f;
		}

		if(flashlight.taLigada || PlayerNaLuz)
		{
			alcance = 50f;
		}else{
			alcance = 10f;
		}

        PlayerNaLuz = Physics.CheckSphere(player.transform.position, 3f, luzesMask);

		RaycastHit hit;

		olhos.LookAt (miraNoPlayer.transform.position);

		anim.SetFloat("velocidade", agent.velocity.magnitude);

		switch(estado)
		{
			case "procurando":
				agent.speed = velocidadeAndar;
				anim.SetBool("Perseguindo", false);
				if (agent.remainingDistance <= agent.stoppingDistance)
				{
					if(vezesPraParar > 0)
					{
						irPraUmLugar();
						vezesPraParar -= 1;
					}else{
						estado = "parada";
						tempoParada = Random.Range(600f, 2000f);
					}
				}
				break;

			case "parada":
				if(tempoParada <= 0)
				{
					vezesPraParar = Random.Range(2, 8);
					irPraUmLugar();
					estado = "procurando";
				}
				break;

			case "chamada":
				agent.speed = velocidadeCorrer;

				if(agent.remainingDistance <= agent.stoppingDistance)
				{
					estado = "parada";
					tempoParada = Random.Range(100f, 800f);
					anim.SetBool("foiChamada", false);
				}
				break;

			case "perseguindo":
				agent.speed = velocidadeCorrer;
				anim.SetBool("Perseguindo", true);

				if (Physics.Raycast (olhos.transform.position, olhos.transform.forward, out hit, alcance, mask)) 
				{
					//Debug.Log (hit.transform.name);
					if (hit.transform.tag == "Player")
					{
						cooldown = 20;
					}
				}

				if (cooldown > 0)
				{
					agent.SetDestination (player.transform.position);
				}else{
					if (agent.remainingDistance <= agent.stoppingDistance)
					{
						estado = "procurando";
						agent.SetDestination (player.transform.position);
					}
				}
				break;
		}

		if(estado != "perseguindo" && estado != "te comendo")
		{
			if(mia.naMira)
			{
				if (Physics.Raycast (olhos.transform.position, olhos.transform.forward, out hit, alcance, mask)) 
				{
					//Debug.Log (hit.transform.tag);
					//Debug.Log (hit.transform.name);
					if (hit.transform.tag == "Player" && !ScripPlayer.deitado) 
					{
						cooldown = 20;
						estado = "perseguindo";
						anim.SetBool("foiChamada", false);
					}
				}
			}
		}
	}

	void irPraUmLugar()
	{
		Vector3 randomPos = Random.insideUnitSphere * 20f;
		NavMeshHit navHit;
		NavMesh.SamplePosition (transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);
		agent.SetDestination (navHit.position);
	}

	public void chamarEla(Transform destino)
	{
		agent.SetDestination(destino.transform.position);
		anim.SetBool("foiChamada", true);
		estado = "chamada";
	}

	public void fazerPasso()
	{
		somSouce.clip = sons[0];
		somSouce.pitch = Random.Range(0.95f, 1.05f);
		somSouce.Play();
	}

	void FixedUpdate () {
		if(estado == "parada")
		{
			tempoParada -= 1;
		}
		
		if(estado == "perseguindo" && cooldown > 0)
		{
			cooldown -= 0.4f;
		}

		miraNoPlayer.transform.position.Set(playerCam.transform.position.x, playerCam.transform.position.y - 0.1f, playerCam.transform.position.z);
	}

	void mudarRosto(int rostoAtual)
	{
		rend.sharedMaterial = materiais[rostoAtual];
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, alcance);

		Gizmos.DrawLine (olhos.transform.position, olhos.transform.forward);
	}
}
