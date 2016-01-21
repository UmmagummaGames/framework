using UnityEngine;
using System.Collections;

public class BaseVehicle : ExtendedCustomMonoBehavior {

	public WheelCollider frontWheelLeft;
	public WheelCollider frontWheelRight;
	public WheelCollider rearWheelLeft;
	public WheelCollider rearWheelRight;

	public float steer = 0f;
	public float motor = 0f;
	public float brake = 0f;

	public float mySpeed;
	public bool isLocked;

	[System.NonSerialized]
	public Vector3 velo;

	[System.NonSerialized]
	public Vector3 flatVelo;

	public BasePlayerManager myPlayerController;

	[System.NonSerialized]
	public Keyboard_Input default_input;

	public AudioSource engineSoundSource;

	public virtual void Start()
	{
		Init ();
	}

	public virtual void Init()
	{
		// cache lo de siempre
		myBody = rigidbody;
		myGO = gameObject;
		myTransform = transform;

		//añade entrada default de teclado
		default_input = myGO.AddComponent<Keyboard_Input>();

		myPlayerController = myGO.GetComponent<BasePlayerManager> ();

		myPlayerController.Init ();

		myBody.centerOfMass = new Vector3 (0,-4f,0);

		if (engineSoundSource == null) {
			engineSoundSource = myGO.GetComponent<AudioSource> ();
		}

	}

	public void SetUserInput(bool setInput)
	{
		canControl = setInput;
	}

	public bool setLock(bool lockState)
	{
		isLocked = lockState;
	}

	public virtual void LateUpdate()
	{
		if(canControl){
			GetInput ();
		}

		UpdateEngineAudio ();
	}

	public virtual void FixedUpdate()
	{
		UpdatePhysics ();
	}

	public virtual void UpdateEngineAudio()
	{
		engineSoundSource.pitch = 0.5f + (Mathf.Abs (mySpeed) * 0.005f);
	}

	public virtual void UpdatePhysics()
	{
		CheckLock ();

		velo = myBody.angularVelocity;

		velo = transform.InverseTransformDirection (myBody.velocity);
		flatVelo.x = velo.x;
		flatVelo.y = 0;
		flatVelo.z = velo.z;

		mySpeed = velo.z;

		if(mySpeed < 2)
		{
			if (brake > 0) {
				rearWheelLeft.motorTorque = -brakeMax * brake;
				rearWheelRight.motorTorque = -brakeMax * brake;

				rearWheelLeft.brakeTorque = 0;
				rearWheelRight.brakeTorque = 0;

				frontWheelLeft.steerAngle = steerMax * steer;
				frontWheelRight.steerAngle = steerMax * steer;

				return;
			}
		}

		rearWheelLeft.motorTorque = accelMax * motor;
		rearWheelRight.motorTorque = accelMax * motor;

		rearWheelLeft.brakeTorque = brakeMax * brake;
		rearWheelRight.brakeTorque = brakeMax * brake;
	}

	public void CheckLock()
	{
		if (isLocked) {

			// el control esta bloqueado asi que deberiamos detenernos 
			steer = 0;
			brake = 0;
			motor = 0;

			// mantiene nuestro rigidbody en su lugar (pero permite el movimiento en el eje Y de esa manera el vehiculo caeria si no esta sobre el suelo)
			Vector3 tempVEC = myBody.velocity;
			tempVEC.x = 0;
			tempVEC.z = 0;
			myBody.velocity = tempVEC;
		}
	}

	public virtual void GetInput()
	{
		// calcula la cantidad de giro del volante
		steer = Mathf.Clamp(default_input.GetHorizontal(),-1,1);

		// que tanto acelera 
		motor = Mathf.Clamp(default_input.GetVertical(),0,1);

		// que tanto frena 
		brake = -1 * Mathf.Clamp(default_input.GetVertical(),-1,0);
	}
}

