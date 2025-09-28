using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairClimb : MonoBehaviour
{
	Rigidbody rigidBody;
	[SerializeField] GameObject stepRayUpper;
	[SerializeField] GameObject stepRayLower;
	[SerializeField] float stepHeight = 0.3f;
	[SerializeField] float stepSmooth = 2f;

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();

		stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.z);
	}

	private void FixedUpdate()
	{
		stepClimb();
	}

	void stepClimb()
	{
		// Lower ray
		RaycastHit hitLower;
		Vector3 lowerOrigin = stepRayLower.transform.position;
		Vector3 direction = transform.TransformDirection(Vector3.forward);

		Debug.DrawRay(lowerOrigin, direction * 0.3f, Color.red); // visualize lower ray

		if (Physics.Raycast(lowerOrigin, direction, out hitLower, 0.1f))
		{
			// Upper ray
			RaycastHit hitUpper;
			Vector3 upperOrigin = stepRayUpper.transform.position;
			Debug.DrawRay(upperOrigin, direction * 0.4f, Color.green); // visualize upper ray

			if (!Physics.Raycast(upperOrigin, direction, out hitUpper, 0.2f))
			{
				rigidBody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
			}
		}
	}
}