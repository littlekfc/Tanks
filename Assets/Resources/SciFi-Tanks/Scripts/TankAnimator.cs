using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TankAnimator : MonoBehaviour
{
	public Transform target;
	public Transform pivot;
	public Transform yaw;
	public Transform pitch;
	public Transform leftTrack;
	public Transform rightTrack;
	public float yawSpeed = 10;
	public float pitchSpeed = 10;
	public float smoothTime = 1;
	public float maxTurretSpeed = 30;
	public float forwardSpeed;
	public float turnSpeed;
	public Transform[] leftWheels;
	public Transform[] rightWheels;
	public float wheelSpeedFactor = 5;
	float yawVelocity;
	float pitchVelocity;
	Material leftTrackMaterial, rightTrackMaterial;
	
	[ContextMenu("Find Wheels")]
	void ResetWheels ()
	{
		pivot = (from Transform t in transform where  t.childCount == 1 select t).First ();
		yaw = pivot.GetChild (0);
		pitch = yaw.GetChild (0);
		leftWheels = (from Transform t in transform where t.gameObject.name.ToLower ().Contains ("wheel") && t.localPosition.x < 0 select t).ToArray ();
		rightWheels = (from Transform t in transform where t.gameObject.name.ToLower ().Contains ("wheel") && t.localPosition.x > 0 select t).ToArray ();
		leftTrack = transform.FindChild ("treads_L");
		rightTrack = transform.FindChild ("treads_R");
	}
	
	void Start ()
	{
		leftTrackMaterial = leftTrack.renderer.material;	
		rightTrackMaterial = rightTrack.renderer.material;	
	}
	
	void AnimateTracks (Material track, Transform[] wheels, float speed)
	{
			
		var offset = track.mainTextureOffset;
		offset.x += speed * Time.deltaTime;
		track.mainTextureOffset = offset;
		track.SetTextureOffset ("_BumpMap", offset);
		foreach (var w in wheels) {
			w.RotateAround (w.right, speed * Time.deltaTime * wheelSpeedFactor);	
		}
		
	}

	void Update ()
	{
		
		if (target != null) {
			var E = yaw.localEulerAngles;
			var targetYaw = E.y;
			{   
				var LP = transform.InverseTransformPoint (target.position);
				var A = Vector3.Angle (new Vector3 (LP.x, 0, LP.z), Vector3.forward);
				targetYaw = A * (LP.x < 0 ? -1 : 1);
				E.z = Mathf.SmoothDampAngle (E.z, targetYaw, ref yawVelocity, smoothTime, maxTurretSpeed);
				yaw.localEulerAngles = E;
			}
			
			if (Mathf.Abs (Mathf.DeltaAngle (E.z, targetYaw)) < 30) {
				var LP = yaw.InverseTransformPoint (target.position + (Vector3.down * pitch.localPosition.y));
				var A = Vector3.Angle (new Vector3 (0, LP.y, LP.z), Vector3.down);
				var PE = pitch.localEulerAngles;
				var targetPitch = -A * (LP.y < 0 ? 1 : -1);
				PE.x = Mathf.SmoothDampAngle (PE.x, targetPitch, ref pitchVelocity, smoothTime, maxTurretSpeed);
				pitch.localEulerAngles = PE;
			}
		}
		if (Mathf.Approximately (turnSpeed, 0f)) {
			AnimateTracks (leftTrackMaterial, leftWheels, forwardSpeed);
			AnimateTracks (rightTrackMaterial, rightWheels, forwardSpeed);			
		} else {
			AnimateTracks (leftTrackMaterial, leftWheels, turnSpeed);
			AnimateTracks (rightTrackMaterial, rightWheels, -turnSpeed);
		}
	}

}
