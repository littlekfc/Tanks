using UnityEngine;
using System.Collections;

public class TankFPSControl : MonoBehaviour
{
	public GameObject reticle;
	Transform pivot;
	Transform target;
	TankAnimator tank;
	void Start ()
	{
		pivot = new GameObject ("Targeting Pivot").transform;
		pivot.position = transform.position + (transform.up * 5);
		pivot.parent = transform;
		target = new GameObject ("Target Reticle").transform;
		target.position = pivot.position + (Vector3.forward * 25);
		target.parent = pivot;
		tank = GetComponent<TankAnimator> ();
		tank.target = target;
		var r = Instantiate(reticle) as GameObject;
		r.transform.parent = target;
		r.transform.localPosition = Vector3.zero;
		r.transform.LookAt(pivot);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		var speed = Input.GetAxis ("Vertical");
		var turnSpeed = Time.deltaTime*Input.GetAxis ("Horizontal")*0.5f;
		transform.RotateAroundLocal (Vector3.up, turnSpeed);
		transform.Translate (Vector3.forward * speed * Time.deltaTime*5); 
		tank.forwardSpeed = speed;
		tank.turnSpeed = turnSpeed*100;
		pivot.RotateAroundLocal(Vector3.up, Time.deltaTime*Input.GetAxis("Mouse X"));
		target.Translate(Vector3.up*Time.deltaTime*Input.GetAxis("Mouse Y")*10);
	}
	
	
}
