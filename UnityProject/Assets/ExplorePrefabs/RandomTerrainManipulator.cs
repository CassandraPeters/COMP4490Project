using UnityEngine;
using System.Collections;
using System.Linq;
using AssemblyCSharp;

public class RandomTerrainManipulator : MonoBehaviour {
	private System.Random r;
	private Terrain terr;
	private TerrainData td;

	public bool forceIsland = true;
	public int minProtrusions = 5;
	public int maxProtrusions = 5;

	public int isleMinRadius = 250;
	public int isleMaxRadius = 300;
	public int minRadius = 200;
	public int maxRadius = 200;
	public float minHeight = 1f;
	public float maxHeight = 3f;
	public int minDips = 2;
	public int maxDips = 5;
	public int minDipRadius = 200;
	public int maxDipRadius = 200;
	public float minDipHeight = 1f;
	public float maxDipHeight = 3f;
	public float mountainHeight = 0.5f;
	private int islandChoice;
	public int minObjects = 100;
	public int maxObjects = 300;

	void randomizeObjectsDesert() {
		int numObjects = Random.Range (minObjects/10, maxObjects/10);
		
		Terrain terrain = GetComponent<Terrain>();
		
		// Get a reference to the terrain data
		TerrainData terrainData = terrain.terrainData;
		
		// Splatmap data is stored internally as a 3d array of floats, so declare a new empty array ready for your custom splatmap data:
		float[, ,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];
		Vector3 size = terrainData.size;
		Vector3 newPos = new Vector3 ();
		
		GameObject rock1 = GameObject.Find ("ToonRock1");
		GameObject rock2 = GameObject.Find ("ToonRock2");
		
		for (int i = 0; i < numObjects; i++) {
			bool keepGoing = true;
			
			newPos = terrain.transform.position;
			float w = Random.Range(0.0f, size.x);
			float h = Random.Range (0.0f, size.z);
			newPos.x += w;
			newPos.y += size.y + 10.0f;
			newPos.z += h;
			
			float y_01 = newPos.z/(float)terrainData.alphamapHeight;
			float x_01 = newPos.x/(float)terrainData.alphamapWidth;
			
			float height = terrainData.GetHeight (Mathf.RoundToInt (y_01 * terrainData.heightmapHeight), Mathf.RoundToInt (x_01 * terrainData.heightmapWidth));
			
			RaycastHit hit;
			float yoffset = 0;
			if(Physics.Raycast(newPos, -Vector3.up, out hit)){
				if (hit.collider.tag == "Rock"){
					i--;
					keepGoing = false;
				} else {
					yoffset = hit.distance;
				}
			}
			if (keepGoing){
				int objectChoice = r.Next () % 2;
				float scale = Random.Range (0.5f, 1.5f);

				newPos.y -= (yoffset-0.5f);
				if (objectChoice == 0){
					rock1.GetComponent<Transform>().localScale *= scale;
					Instantiate(rock1, newPos, rock1.transform.rotation);
				} else {
					rock2.GetComponent<Transform>().localScale *= scale;
					Instantiate(rock2, newPos, rock2.transform.rotation);
				}
			}
		}
	}
	void randomizeObjectsIsland(){
		int numObjects = Random.Range (minObjects, maxObjects);

		Terrain terrain = GetComponent<Terrain>();
		
		// Get a reference to the terrain data
		TerrainData terrainData = terrain.terrainData;
		
		// Splatmap data is stored internally as a 3d array of floats, so declare a new empty array ready for your custom splatmap data:
		float[, ,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];
		Vector3 size = terrainData.size;
		Vector3 newPos = new Vector3 ();

		GameObject tree = GameObject.Find ("ToonTree");
		GameObject bird = GameObject.Find ("birdrandom");

		for (int i = 0; i < numObjects; i++) {
			bool keepGoing = true;

			newPos = terrain.transform.position;
			float w = Random.Range(0.0f, size.x);
			float h = Random.Range (0.0f, size.z);
			newPos.x += w;
			newPos.y += size.y + 10.0f;
			newPos.z += h;

			float y_01 = newPos.z/(float)terrainData.alphamapHeight;
			float x_01 = newPos.x/(float)terrainData.alphamapWidth;

			float height = terrainData.GetHeight (Mathf.RoundToInt (y_01 * terrainData.heightmapHeight), Mathf.RoundToInt (x_01 * terrainData.heightmapWidth));

			RaycastHit hit;
			float yoffset = 0;
			if(Physics.Raycast(newPos, -Vector3.up, out hit)){
				if (hit.collider.tag == "Water" || hit.collider.tag == "Bird"){
					i--;
					keepGoing = false;
				} else {
					yoffset = hit.distance;
				}
			}
			if (keepGoing){
				int objectChoice = r.Next () % 2;
				if (objectChoice == 0){
					newPos.y -= (yoffset);
					Instantiate(tree, newPos, tree.transform.rotation);
				} else {
					newPos.y -= (yoffset-0.4f);
					Instantiate(bird, newPos, bird.transform.rotation);
				}
			}
		}
	}

	void terrainIsland()
	{
		float maxHeightf = 0;
		// Get the attached terrain component
		Terrain terrain = GetComponent<Terrain>();
		
		// Get a reference to the terrain data
		TerrainData terrainData = terrain.terrainData;
		
		// Splatmap data is stored internally as a 3d array of floats, so declare a new empty array ready for your custom splatmap data:
		float[, ,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];
		
		for (int y = 0; y < terrainData.alphamapHeight; y++) 
		{
			for (int x = 0; x < terrainData.alphamapWidth; x++) 
			{
				// Normalise x/y coordinates to range 0-1 
				float y_01 = (float)y/(float)terrainData.alphamapHeight;
				float x_01 = (float)x/(float)terrainData.alphamapWidth;
				
				float height = terrainData.GetHeight (Mathf.RoundToInt (y_01 * terrainData.heightmapHeight), Mathf.RoundToInt (x_01 * terrainData.heightmapWidth));
				if (height > maxHeightf) maxHeightf = height;
			}
		}
		for (int y = 0; y < terrainData.alphamapHeight; y++)
		{
			for (int x = 0; x < terrainData.alphamapWidth; x++)
			{
				// Normalise x/y coordinates to range 0-1 
				float y_01 = (float)y/(float)terrainData.alphamapHeight;
				float x_01 = (float)x/(float)terrainData.alphamapWidth;
				
				// Sample the height at this location (note GetHeight expects int coordinates corresponding to locations in the heightmap array)
				float height = terrainData.GetHeight(Mathf.RoundToInt(y_01 * terrainData.heightmapHeight),Mathf.RoundToInt(x_01 * terrainData.heightmapWidth) );
				
				// Calculate the normal of the terrain (note this is in normalised coordinates relative to the overall terrain dimensions)
				Vector3 normal = terrainData.GetInterpolatedNormal(y_01,x_01);
				
				// Calculate the steepness of the terrain
				float steepness = terrainData.GetSteepness(y_01,x_01);
				
				// Setup an array to record the mix of texture weights at this point
				float[] splatWeights = new float[terrainData.alphamapLayers];
				
				// CHANGE THE RULES BELOW TO SET THE WEIGHTS OF EACH TEXTURE ON WHATEVER RULES YOU WANT
				
				// Texture[0] has constant influence
				splatWeights[0] = 0.5f;
				
				// Texture[1] is stronger at lower altitudes
				splatWeights[1] = Mathf.Clamp01((terrainData.heightmapHeight - height));
				
				// Texture[2] stronger on flatter terrain
				splatWeights[2] = 1.0f - Mathf.Clamp01(steepness*steepness/(terrainData.heightmapHeight/5.0f));
				
				// Texture[3] increases with height 
				if (height > maxHeightf*mountainHeight){
					splatWeights[3] = height;
				} else {
					splatWeights[0] = 0;
				}
				
				// Sum of all textures weights must add to 1, so calculate normalization factor from sum of weights
				float z = splatWeights.Sum();
				
				// Loop through each terrain texture
				for(int i = 0; i<terrainData.alphamapLayers; i++){
					
					// Normalize so that sum of all texture weights = 1
					splatWeights[i] /= z;
					
					// Assign this point to the splatmap array
					splatmapData[x, y, i] = splatWeights[i];
				}
			}
		}
		
		// Finally assign the new splatmap to the terrainData:
		terrainData.SetAlphamaps(0, 0, splatmapData);
		randomizeObjectsIsland ();
	}

	void desertIsland()
	{
		float maxHeightf = 0;
		// Get the attached terrain component
		Terrain terrain = GetComponent<Terrain>();
		
		// Get a reference to the terrain data
		TerrainData terrainData = terrain.terrainData;
		
		// Splatmap data is stored internally as a 3d array of floats, so declare a new empty array ready for your custom splatmap data:
		float[, ,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];
		
		for (int y = 0; y < terrainData.alphamapHeight; y++) 
		{
			for (int x = 0; x < terrainData.alphamapWidth; x++) 
			{
				// Normalise x/y coordinates to range 0-1 
				float y_01 = (float)y/(float)terrainData.alphamapHeight;
				float x_01 = (float)x/(float)terrainData.alphamapWidth;
				
				float height = terrainData.GetHeight (Mathf.RoundToInt (y_01 * terrainData.heightmapHeight), Mathf.RoundToInt (x_01 * terrainData.heightmapWidth));
				if (height > maxHeight) maxHeight = height;
			}
		}
		for (int y = 0; y < terrainData.alphamapHeight; y++)
		{
			for (int x = 0; x < terrainData.alphamapWidth; x++)
			{
				// Normalise x/y coordinates to range 0-1 
				float y_01 = (float)y/(float)terrainData.alphamapHeight;
				float x_01 = (float)x/(float)terrainData.alphamapWidth;
				
				// Sample the height at this location (note GetHeight expects int coordinates corresponding to locations in the heightmap array)
				float height = terrainData.GetHeight(Mathf.RoundToInt(y_01 * terrainData.heightmapHeight),Mathf.RoundToInt(x_01 * terrainData.heightmapWidth) );
				
				// Calculate the normal of the terrain (note this is in normalised coordinates relative to the overall terrain dimensions)
				Vector3 normal = terrainData.GetInterpolatedNormal(y_01,x_01);
				
				// Calculate the steepness of the terrain
				float steepness = terrainData.GetSteepness(y_01,x_01);
				
				// Setup an array to record the mix of texture weights at this point
				float[] splatWeights = new float[terrainData.alphamapLayers];
				
				// CHANGE THE RULES BELOW TO SET THE WEIGHTS OF EACH TEXTURE ON WHATEVER RULES YOU WANT
				
				// Make everything sandy
				splatWeights[0] = 1.0f;

				// The other textures have no weight
				splatWeights[1] = 0;
				splatWeights[2] = 0;
				splatWeights[3] = 0;

				// Sum of all textures weights must add to 1, so calculate normalization factor from sum of weights
				float z = splatWeights.Sum();
				
				// Loop through each terrain texture
				for(int i = 0; i<terrainData.alphamapLayers; i++){
					
					// Normalize so that sum of all texture weights = 1
					splatWeights[i] /= z;
					
					// Assign this point to the splatmap array
					splatmapData[x, y, i] = splatWeights[i];
				}
			}
		}
		
		// Finally assign the new splatmap to the terrainData:
		terrainData.SetAlphamaps(0, 0, splatmapData);
		randomizeObjectsDesert ();
	}
	void assignSplatMaps()
	{
		if (forceIsland == true) {
			terrainIsland ();
		} else {
			if (islandChoice == 0) {
				terrainIsland ();
			} else {
				desertIsland ();
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		r = new System.Random ();
		islandChoice = r.Next () % 2;
		terr = Terrain.activeTerrain;
		td = terr.terrainData;
		float[,] heights;
		int w = td.heightmapWidth;
		int h = td.heightmapHeight;
		heights = td.GetHeights (0, 0, w, h);

		float startx = r.Next (99);
		float starty = r.Next (99);

		for (int i = 0; i < w; i++) {
			for (int j = 0; j < h; j++) {
				heights[i,j] = Mathf.PerlinNoise((float)(i + startx) / 80f, (float)(j + starty) / 80f)/3f ;
			}
		}

		switch (islandChoice)
		{
		case 0:
			//modify the island's edges so they go beneath the water
			Vector2 center, dist;
			float height = minHeight;
			float mag;
			int isleRadius = r.Next(isleMinRadius, isleMaxRadius);
			center = new Vector2(w/2,h/2);
			for (int i = 0; i < w; i++)
			{
				for (int j = 0; j < h; j++)
				{
					dist = new Vector2(i,j);
					mag = (dist - center).magnitude;
					
					if (mag > isleRadius)
						height = 0f;
					else{
						//Set the outer edge of the circle to be 0 and the center to be 1
						height = -(mag / ((float)isleRadius) - 1f);
						
						//Set up height for 
						height *= Mathf.PI;
						height -= Mathf.PI / 2;
						height = (Mathf.Sin(height) + 1) / 2;
					}
					heights[i,j] *= height;
					
				}
			}
			break;
		case 1:
			//reduce the random height of protrusions and dips for deserts
			maxHeight = minHeight + ((maxHeight - minHeight) / 2);
			minProtrusions /= 2;
			maxProtrusions /= 2;
			minDipRadius /= 2;
			maxDipRadius /= 2;
			maxDipHeight = minDipHeight + ((maxDipHeight - minDipHeight) / 2);
			break;
		}

		//Create upward protrusions for mountains
		TerrainProtrusions.createProtrusions((int)transform.position.x,(int)transform.position.y,w+(int)transform.position.x,h+(int)transform.position.y,minRadius,maxRadius,minHeight,maxHeight,minProtrusions,maxProtrusions,true);

		//Create the downward protrusions for rivers and lakes
		TerrainProtrusions.createProtrusions((int)transform.position.x,(int)transform.position.y,w+(int)transform.position.x,h+(int)transform.position.y,minDipRadius,maxDipRadius,minDipHeight,maxDipHeight,minDips,maxDips,false);

		//apply the protrusions to the height map
		TerrainProtrusions.modifyHeightMap (heights, (int)transform.position.x,(int)transform.position.y);

		td.SetHeights (0, 0, heights);

		assignSplatMaps ();
	}

	private float getHeight(float min, float max)
	{
		return (float)(min + (r.NextDouble() * (max-min)));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
