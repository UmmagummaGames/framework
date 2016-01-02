using UnityEngine;
using System.Collections;

public class Mouse_Input : BaseInputController {

	private Vector2 prevMousePos;
	private Vector2 mouseDelta;


	private float speedX = 0.05f;
	private float speedY = 0.1f;

	public void Start()
	{
		prevMousePos = Input.mousePosition;
	}

	public override void CheckInput(){

		// optiene datos de entrada del eje vertical y horizontal
		// y las almacena internamente en vert y horz asi no 
		// tenemos que acceder a ellas cada vez que necesitemos datos de salida

		// calcular un porcentaje para usar por pixel 
		float scalerX = 100f/ Screen.width;
		float scalerY = 100f/ Screen.height;

		// calcular y usar los deltas 
		float mouseDeltaX = Input.mousePosition.x - prevMousePos;
		float mouseDeltaY = Input.mousePosition.y - prevMousePos;

		//scala basada en el tamaño de la pantalla 
		vert += (mouseDeltaY * speedY) * scalerY;
		horz += (mouseDeltaX * speedX) * scalerX;

		//almacaena la posicion del mouse para la siguiente vez
		prevMousePos = Input.mousePosition;

		// configurar algunos boolean para arriba, abajo, izq y der
		Up = (vert > 0);
		Down = (vert < 0);
		Left = (horz < 0);
		Right = (horz > 0);

		//consigue los botones de accion
		Fire1 = Input.GetButton("Fire1");
	}

	public void LateUpdate()
	{
		// revisamos las entradas en cada LateUpdate() para que esten listos en el siguente tick
		CheckInput();
	}
}
