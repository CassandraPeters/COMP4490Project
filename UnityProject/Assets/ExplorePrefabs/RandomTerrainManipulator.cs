using UnityEngine;
using System.Collections;

public class RandomTerrainManipulator : MonoBehaviour {
	private System.Random r;
	private Terrain terr;
	private TerrainData td;

	void averageHeight()
	{

	}

	bool checkBetween(int sx, int ex, int i, int sy, int ey, int j)
	{
		return (sx <= i && ex >= i && sy <= j && ey >= j);
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

		int numProtrusions = r.Next (4, 15);

		float baseHeight = 0.5f;
		float startx = r.Next (99);
		float starty = r.Next (99);

		float mult = ((float)r.Next (80, 300))/100f;

		int sx, sy, ex, ey;


		for (int i = 0; i < w; i++) {
			for (int j = 0; j < h; j++) {
				heights[i,j] = Mathf.PerlinNoise((float)(i + startx) / 80f, (float)(j + starty) / 80f)/3f + 0.2f;
			}
		}
		
		for (int k = 0; k < numProtrusions; k++) {
			sx = r.Next (0, w - 3);
			sy = r.Next (0, h - 3);
			ex = r.Next (sx, w);
			ey = r.Next (sy, h);

			mult = ((float)r.Next (80, 300))/100f;
			for (int i = sx; i < ex; i++) {
				for (int j = sy; j < ey; j++) {
					heights[i,j] *= mult;
				}
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
