using UnityEngine;
using System.Collections;

public class TankSpawn : MonoBehaviour {
	public float spawnRate = 5;
	public Transform player;
	public Transform destroyPoint;
	public float tankSpeed = 10;
	public GameObject[] tankPrefabs;
	
	IEnumerator Start () {
		while(true) {
			foreach(var i in tankPrefabs) {
				var g = Instantiate(i, transform.position, transform.rotation) as GameObject;
				var t = g.GetComponent<TankAnimator>();
				t.target = player;
				StartCoroutine(Move(t, destroyPoint));
				yield return new WaitForSeconds(spawnRate);
			}
		}
	}
	
	
	IEnumerator Move(TankAnimator tank, Transform target) {
		tank.forwardSpeed = 1;
		var direction = (target.position-tank.transform.position).normalized;
		tank.transform.rotation = Quaternion.LookRotation(direction);
		while((target.position-tank.transform.position).sqrMagnitude > 1) {
			tank.transform.Translate(Vector3.forward*Time.deltaTime*tankSpeed);	
			yield return null;
		}
		Destroy(tank.gameObject);
	}
	
}
