using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkyboxScript : MonoBehaviour {
	
	float timeOfDay;
	string skyboxLoc = "Standard Assets/Skyboxes/";
	public float timeSpeed = 20f;
	public Material daySky;
	public Material nightSky;
	public Material dawnSky;
	public Material duskSky;
	private Material currSky;
	private Material nextSky;
	private Material lerpSky;

	public List<Material> skies;


	// Use this for initialization
	void Start () {
		timeOfDay = 45f;
		lerpSky = daySky;
		currSky = daySky;
		nextSky = nightSky;
	}

	void Update() {
		float timePassage = Time.deltaTime * timeSpeed;
		transform.Rotate (timePassage, 0, 0, Space.Self);

		if (skies.Count > 0) {
			float daySegments = 360f / skies.Count;
			timeOfDay += timePassage;
			timeOfDay %= 360f;

			for (int i = 0; i < skies.Count; i++)
			{
				if (timeOfDay > daySegments * i && timeOfDay < daySegments * (i+1))
				{
					currSky = skies[i];

					if (i+1 < skies.Count)
						nextSky = skies[i+1];
					else
						nextSky = skies[0];
				}
			}
			
			RenderSettings.skybox.SetTexture("_FrontTex", currSky.GetTexture("_FrontTex"));
			RenderSettings.skybox.SetTexture("_BackTex", currSky.GetTexture("_BackTex"));
			RenderSettings.skybox.SetTexture("_RightTex", currSky.GetTexture("_RightTex"));
			RenderSettings.skybox.SetTexture("_LeftTex", currSky.GetTexture("_LeftTex"));
			RenderSettings.skybox.SetTexture("_UpTex", currSky.GetTexture("_UpTex"));
			RenderSettings.skybox.SetTexture("_DownTex", currSky.GetTexture("_DownTex"));
			
			RenderSettings.skybox.SetTexture("_FrontTex2", nextSky.GetTexture("_FrontTex"));
			RenderSettings.skybox.SetTexture("_BackTex2", nextSky.GetTexture("_BackTex"));
			RenderSettings.skybox.SetTexture("_RightTex2", nextSky.GetTexture("_RightTex"));
			RenderSettings.skybox.SetTexture("_LeftTex2", nextSky.GetTexture("_LeftTex"));
			RenderSettings.skybox.SetTexture("_UpTex2", nextSky.GetTexture("_UpTex"));
			RenderSettings.skybox.SetTexture("_DownTex2", nextSky.GetTexture("_DownTex"));
			
			RenderSettings.skybox.SetFloat ("_Blend", (timeOfDay % daySegments) / daySegments);
		}
	}
	/*
	// Update is called once per frame
	void Update () {
		float timePassage = Time.deltaTime * timeSpeed;
		timeOfDay += timePassage;
		timeOfDay %= 360;
		float t = (timeOfDay % 90) / 90;
		t = timeOfDay / 360;
		transform.Rotate (timePassage, 0, 0, Space.Self);

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
		
		RenderSettings.skybox.SetTexture("_FrontTex", currSky.GetTexture("_FrontTex"));
		RenderSettings.skybox.SetTexture("_BackTex", currSky.GetTexture("_BackTex"));
		RenderSettings.skybox.SetTexture("_RightTex", currSky.GetTexture("_RightTex"));
		RenderSettings.skybox.SetTexture("_LeftTex", currSky.GetTexture("_LeftTex"));
		RenderSettings.skybox.SetTexture("_UpTex", currSky.GetTexture("_UpTex"));
		RenderSettings.skybox.SetTexture("_DownTex", currSky.GetTexture("_DownTex"));
		//(RenderSettings.skybox).Lerp(currSky, nextSky, t);
	}*/
}
