using UnityEngine;
using System.Collections;

public class Path_Spawner : MonoBehaviour {

	public Waypoints_Controller waypointControl;

	public bool distanceBasedSpawnStart;
	public float distanceFromCameraToSpawnAt = 35f;

	public bool shouldAutoStartSpawningOnLoad;

	public float timeBetweenSpawns = 1;
	public int totalAmountToSpawn = 10;
	public bool shouldReversePath;

	public GameObject[] spawnObjectPrefabs;

	private int totalSpawnObjects;

	private Transform myTransform;
	private GameObject tempObj;

	private int spawnCounter = 0;
	private int currentObjectNum;
	private Transform cameraTransform;
	private bool spawning;

	public bool shouldSetSpeed;
	public float speedToset;

	public bool shouldSmoothing;
	public float smoothingToSet;

	public bool shouldSetRotateSpeed;
	public float rotateToSet;

	private bool didInit;

	void Start()
	{
		Init();
	}

	void Init()
	{
		//referencia a nuetro cache 
		myTransform = transform;

		//referencia a la camara 
		Camera mainCam = Camera.main;

		if(mainCam == null)
			return;

		cameraTransform = mainCam.transform;

		// le decimos al waypoint_controller si queremos invertir el camino o no 
		waypointControl.SetReverseMode(shouldReversePath);

		totalSpawnObjects = spawnObjectPrefabs.Length;

		if(shouldAutoStartSpawningOnLoad)
			StartWave(totalAmountToSpawn,timeBetweenSpawns);
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(0,0,1,0.5f);
		Gizmos.DrawCube(transform.position,new Vector3(200,0,distanceFromCameraToSpawnAt));
	}

	public void Update()
	{
		float aDist = Mathf.Abs(myTransform.position.z - cameraTransform.position.z );

		if(distanceBasedSpawnStart && !spawning && aDist < distanceFromCameraToSpawnAt)
		{
			StartWave(totalAmountToSpawn, timeBetweenSpawns);
			spawning = true;
		}
	}

	public void StartWave(int HowMany, float timeBetweenSpawns)
	{
		spawnCounter = 0;
		totalAmountToSpawn = HowMany;

		//reset
		currentObjectNum = 0;

		CancelInvoke("doSpawn");
		InvokeRepeating("doSpawn",timeBetweenSpawns,timeBetweenSpawns);
	}

	void doSpawn()
	{
		SpawnObject();
	}

	public void SpawnObject()
	{
		if(spawnCounter >= totalAmountToSpawn)
		{
			//decirle al script que la ola ha terminado
			CancelInvoke("doSpawn");
			this.enabled = false;
			return;
		}

		// crea un objeto
		tempObj = SpawnController.Instance.SpawnGO(spawnObjectPrefabs[currentObjectNum],myTransform.position,Quaternion.identity);

		//decirle al objeto que invierta su pathfinding, si es requerido
		tempObj.SendMessage("SetReversePath",shouldReversePath,SendMessageOptions.DontRequireReceiver);

		// decirle al objeto generado que use el waypoint controler
		tempObj.SendMessage("SetWayController",waypointControl,SendMessageOptions.DontRequireReceiver);

		// decirle al objeto que use velocidad 
		if(shouldSetSpeed)
			tempObj.SendMessage("SetSpeed",speedToset,SendMessageOptions.DontRequireReceiver);
			
		// decirle al objeto que use velocidad de suaviazado
		if(shouldSmoothing)
			tempObj.SendMessage("SetPathSmoothingRate",smoothingToSet,SendMessageOptions.DontRequireReceiver);

		// decirle al objeto que use velocidad de rotacion
		if(shouldSetRotateSpeed)
			tempObj.SendMessage("SetRotateSpeed",rotateToSet,SendMessageOptions.DontRequireReceiver);

		//incrementamos el contador de cuantos objetos hemos generado
		spawnCounter++;

		// incrementamos el contador de que objeto generar 
		currentObjectNum++;

		if(currentObjectNum > totalSpawnObjects -1)
			currentObjectNum = 0;
	}
}
