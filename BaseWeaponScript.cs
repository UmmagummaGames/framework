using UnityEngine;
using System.Collections;

public class BaseWeaponScript : MonoBehaviour {

	[System.NonSerialized]
	public bool canFire;

	public int ammo = 100;
	public int maxAmmo = 100;

	public bool isInfiniteAmmo;
	public GameObject projectileGO;
	public Collider parentCollider;

	private Vector3 fireVector;

	[System.NonSerialized]
	public Transform myTransform;

	private int myLayer;

	public Vector3 spawnPosOffset;
	public float forwardOffSet = 1.5f;
	public float reloadTime = 2.0f;
	public float projectileSpeed = 10f;
	public bool inheritVelocity;

	[System.NonSerialized]
	public Transform theProjectile;

	private GameObject theProjectileGO;
	private bool isLoaded;
	private ProjectileController theProjectileController;

	public virtual void Start(){
		Init ();
	}

	public virtual void Init(){
		// cache el transform
		myTransform = transform;

		// cache el layer (hare que los projectiles eviten este layer en las coliciones asi no se chocaran entre ellos mismos)
		myLayer = gameObject.layer;

		//carga el arma
		Reloaded();

	}

	public virtual void Enabled(){
		//para de dispara si esta deshabilitado
		if (canFire == true)
			return;

		//habilita el arma
		canFire = true;
	}

	public virtual void Disable(){
		// para de dispara si esta deshabilitado 
		if (canFire == false)
			return;

		// guarda el arma
		canFire = false;
	}

	public virtual void Reloaded(){
		// la variable "isLoaded" nos dice si el arma esta cargada y lista para disparar
		isLoaded = true;
	}

	public virtual void SetCollider(Collider aCollider){
		parentCollider = aCollider;
	}

	public virtual void Fire(Vector3 aDirection, int ownerID){
		// hay que estar seguros que se puede disparar
		if (!canFire)
			return;

		// si el arma no esta cargada no se dispara 
		if (!isLoaded)
			return;

		// si se nos acabo las balas tampoco se dispara 
		if (ammo <= 0 && !isInfiniteAmmo)
			return;

		// resta ammo
		ammo--;

		//genera el projectil actual
		FireProjectil(aDirection, ownerID);

		//racargamos anted de disparar otra vez
		isLoaded = false;

		CancelInvoke ("Reloaded");
		Invoke("Reloaded", reloadTime);
	}

	public virtual void FireProjectile(Vector3 fireDirection, int ownerID){
		// crea el primer projectil 
		theProjectile = MakeProjectile(ownerID);

		// direcciona el prejectil en direccion de fuego
		theProjectile.LookAt(theProjectile.position + fireDirection);

		//añade fuerza para mover el projectile
		theProjectile.rigidbody.velocity = fireDirection * projectileSpeed;
	}

	public virtual Transform MakeProjectile(int ownerID){

		//crea el proyectil 
		theProjectile = SpawnController.Instance.Spawn(projectileGO, myTransform.position + spawnPosOffset + (myTransform.forward * forwardOffSet), myTransform.rotation);
		theProjectileGO = theProjectile.gameObject;
		theProjectileGO.layer = myLayer;

		//tomamos una referencia al controlador del proyectile asi podemos pasarle info
		theProjectileController = theProjectileGO.GetComponent<ProjectileController>();

		//definimos el propietario
		theProjectileController.SetOwnerType(ownerID);

		Physics.IgnoreLayerCollision (myTransform.gameObject.layer,myLayer);


		if (parentCollider != null) {
			// desabilitar la colision entre nosotros y nuestros proyectiles
			Physics.IgnoreCollision(theProjectile.collider, parentCollider);
		}

		//retornamos el projectil en caso de que queramos hacer algo mas con el 
		return theProjectile;
	}
}
