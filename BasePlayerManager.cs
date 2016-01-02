using UnityEngine;
using System.Collections;

public class BasePlayerManager : MonoBehaviour {

	public bool didInit;

	public BaseUserManager DataManager;

	public void Awake()
	{
		didInit = false;
		Init();
		Debug.Log ("Inicializando PlayerManager.cs ...");
	}

	public virtual void Init()
	{
		if(DataManager == null)
			DataManager = gameObject.GetComponent<BaseUserManager>();

		didInit = true;
		Debug.Log("PlayerManager.cs ... ready.");
	}

	public virtual void GameFinished()
	{
		DataManager.SetIsFinished(true);
	}

}
