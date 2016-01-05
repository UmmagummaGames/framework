using UnityEngine;
using System.Collections;

public class BaseTopDown : ExtendedCustomMonoBehavior {

	public AnimationClip idleAnimation;
	public Animation walkAnimation;

	public float walkMaxAnimationSpeed = 0.75f;
	public float runMaxAnimationSpeed = 1.0f;

	// cuando empezo el usuario a correr (usado para empezar a correr despues de un tiempo)
	private float walkTimeStart = 0.0f;

	// hacemos la siguiente variable publica asi podemos usar una animacion en un gameObject diferente 
	public Animation _animation;

	enum CharacterState {
		Idle = 0,
		Walking = 1,
		Running = 2,
	}

	private CharacterState _characterState;

	// la velocidad cuando se camina 
	public float walkSpeed = 2.0f;

	// despues de caminar runAfterSeconds corremos con velocidad runSpeed
	public float runSpeed = 4.0f;

	public float speedSmoothing = 10.0f;
	public float rotateSpeed = 500.0f;
	public float runAfterSeconds = 3.0f;

	//la actual direccion de movimiento en x-z
	private Vector3 moveDirection = Vector3.zero;

	// la velocidad vertical actual 
	private float verticalSpeed = 0.0f;

	//la actual velocidad de movimiento x-z
	public float moveSpeed = 0.0f;

	// la ultima bandera de colision regresada de controller.move
	private CollisionFlags;

	public BasePlayerManager myPlayerController;

	[System.NonSerialized]
	public Keyboard_Input defaul_Input;

	public float horz;
	public float vert;

	private CharacterController controller;

	//-----------------------------------------------------------------------------------------------

	void Awake()
	{
		// necesitamos hacer esto antes que nada pase en el script o el objeto
		// asi que lo ponemos en el Awake()
		// si es necesario añadir configuraciones especificas, considerar añadirlos en la funcion Init() en lugar de esta funcion 
		// limitar esta funcion para cosas que necesitamos que pasen antes que nada

		moveDirection = transform.TransformDirection(Vector3.forward);

		// si la animacion no ha sido configurada en el inspector 
		// trataremos de encontrarla en el gameObject actual 

		if(_animation == null)
			_animation = GetComponent<Animation>();

		if(!_animation)
			Debug.Log("el personaje que quiere controlar no tiene animaciones");

		if(!idleAnimation){
			_animation = null;
			Debug.Log("no se encontro animacion idle. Apagando animaciones");
		}

		if(!walkAnimation){
			_animation = null;
			Debug.Log("no se encontro animacion walk. Apagando animaciones");
		}

		controller = GetComponent<CharacterController>();
	}

	public virtual void Start()
	{
		Init();
	}

	public virtual void Init()
	{
		// cache lo usual
		myBody = rigidbody;
		myGO = gameObject;
		myTransform = transform;

		// añadir entrada de teclado default
		default_input = myGO.AddComponent<Keyboard_Input>();

		// cache una referencia al controlador del jugador 
		myPlayerController = myGO.GetComponent<BasePlayerManager>();

		if(myPlayerController != null)
			myPlayerController.Init();
	}

	public void SetUserInput(bool setInput)
	{
		canControl = setInput;
	}

	public virtual void GetInput()
	{
		horz = Mathf.Clamp(defaul_Input.GetHorizontal(),-1,1);
		vert = Mathf.Clamp(default_input.GetVertical(),-1,1);
	}
}
