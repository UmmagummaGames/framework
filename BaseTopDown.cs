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

	public virtual void LateUpdate()
	{
		// revisamos entradas en LateUpdate porque unity lo recomienda
		if(canControl)
			GetInput();
	}

	public bool moveDirectionally;

	private Vector3 targetDirection;
	private float curSmooth;
	private float targetSpeed;
	private float curSpeed;
	private Vector3 forward;
	private Vector3 right;

	void UpdateSmoothMovementDirection()
	{
		if(moveDirectionally)
		{
			UpdateDirectionalMovement();
		}else{
			UpdateRotationMovement();
		}
	}

	void UpdateDirectionalMovement()
	{
		targetDirection = horz * Vector3.right;
		targetDirection += vert * Vector3.forward;

		// almacenamos la velocidad y direccion de forma separada
		// de esa manera si el personaje permanece quieto todavia tendremos
		// una direccion valida
		// moveDirection esta siempre normalizada, y solo la actualizamos 
		// si hay entrada de usuario
		if(targetDirection != Vector3.zero)
		{
			moveDirection = Vector3.RotateTowards(moveDirection,targetDirection,rotateSpeed*Mathf.Deg2Rad*Time.deltaTime,1000);
			moveDirection = moveDirection.normalized;
		}

		//suavizar la velocidad base de la actual direccion objetivo
		curSmooth = speedSmoothing * Time.deltaTime;

		//escojer la velocidad objetivo
		//* queremos soportar entrada analoga pero sin que camine mas rapido en diagonal
		targetSpeed = Mathf.Min(targetDirection.magnitude,1.0f);

		_characterState = CharacterState.Idle;

		// decide sobre el estado de la animacion y ajusta la velocidad de movimiento 
		if(Time.time - runAfterSeconds > walkTimeStart)
		{
			targetSpeed *= runSpeed;
			_characterState = CharacterState.Running;
		}else{
			targetSpeed *= walkSpeed;
			_characterState = CharacterState.Walking;
		}

		moveSpeed = Mathf.Lerp(moveSpeed,targetSpeed,curSpeed);

		// reiniciamos el tiempo de inicio de la caminada cuando se ponga mas lento 
		if(moveSpeed < walkSpeed * 0.3f)
			walkTimeStart = Time.time;

		// calcular el movimiento actual 
		Vector3 movement = moveDirection * moveSpeed;
		movement *= Time.deltaTime;

		//mover el controlador 
		CollisionFlags = controller.Move(movement);

		// configurar la rotacion a la direccion de movimiento 
		myTransform.rotation = Quaternion.LookRotation(moveDirection);

	}

	void UpdateRotationMovement()
	{
		myTransform.Rotate(0,horz*rotateSpeed*Time.deltaTime,0);
		curSpeed = moveSpeed * vert;
		controller.SimpleMove(myTransform.forward*curSpeed);

		// direccion objetivo (la maxima que queremos mover, usada para calcular la velocidad objetivo)
		targetDirection = vert * myTransform.forward;

		// suavizar la velocidad base en la actual direccion objetivo 
		float curSmooth = speedSmoothing * Time.deltaTime;

		// escoje la velocidad objetivo
		targetSpeed = Mathf.Min(targetDirection.magnitude,1.0f);

		_characterState = CharacterState.Idle;

		// decide sobre el estado de la animacion y ajusta la velocidad de movimiento 
		if (Time.time - runAfterSeconds > walkTimeStart) {
			targetSpeed *= runSpeed;
			_characterState = CharacterState.Running;
		
		} 
		else 
		{
			targetSpeed *= walkSpeed;
			_characterState = CharacterState.Walking;
		}

		moveSpeed = Mathf.Lerp (moveSpeed,targetSpeed,curSmooth);

		//Reiniciar el tiempo de inicio de la caminada cuando se pone lento 
		if (moveSpeed < walkSpeed * 0.3f)
			walkTimeStart = Time.time;
	}

	void Update ()
	{
		if (!canControl) {
			// mata todas las entradas si no es controlable 
			Input.ResetInputAxes();
		}

		UpdateSmoothMovementDirection ();

		//ANIMATION sector
		if(_animation){
			if (controller.velocity.sqrMagnitude < 0.1f) {
				_animation.CrossFade (idleAnimation.name);
			} else {
				if (_characterState == CharacterState.Running) {
					_animation [walkAnimation.name].speed = Mathf.Clamp (controller.velocity.magnitude, 0.0f, runMaxAnimationSpeed);
					_animation.CrossFade (walkAnimation.name);
				} else if (_characterState == CharacterState.Walking) {
					_animation [walkAnimation.name].speed = Mathf.Clamp (controller.velocity.magnitude, 0.0f, walkMaxAnimationSpeed);
					_animation.CrossFade (walkAnimation.name);
				}

			}
		}
	}

	public float GetSpeed()
	{
		return moveSpeed;
	}

	public Vector3 GetDirection()
	{
		return moveDirection;
	}

	public bool IsMoving()
	{
		return Mathf.Abs (vert) + Mathf.Abs (horz) > 0.5f;
	}

	public void Reset()
	{
		gameObject.tag = "Player";
	}

}
