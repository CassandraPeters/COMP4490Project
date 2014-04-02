using UnityEngine;
using System.Collections;

public class LavaGlow : MonoBehaviour {
	public float minIntensity = 50;
	public float maxIntensity = 100;
	public float speed = 100f;
	private float direction = 1f;
	private float currIntensity;

	// Use this for initialization
	void Start () {
		currIntensity = minIntensity;
	}
	
	// Update is called once per frame
	void Update () {
		light.range = currIntensity;
		currIntensity += direction * speed * Time.deltaTime;

		if (currIntensity > maxIntensity)
				direction = -1f;
		else if (currIntensity < minIntensity)
				direction = 1f;
	}
}
