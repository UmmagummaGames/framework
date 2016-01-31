using UnityEngine;
using System.Collections;

public class BaseWheelAligment : MonoBehaviour {
	// define variables usadas en el script, los correspondientes collider y demas


	public WheelCollider CorrespondingCollider;
	public GameObject  slipPrefab;
	public float slipAmoutForTireSmoke = 50f;

	private float RotationValue = 0.0f;
	private Transform myTransform;
	private Quaternion zeroRotation;
	private Transform colliderTransform;
	private float suspensionDistance;


	void Start()
	{
		// cache algunas cosas comunmente usadas
		myTransform = transform;
		zeroRotation = Quaternion.identity;
		colliderTransform = CorrespondingCollider;
	}

	void Update()
	{
		// define un punto de colision para el raycast
		RaycastHit hit;
		Vector3 ColliderCenterPoint = colliderTransform.TransformPoint (CorrespondingCollider.center);

		if (Physics.Raycast (ColliderCenterPoint, -colliderTransform.up, out hit, CorrespondingCollider.suspensionDistance + CorrespondingCollider.radius)) {
			myTransform.position = hit.point + (colliderTransform.up * CorrespondingCollider.radius);
		} else {
			myTransform.position = ColliderCenterPoint - (colliderTransform.up * CorrespondingCollider.suspensionDistance);
		}

		myTransform.rotation = colliderTransform.rotation * Quaternion.Euler (RotationValue, CorrespondingCollider.steerAngle,0);

		RotationValue += CorrespondingCollider.rpm * (360 / 60) * Time.deltaTime;

		WheelHit correspondingGroundHit = new WheelHit ();
		CorrespondingCollider.GetGroundHit (out correspondingGroundHit);

		if(Mathf.Abs(correspondingGroundHit.sidewaysSlip) > slipAmoutForTireSmoke){
			if(slipPrefab){
				SpawnController.Instance.Spawn (slipPrefab, correspondingGroundHit.point, zeroRotation);
			}
		}
	}
}
