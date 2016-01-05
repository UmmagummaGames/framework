using UnityEngine;
using System.Collections;

public class BaseTopDownSpaceShip : ExtendedCustomMonoBehavior {

	private Quaternion targetRotation;

	private float thePos;
	private float moveXAmount;
	private float moveZAmount;

	public float moveXSpeed = 40f;
	public float moveZSpeed = 15f;

	public float limitX = 15f;
	public float limitZ = 15f;

	private float originZ;

	[System.NonSerialized]
	public Keyboard_Input default_input;

	public float horizontal_input;
	public float vertical_input;

	public virtual void Start()
	{
		didInit = false;
		this.Init();
	}

	public virtual void Init()
	{
		// referencias cache a nuestros transform y gameobject
		myTransform = transform;
		myGO = gameObject;
		myBody = rigidbody;

		default_input = myGO.AddComponent<Keyboard_Input>();

		originZ = myTransform.localPosition.z;

		didInit = true;
	}

	public virtual void GameStart()
	{
		canControl = true;
	}

	public void GetInput()
	{
		horizontal_input = default_input.GetHorizontal();
		vertical_input = default_input.GetVertical();
	}

	public virtual void Update()
	{
		UpdateShip();
	}

	public virtual void UpdateShip()
	{
		if(!didInit)
			return;
		if(!canControl)
			return;

		GetInput();

		//calcular las cantidades de movimiento para X y Z
		moveXAmount = horizontal_input * Time.deltaTime * moveXSpeed;
		moveZAmount = vertical_input * Time.deltaTime * moveZSpeed;

		Vector3 tempRotation = myTransform.eulerAngles;
		tempRotation.z = horizontal_input * -30f;
		myTransform.eulerAngles = tempRotation;

		// mover nuestro transform a nuestra posicion actualizada 
		myTransform.localPosition += new Vector3(moveXAmount,0,moveZAmount);

		// revisar la posicion para asegurar que esta dentro de los limites
		if(myTransform.localPosition.x <= -limitX || myTransform.localPosition.x >= limitX)
		{
			thePos = Mathf.Clamp(myTransform.localPosition.x,-limitX,limitX);
			myTransform.localPosition = new Vector3(thePos,myTransform.localPosition.y,myTransform.localPosition.z);
		}

		//tambien revisamos la posicion z
		if(myTransform.localPosition.z <= originZ || myTransform.localPosition.z >= limitZ)
		{
			thePos = Mathf.Clamp(myTransform.localPosition.z,originZ,limitZ);
			myTransform.localPosition = new Vector3(myTransform.localPosition.x,myTransform.localPosition.y,thePos);

		}

	}
}
