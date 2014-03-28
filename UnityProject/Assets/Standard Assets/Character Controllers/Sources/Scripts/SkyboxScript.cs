using UnityEngine;
using System.Collections;

public class SkyboxScript : MonoBehaviour {
	
	float timeOfDay;
	string skyboxLoc = "Standard Assets/Skyboxes/";
	public Material daySky;
	public Material nightSky;
	public Material dawnSky;
	public Material duskSky;
	private Material currSky;
	private Material nextSky;
	private Material lerpSky;


	// Use this for initialization
	void Start () {
		timeOfDay = 45f;
		lerpSky = daySky;
		currSky = daySky;
		nextSky = nightSky;
	}
	
	// Update is called once per frame
	void Update () {
		
		timeOfDay += Time.deltaTime * 20;
		timeOfDay %= 360;
		float t = (timeOfDay % 90) / 90;
		t = timeOfDay / 360;


		if (timeOfDay < 90f && currSky != dawnSky) {
			lerpSky = dawnSky;
			currSky = dawnSky;
			nextSky = daySky;
		} else if (timeOfDay > 90f && timeOfDay < 180f && currSky != daySky) {
			lerpSky = daySky;
			currSky = daySky;
			nextSky = duskSky;
		} else if (timeOfDay > 180f && timeOfDay < 270f && currSky != duskSky) {
			lerpSky = duskSky;
			currSky = duskSky;
			nextSky = nightSky;
		} else if (timeOfDay > 270f && timeOfDay < 360f && currSky != nightSky) {
			lerpSky = nightSky;
			currSky = nightSky;
			nextSky = dawnSky;
		}

		lerpSky.Lerp (currSky, nextSky, Time.deltaTime * 20 / 90);
		//Oh wait this only lerps through colours so fuck it

		RenderSettings.skybox = lerpSky;
		//(RenderSettings.skybox).Lerp(currSky, nextSky, t);
	}
}
