using UnityEngine;
using System.Collections;

public class SunScript : MonoBehaviour {


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Time.deltaTime * 20, 0, 0, Space.Self);
		//transform.Rotate (Vector3.up * Time.deltaTime * 20);
		//transform.localEulerAngles = new Vector3 (0, Time.deltaTime, 0);
	}
}
