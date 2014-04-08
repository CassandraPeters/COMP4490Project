using UnityEngine;
using System.Collections;

public class ScaleObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		float scale = Random.Range (0.3f, 1.0f);

		transform.localScale *= scale; 
	}
}
