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

			//añadir el weapon controller a este gameobject para que sea el padre de las armas y las armas 
			// se muevan junto con el jugador 

			//NOTA: si se necesita que los proyectiles esten en un layer diferente del gameobject principal
			// configurar el layer del forceparent en el layer en el que se quiere los proyectiles

			TEMPgameObject.transform.parent = forceParent;
			TEMPgameObject.layer = forceParent.gameObject.layer;
			TEMPgameObject.transform.position = forceParent.position;
			TEMPgameObject.transform.rotation = forceParent.rotation;

			//almacena una referencia al gameobject en un arraylist
			weaponSlots.Add(TEMPgameObject);

			//guardamos una referencia del script unida al arma 
			TEMPWeapon = TEMPgameObject.GetComponent<BaseWeaponScript>();
			weaponScripts.Add (TEMPWeapon);

			//desactiva el arma
			TEMPgameObject.SetActive(false);
		}

		// definimos el arma default seleccionada
		SetWeaponSlot(0);
	}

	public void SetOwner(int aNum){
		// usado para identificar el objeto que dispara, si es requerido 
		ownerNum = aNum;
	}

	public virtual void SetWeaponSlot(int slotNum){
		//si el arma ya es la que esta actualmente seleccionada, se sale.
		if (slotNum == lastSelectedWeaponSlot)
			return;

		//desactivar la arma actual
		DisableCurrentWeapon();

		// poner la arma pasada como la actual
		selectedWeaponSlot = slotNum;

		// asegurarse que valores importantes estan siendo pasados 
		if (selectedWeaponSlot < 0)
			selectedWeaponSlot = weaponSlots.Count - 1;

		//almacenamos el slot seleccionada para prevenir que se dupliquen las configuraciones 
		lastSelectedWeaponSlot = selectedWeaponSlot;

		//habilitar nuevamente el arma seleccionada
		EnableCurrentWeapon();
	}

	public virtual void NextWeaponSlot(bool shouldLoop){
		// desabilitar el arma actual
		DisableCurrentWeapon();

		//siguiente slot
		selectedWeaponSlot++;

		//asegurarse que el slot no es mayor al numero total de armas en la lista 
		if (selectedWeaponSlot == weaponSlots.Count) {
			if (shouldLoop) {
				selectedWeaponSlot = 0;
			} else {
				selectedWeaponSlot = weaponScripts.Count - 1;
			}
		}

		//almacenamos el slot seleccionado
		lastSelectedWeaponSlot = selectedWeaponSlot;

		//habilitamos nuevamente el arma seleccionada
		EnableCurrentWeapon();
	}

	public virtual void PrevWeaponSlot(bool shouldLoop){

		//desactivar el arma actual
		DisableCurrentWeapon();

		// slot previo 
		selectedWeaponSlot--;

		if (selectedWeaponSlot < 0) {
			if (shouldLoop) {
				selectedWeaponSlot = weaponScripts.Count - 1;
			} else {
				selectedWeaponSlot = 0;
			}
		}

		//almacenamos el slot 
		lastSelectedWeaponSlot = selectedWeaponSlot;

		//habilitamos nuevamente el arma
		EnableCurrentWeapon();
	}

	public virtual void DisableCurrentWeapon(){
		if (weaponScripts == 0)
			return;

		// hacemos una referencia al scropt del arma actualmente seleccionada
		TEMPWeapon = (BaseWeaponScript)weaponScripts[selectedWeaponSlot];

		//ahora le decimos al script que se desactive 
		TEMPWeapon.Disable();

		// hacemos una referencia al gameobject del arma para desactivarlo tambien 
		TEMPgameObject = (GameObject)weaponSlots[selectedWeaponSlot];
		TEMPgameObject.SetActive(false);
	}

	public virtual void EnableCurrentWeapon(){
		if (weaponScripts.Count == 0)
			return;

		// tomamos una referencia al arma seleccionada
		TEMPWeapon = (BaseWeaponScript)weaponScripts[selectedWeaponSlot];

		// la activamos 
		TEMPWeapon.Enable();

		TEMPgameObject = (GameObject)weaponSlots [selectedWeaponSlot];
		TEMPgameObject.SetActive (true);
	}

	public virtual void Fire(){
		if (weaponScripts == null)
			return;

		if (weaponScripts.Count == 0)
			return;

		//encuentra el arma
		TEMPWeapon = (BaseWeaponScript)weaponScripts[selectedWeaponSlot];

		theDir = myTransform.forward;

		if (useForceVectorDirection)
			theDir = forceVector;

		//dispara el proyectil
		TEMPWeapon.Fire(theDir, ownerNum);
	}
}
