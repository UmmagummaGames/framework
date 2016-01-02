using UnityEngine;
using System.Collections;

public class SpawnController : ScriptableObject {


	private ArrayList playerTransforms;
	private ArrayList playerGameObjects;

	private Transform tempTrans;
	private GameObject tempGO;

	private GameObject[] playerPrefabList;
	private Vector3[] startPositions;
	private Quaternion[] startRotations;



	// singleton

	private static SpawnController instance;
	public SpawnController() 
	{
		if(instance != null){
			Debug.LogWarning("trato de generar mas de una instancia en estructura singleton");
			return;
		}
		instance = this;
	}
	public static SpawnController Instance
	{
		//para los otros scripts este getter setter es la manera de tener acceso 
		// a la instancia singleton de este script.
		get
		{
			if(instance == null)
				ScriptableObject.CreateInstance<SpawnController>();
			return instance;
		}
	}

	// fin singleton



	public void Restart()
	{
		playerTransforms = new ArrayList();
		playerGameObjects = new ArrayList();
	}


	public void SetUpPlayers(GameObject[] playerPrefabs, Vector3[] playerStartPositions, Quaternion[] playerStartRotations, Transform theParentObj, int totalPlayers)
	{
		playerPrefabList = playerPrefabs;
		startPositions = playerStartPositions;
		startRotations = playerStartRotations;

		//llamar la funcion que se encarga de generar todos los jugadores y ponerlos en el lugar correcto
		CreatePlayers(theParentObj, totalPlayers);

	}

	public void CreatePlayers(Transform theParent, int totalPlayers)
	{
		playerTransforms = new ArrayList();
		playerGameObjects = new ArrayList();

		for(int i=0; i<totalPlayers;i++)
		{
			//genera un jugador
			tempTrans = Spawn(playerPrefabList[i],startPositions[i],startRotations[i]);
			if(theParent != null)
			{
				tempTrans.parent = theParent;
				tempTrans.localPosition = startPositions[i];
			}

			playerTransforms.Add(tempTrans);
			playerGameObjects.Add (tempTrans.gameObject);
		}
	}

	public GameObject GetPlayerGO(int indexNum)
	{
		return (GameObject)playerGameObjects[indexNum];
	}

	public Transform GetPlayerTransform (int indexNum)
	{
		return (Transform)playerTransforms[indexNum];
	}

	public Transform Spawn (GameObject anObject, Vector3 aPosition, Quaternion aRotation)
	{
		// instanciar el objeto
		tempGO = (GameObject)Instantiate(anObject, aPosition, aRotation);
		tempTrans = tempGO.transform;

		//retorna el objeto a quien lo haya llamado
		return tempTrans;
	}

	// aqui regresamos el gameObject en lugar de los transforms 
	public GameObject SpawnGO (GameObject anObject, Vector3 aPosition, Quaternion aRotation)
	{
		// instanciar el objeto
		tempGO = (GameObject)Instantiate(anObject, aPosition, aRotation);
		tempTrans = tempGO.transform;
		
		//retorna el objeto a quien lo haya llamado
		return tempGO;
	}

	public ArrayList GetAllSpawnedPlayers()
	{
		return playerTransforms;
	}
}
