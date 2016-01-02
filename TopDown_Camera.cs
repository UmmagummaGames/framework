using UnityEngine;
using System.Collections;

public class TopDown_Camera : MonoBehaviour {


	public Transform followTarget;
	public Vector3 targetOffSet;
	public float moveSpeed = 2f;

	private Transform myTransform;

	void Start()
	{
		myTransform = transform;
	}

	public void SetTarget(Transform aTransform)
	{
		followTarget = aTransform;
	}

	void LateUpdate ()
	{
		if(followTarget != null)
			myTransform.position = Vector3.Lerp(myTransform.position,followTarget.position + targetOffSet, moveSpeed * Time.deltaTime);
	}
}
