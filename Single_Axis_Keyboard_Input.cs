using UnityEngine;
using System.Collections;

public class Single_Axis_Keyboard_Input : BaseInputController {

	public override void CheckInput()
	{
		// conseguir datos de los ejes horizontales y verticales
		// y almacenarlos internamente en vert y horz asi no tenemos que accederlos cada ves 
		horz = Input.GetAxis("Horizontal");

		// configurar algunos booleans 
		Left = (horz < 0);
		Right = (horz > 0);

		// consigue botones de accion
		Fire1 = Input.GetButton("Fire1");
	}

	public void LateUpdate()
	{
		CheckInput();
	}
}
