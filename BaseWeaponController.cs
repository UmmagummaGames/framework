using UnityEngine;
using System.Collections;

public class BaseWeaponController : MonoBehaviour {

	public GameObject[] weapons;

	public int selectedWeaponSlot;
	public int lastSelectedWeaponSlot;

	public Vector3 offsetWeaponSpawnPosition;

	public Transform forceParent;

	private ArrayList weaponSlots;
	private ArrayList weaponScripts;
	private BaseWeaponScript TEMPWeapon;
	private Vector3 TEMPvector3;
	private Quaternion TEMProtation;
	private GameObject TEMPgameObject;

	private Transform myTransform;
	private int ownerNum;

	public bool useForceVectorDirection;
	public Vector3 forceVector;
	private Vector3 theDir;

	public void Start()
	{
		// default para la primera casilla de arma 
		selectedWeaponSlot = 0;
		lastSelectedWeaponSlot = -1;

		//inicializa la lista de armas 
		weaponSlots = new ArrayList();

		myTransform = transform;

		if (forceParent == null) {
			forceParent = myTransform;
		}

		// en lugar de vigilar la rotacion y la posicion del transform del juagador en cada iteracion del ciclo
		// primero le hacemos cache en variables temporales
		TEMPvector3 = forceParent.position;
		TEMProtation = forceParent.rotation;

		// instanciamos todas las armas y las ocultamos, asi podemos activarlas cuando las necesitemos 
		for (int i = 0; i < weapons.Length; i++) {
			//instanciamos el item de la lista de armas 
			TEMPgameObject = (GameObject) Instantiate (weapons[i], TEMPvector3 + offsetWeaponSpawnPosition, TEMProtation);
		}


	}
}
