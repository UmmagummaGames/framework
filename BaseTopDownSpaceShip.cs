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
	public float limtZ = 15f;

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
}
