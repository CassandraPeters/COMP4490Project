using UnityEngine;
using System.Collections;

public class RandomTerrainManipulator : MonoBehaviour {
	private System.Random r;
	private Terrain terr;
	private TerrainData td;

	public int minHeightAvg = 1;
	public int maxHeightAvg = 10;
	public int minProtrusions = 5;
	public int maxProtrusions = 20;

	void averageHeight()
	{

	}

	bool checkBetween(int sx, int ex, int i, int sy, int ey, int j)
	{
		return (sx <= i && ex >= i && sy <= j && ey >= j);
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

	// Use this for initialization
	void Start () {
		r = new System.Random ();
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

		float baseHeight = 0.5f;
		float startx = r.Next (99);
		float starty = r.Next (99);

		float mult = ((float)r.Next (80, 300))/100f;

		int sx, sy, ex, ey;
		int maxProtSize = 200;
		int minProtSize = 10;


		for (int i = 0; i < w; i++) {
			for (int j = 0; j < h; j++) {
				heights[i,j] = Mathf.PerlinNoise((float)(i + startx) / 80f, (float)(j + starty) / 80f)/3f;
			}
		}
		
		for (int k = 0; k < numProtrusions; k++) {
			sx = r.Next (minProtSize + 1, w - maxProtSize) - minProtSize;
			sy = r.Next (minProtSize + 1, h - maxProtSize) - minProtSize;
			ex = r.Next (sx, sx+maxProtSize-1);
			ey = r.Next (sy, sy+maxProtSize-1);

			mult = ((float)r.Next (80, 300))/100f;
			for (int i = sx; i < ex; i++) {
				for (int j = sy; j < ey; j++) {
					heights[i,j] *= mult;
				}
			}
			int numPasses = r.Next (minHeightAvg, maxHeightAvg);
			
			for (int i = 0; i < numPasses; i++) {
				avgHeightsInRange(heights,sx,sy,ex,ey);
			}
		}

		td.SetHeights (0, 0, heights);

		/*
		for (int i = 0; i < w; i++) {
			for (int j = 0; j < h; j++) {
				heights[i,j] = baseHeight;
			}
		}
		td.SetHeights (0, 0, heights);
		
		for (int i = 0; i < w; i++) {
			for (int j = 0; j < h; j++) {
				left = baseHeight;
				up = baseHeight;
				min = baseHeight;
				max = baseHeight;
				tendency = 0;
				if (i - 1 >= 0)
				{
					left = heights[i-1,j];
					if (left < baseHeight)
						tendency--;
					else
						tendency++;
				}
				if (j - 1 >= 0)
				{
					up = heights[i, j-1];
					if (up < baseHeight)
						tendency--;
					else
						tendency++;
				}
				
				if (left > up)
				{
					max = left;
					min = up;
				}
				else
				{
					max = up;
					min = left;
				}
				
				if (max - min <= 2*diff)
				{
					min -=2*diff;
					max += 2*diff;
				}
				
				min -= diff;
				max += diff;
				
				if (tendency < 0)
				{
					min -= diff;
				}
				else if (tendency > 0)
				{
					max += diff;
				}
				
				heights[i,j] = getHeight(min, max);
			}
		}
		td.SetHeights (0, 0, heights);

		int jump;*/
	}

	private float getHeight(float min, float max)
	{
		return (float)(min + (r.NextDouble() * (max-min)));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
