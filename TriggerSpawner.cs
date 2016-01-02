using UnityEngine;
using System.Collections;

public class TriggerSpawner : MonoBehaviour {

	public GameObject ObjectToSpawnOnTrigger; // este seria el enemigo a generar cuando se active el trigger 
	public Vector3 offsetPosition; // la distancia en la que se generara el enemigo 
	public bool onlySpawnOnce; // esto define si el trigger sera solo una vez (preferiblemente si)
	public int layerToCauseTriggerHit = 13; // este podria ser configurado al numero de la capa de la camara 

	private Transform myTransform;


	void Start()
	{
		Vector3 tempPos = transform.position;
		tempPos.y = Camera.main.transform.position.y; // nos aseguramos que la altura del trigger este a la misma altura de la camara
		transform.position = tempPos;

		//cache transform
		myTransform = transform;
	}

	void OnTriggerEnter(Collider other)
	{
		//hay que asegurar que la capa del objeto que entra al trigger es la correcta para la activacion 
		if(other.gameObject.layer != layerToCauseTriggerHit)
			return;

		//instanciar el objeto al entrar al trigger
		Instantiate(ObjectToSpawnOnTrigger,myTransform.position + offsetPosition, Quaternion.identity);

		//si solo vamos a generar una sola vez, destruimos este gameObject despues que la generacion ocurre 
		if(onlySpawnOnce)
			Destroy(gameObject);
	}

}
