using UnityEngine;
using System.Collections;

public class Waypoints_Controller : MonoBehaviour {

	[ExecuteInEditMode]

	// este script un camino visual para hacer mas facil la edicion de los waypoints
	private ArrayList transforms; //arraylist para facil acceso a los transforms 

	private Vector3 firstPoint; // almacena el primer waypoint para poder hacer un ciclo

	private float distance; // usado para calcular la ditancia entre los puntos 

	private Transform TEMPtrans; 
	private int TEMPindex;
	private int totalTransforms;

	private Vector3 diff;
	private float curDistance;
	private Transform closest;

	private Vector3 currentPos;
	private Vector3 lastPos;
	private Transform pointT;

	public bool closed = true;
	public bool shouldReverse;

	void Start(){
		//asegurarse que cuando este script se inicie tengamos todos los transform para cada punto 
		GetTransfoms();
	}

	void OnDrawGizmos(){
		// solo se quiere dibujar los waypoints cuando los estamos editando
		if (Application.isPlaying)
			return;

		GetTransforms ();
		//asegurarse de tener mas de un waypoint en la lista de otra manera no se puede dibujar las lineas 
		if (totalTransforms < 2)
			return;

		// dibujamos nuestro camino primero, tomamos la posicion de nuestro primer waypoint 
		// asi la linea tendra un punto de inicio
		TEMPtrans = (Transform)transforms[0];
		lastPos = TEMPtrans.position;

		//apuntamos cada waypoint al siguiente asi sabemos cuando el jugador esta llendo en la direccion incorrecta
		// primero necesitamos una referencia al tranform al que vamos a apuntar

		pointT = (Transform)transforms [0];

		firstPoint = lastPos;

		//dibujamos las lineas entre los puntos 
		for (int i = 1; i < transforms.Count; i++) {
			TEMPtrans = (Transform)transforms [i];
			if (TEMPtrans == null) {
				GetTransforms ();
				return;
			}
			// tomamos la posicion actual del waypoint
			currentPos = TEMPtrans.position;

			Gizmos.color = Color.green;
			Gizmos.DrawSphere (currentPos,2);

			//dibuja la linea
			Gizmos.color = Color.red;
			Gizmos.DrawLine (lastPos, currentPos);

			//apuntamos el ultimo transform a la ultima posicion
			pointT.LookAt(currentPos);

			lastPos = currentPos;

			pointT = (Transform)transforms[i];
		}
		//cerrar el camino
		if(closed){
			Gizmos.color = Color.red;
			Gizmos.DrawLine (currentPos, firstPoint);
		}

	}

	public void SetReverseMode (bool rev){
		shouldReverse = rev;
	}

	public void GetTransforms(){
		// almacenamos todos los transforms de waypoints en un arraylist,
		// el cual es inicializado aca (siempre se hace esto antes de usar el arraylist)
		transforms = new ArrayList();

		foreach(Transform t in transform){
			//añade cada tranform al array
			transforms.Add(t);
		}
		totalTransforms = (int)transforms.Count;
	}

	public int FindNearestWaypoint(Vector3 fromPos, float maxRange)
	{
		// asegurarse de que hemos poblado la lista de transforms 
		// y si no, poblarlas.
		if (transforms == null)
			GetTransforms ();

		//la variable de distancia es solo usada para guardar la distancia actual 
		//cuando estamos comparando, asi podemos encontar la distancia mas corta = Mathf.Infinity;

		//Iteramos atravez de ellos y encontramos la mas corta 
		for (int i = 0; i < transforms.Count; i++) {
			//agarra una referencia al transform 
			TEMPtrans = (Transform)transforms[i];

			// calcula la distancia entre el transform actual y el pasado en la posicion del vector
			diff = (TEMPtrans.position - fromPos);
			curDistance = diff.sqrMagnitude;

			//ahora comparamos las distancias asegurandonos que no estamos tan cerca como el 
			//objeto mas cercano (cuya distancia esta guardada en la variable distance)
			if (curDistance < distance) {
				if (Mathf.Abs (TEMPtrans.position.y - fromPos.y) < maxRange) {
					closest = TEMPtrans;
					TEMPindex = i;
					distance = curDistance;
				}
			}

		}

		if (closest) {
			return TEMPindex;
		} else {
			return -1;
		}

	}


}
