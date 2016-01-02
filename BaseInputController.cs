using UnityEngine;
using System.Collections;

public class BaseInputController : MonoBehaviour {

	// botones direccionales 
	public bool Up;
	public bool Down;
	public bool Left;
	public bool Right;

	//fire / boton de accion
	public bool Fire1;

	//espacios para armas 
	public bool Slot1;
	public bool Slot2;
	public bool Slot3;
	public bool Slot4;
	public bool Slot5;
	public bool Slot6;
	public bool Slot7;
	public bool Slot8;
	public bool Slot9;

	public float vert;
	public float horz;
	public bool shouldRespawn;

	public Vector3 TEMPVec3;
	private Vector3 zeroVector = new Vector3(0,0,0);

	public virtual void CheckInput()
	{
		horz = Input.GetAxis("Horizontal");
		vert = Input.GetAxis ("Vertical");
	}

	public virtual float GetHorizontal()
	{
		return horz;
	}

	public virtual float GetVertical()
	{
		return vert;
	}

	public virtual bool GetFire()
	{
		return Fire1;
	}

	public bool GetRespawn()
	{
		return shouldRespawn;
	}

	public virtual Vector3 GetMovementDirectionVector()
	{
		if(Left||Right)
		{
			TEMPVec3.x = horz;
		}
		if(Down||Up)
		{
			TEMPVec3.y = vert;
		}

		return TEMPVec3;
	}
}
