using UnityEngine;
using System.Collections;

public class LaserEffect : MonoBehaviour {
    LineRenderer laserLine;

	// Use this for initialization
	void Awake () {
        laserLine = GetComponent<LineRenderer>();
        laserLine.enabled = false;
	}
	
	public void Toggle(bool is_visible)
    {
        laserLine.enabled = is_visible;
    }
}
