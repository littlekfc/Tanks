using UnityEngine;
using System.Collections;

public class SimpleRotate : MonoBehaviour
{
	public float speed = 1;
	public Vector3 axis = Vector3.up;
	
	void Update ()
	{
		transform.RotateAround (axis, speed * Time.deltaTime);
	}
}
