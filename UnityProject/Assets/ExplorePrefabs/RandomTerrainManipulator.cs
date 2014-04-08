using UnityEngine;
using System.Collections;
using System.Linq;

public class RandomTerrainManipulator : MonoBehaviour {
	private System.Random r;
	private Terrain terr;
	private TerrainData td;

	public bool forceIsland = true;
	public bool isIsland = true;
	public int minHeightAvg = 1;
	public int maxHeightAvg = 10;
	public int minProtrusions = 5;
	public int maxProtrusions = 5;
	public bool bisect = false;
	public int minBisect = 4;
	public int maxBisect = 20;

	public int isleRadius = 300;
	public bool circle = true;
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
		float maxHeight = 0;
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

	bool checkBetween(int sx, int ex, int i, int sy, int ey, int j)
	{
		return (sx <= i && ex >= i && sy <= j && ey >= j);
	}

	void circleDips(float[,] heights)
	{
		int numProt = r.Next (minDips, maxDips);
		int radius;
		int x, y;
		Vector2 center, dist;
		float height = minHeight;
		float mag;
		for (int k = 0; k < numProt; k++) {
			x = r.Next(0, heights.GetLength(0));
			y = r.Next(0, heights.GetLength(1));
			center = new Vector2(x,y);
			radius = r.Next(minDipRadius, maxDipRadius);
			//radius = 200;
			for (int i = 0; i < heights.GetLength(0); i++)
			{
				for (int j = 0; j < heights.GetLength(1); j++)
				{
					dist = new Vector2(i,j);
					mag = (dist - center).magnitude;
					
					if (mag > radius)
						height = 1f;
					else{
						//Set the outer edge of the circle to be 0 and the center to be 1
						height = -(mag / ((float)radius) - 1f);
						
						//Set up height for 
						height *= Mathf.PI;
						height -= Mathf.PI / 2;
						height = (Mathf.Sin(height) + 1) / 2;
						
						height = (height * (maxDipHeight - minDipHeight)) + minDipHeight;
						//height = ((((float)radius) / mag) * (maxHeight - minHeight));// + minHeight;
					}
					heights[i,j] /= height;
					
				}
			}
		}
	}

	void circleProtrusion(float[,] heights)
	{
		int numProt = r.Next (minProtrusions, maxProtrusions);
		int radius;
		int x, y;
		Vector2 center, dist;
		float height = minHeight;
		float mag;
		for (int k = 0; k < numProt; k++) {
			x = r.Next(0, heights.GetLength(0));
			y = r.Next(0, heights.GetLength(1));
			center = new Vector2(x,y);
			radius = r.Next(minRadius, maxRadius);
			//radius = 200;
			for (int i = 0; i < heights.GetLength(0); i++)
			{
				for (int j = 0; j < heights.GetLength(1); j++)
				{
					dist = new Vector2(i,j);
					mag = (dist - center).magnitude;

					if (mag > radius)
						height = 1f;
					else{
						//Set the outer edge of the circle to be 0 and the center to be 1
						height = -(mag / ((float)radius) - 1f);

						//Set up height for 
						height *= Mathf.PI;
						height -= Mathf.PI / 2;
						height = (Mathf.Sin(height) + 1) / 2;

						height = (height * (maxHeight - minHeight)) + minHeight;
						//height = ((((float)radius) / mag) * (maxHeight - minHeight));// + minHeight;
					}
					heights[i,j] *= height;

				}
			}
		}
	}

	float avgHeight(float[,] heights, int x, int y)
	{
		float sum = 0f;

		if (x - 1 >= 0)
			sum += heights [x - 1, y];
		if (x + 1 < heights.GetLength (0))
			sum += heights [x + 1, y];
		if (y - 1 >= 0)
			sum += heights [x, y - 1];
		if (y + 1 < heights.GetLength (1))
			sum += heights [x, y + 1];
		if (x - 1 >= 0 && y - 1 >= 0)
			sum += heights[x-1,y-1];
		if (x + 1 < heights.GetLength(0) && y - 1 >= 0)
			sum += heights[x+1,y-1];
		if (x + 1 < heights.GetLength(0) && y + 1 < heights.GetLength(1))
			sum += heights[x+1,y+1];
		if (x - 1 >= 0 && y + 1 < heights.GetLength(1))
			sum += heights[x-1,y+1];

		return sum / 8;
	}

	float[,] avgHeightsInRange (float[,] heights, int sx, int sy, int ex, int ey)
	{

		for (int i = sx; i < ex; i++) {
			heights[i,sy] = avgHeight(heights,i,sy);
			heights[i,ey-1] = avgHeight(heights,i,ey-1);
		}
		for (int j = sy; j < ey; j++) {
			heights[sx,j] = avgHeight(heights,sx,j);
			heights[ex-1,j] = avgHeight(heights,ex-1,j);
		}
		int newSX = sx;
		int newSY = sy;
		int newEX = ex;
		int newEY = ey;
		if (ex - sx > 2)
		{
			newSX = sx+1;
			newEX = ex-1;
		}
		if (ey - sy > 2)
		{
			newSY = sy+1;
			newEY = ey-1;
		}

		if (Mathf.Abs(ex - sx) > 2 || Mathf.Abs(ey - sy) > 2)
			heights = avgHeightsInRange(heights,newSX,newSY,newEX,newEY);

		return heights;
	}

	float[,] bisectTerrain (float[,] heights, int sx, int sy, int ex, int ey, bool dir, int numBisect)
	{
		float mult = ((float)r.Next (70, 160)) / 100f;
		for (int i = sx; i < ex; i++) {
			for (int j = sy; j < ey; j++) {
				heights [i, j] *= mult;
			}
		}
		int numPasses = r.Next (minHeightAvg, maxHeightAvg);
		
		for (int i = 0; i < numPasses; i++) {
			avgHeightsInRange (heights, sx, sy, ex, ey);
		}

		if (numBisect > 0) {
			int newEX = 0;
			int newEY = 0;
			int newSX2 = 0;
			int newSY2 = 0;

			if (dir)
			{
				newEX = r.Next(sx,ex);
				newSX2 = newEX;
				newEY = ey;
				newSY2 = sy;
			}
			else
			{
				newEY = r.Next(sy,ey);
				newSY2 = newEY;
				newEX = ex;
				newSX2 = sx;
			}
			
			heights = bisectTerrain(heights, sx, sy, newEX, newEY, !dir, numBisect - 1);
			heights = bisectTerrain(heights, newSX2, newSY2, ex, ey, !dir, numBisect - 1);
		}

		return heights;
	}
	
	// Use this for initialization
	void Start () {
		r = new System.Random ();
		islandChoice = r.Next () % 2;
		terr = Terrain.activeTerrain;
		td = terr.terrainData;
		float[,] heights;
		float min, max, temp, avg;
		float left, up;
		int w = td.heightmapWidth;
		int h = td.heightmapHeight;
		float diff = 0.0007f;
		heights = td.GetHeights (0, 0, w, h);
		int tendency;

		int numProtrusions = r.Next (minProtrusions, maxProtrusions);

		float baseHeight = 0.3f;
		float startx = r.Next (99);
		float starty = r.Next (99);

		float mult = ((float)r.Next (80, 300))/100f;

		int sx, sy, ex, ey;
		int maxProtSize = 200;
		int minProtSize = 10;


		for (int i = 0; i < w; i++) {
			for (int j = 0; j < h; j++) {
				heights[i,j] = Mathf.PerlinNoise((float)(i + startx) / 80f, (float)(j + starty) / 80f)/3f ;
			}
		}

		if (islandChoice == 0) {
			int x, y;
			Vector2 center, dist;
			float height = minHeight;
			float mag;
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
						
						//height = (height * (1f - 0f)) + 0f;
						//height = ((((float)radius) / mag) * (maxHeight - minHeight));// + minHeight;
					}
					heights[i,j] *= height;
					
				}
			}
		}

		if (islandChoice == 1) {
			maxHeight = minHeight + ((maxHeight - minHeight) / 2);
			minProtrusions /= 2;
			maxProtrusions /= 2;
			minDipRadius /= 2;
			maxDipRadius /= 2;
			maxDipHeight = minDipHeight + ((maxDipHeight - minDipHeight) / 2);
		}

		if (bisect) {
			heights = bisectTerrain (heights, 0, 0, w, h, true, r.Next (minBisect, maxBisect));
		} else if (circle) {
			circleProtrusion(heights);
		}
		else if (!bisect) {
			for (int k = 0; k < numProtrusions; k++) {
				sx = r.Next (minProtSize + 1, w - maxProtSize) - minProtSize;
				sy = r.Next (minProtSize + 1, h - maxProtSize) - minProtSize;
				ex = r.Next (sx, sx + maxProtSize - 1);
				ey = r.Next (sy, sy + maxProtSize - 1);
				
				mult = ((float)r.Next (80, 300)) / 100f;
				for (int i = sx; i < ex; i++) {
					for (int j = sy; j < ey; j++) {
						heights [i, j] *= mult;
					}
				}
				int numPasses = r.Next (minHeightAvg, maxHeightAvg);
				
				for (int i = 0; i < numPasses; i++) {
					avgHeightsInRange (heights, sx, sy, ex, ey);
				}
			}
		}
		
		circleDips (heights);

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
