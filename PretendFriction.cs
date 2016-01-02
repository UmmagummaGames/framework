using UnityEngine;
using System.Collections;

public class PretendFriction : MonoBehaviour {

	private Rigidbody myBody;
	private Transform myTransform;
	private float myMass;
	private float slideSpeed;
	private Vector3 velo;
	private Vector3 flatVelo;
	private Vector3 myRight;
	private Vector3 TEMPvec3;

	public float theGrip = 100f;

	void Start()
	{
		// cache algunas referencias a nuestro rigidbody, masa y transform
		myBody = rigidbody;
		myMass = myBody.mass;
		myTransform = transform;
	}

	void FixedUpdate()
	{
		// agarramos los valores que necesitamos para calcular el agarre 
		myRight = myTransform.right;

		// calculamos la velocidad plana 
		velo = myBody.velocity;
		flatVelo.x = velo.x;
		flatVelo.y = 0;
		flatVelo.z = velo.z;

		// calcular que tanto se esta deslizando
		slideSpeed = Vector3.Dot(myRight,flatVelo);

		//construimos un nuevo vector para compensar el deslizado
		TEMPvec3 = myRight * (-slideSpeed*myMass*theGrip);

		// aplicar la fuerza correccional al rigidbody
		myBody.AddForce(TEMPvec3*Time.deltaTime);
	}
}
