using UnityEngine;
using System.Collections;

public class ExtendedCustomMonoBehavior : MonoBehaviour {

	/* el objetivo de esta clase es añadir elementos que se utilizaran con frecuencia 
	 * en el juego, tales como:
	 * 
	 * 
	 * 1. una variable myTransform para almacenar una referencia a un tranform.
	 * 2. una varialble myGO para almacenar una referencia a un gameObject.
	 * 3. una variable MyBody para almacenar una referencia a un rigidBody.
	 * 4. una variable boleana didInit para determinar cuando o no el script a sido inicializado.
	 * 5. una variable int id para almacenar un id.
	 * 6. una variable Vector3 tempVEC para usar en acciones temporales de vector.
	 * 7. una variable Transform tempTR para almacenar referencias temporales a transforms.
	 * 8. una funcion SetID(int) asi otras clases pueden definir un id.
	*/

	public Transform myTransform;
	public GameObject myGO;
	public Rigidbody myBody;

	public bool didInit;
	public bool canControl;

	public int id;

	[System.NonSerialized]
	public Vector3 tempVEC;

	[System.NonSerialized]
	public Transform tempTR;

	public virtual void SetID(int anID)
	{
		id = anID;
	}
}
