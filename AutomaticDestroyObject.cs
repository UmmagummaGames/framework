using UnityEngine;
using System.Collections;

public class AutomaticDestroyObject : MonoBehaviour {

	public float timeBeforeObjectDestroys;

	void Start()
	{
		//la funcion destryGO() sera llamada en timeBeforeObjectDestroys segundos
		Invoke("DestroyGO",timeBeforeObjectDestroys);
	}

	void DestroyGO()
	{
		//destruye el gameObject
		Destroy(gameObject);
	}
}
