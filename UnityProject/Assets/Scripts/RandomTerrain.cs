using UnityEngine;
using System;
using System.Collections;

public class RandomTerrain : MonoBehaviour
{
	TerrainData[] terrData; //Array of terrain data for setting heights
	GameObject[] terrList; //List of terrains for instantiating
	bool[,] terrNeighbours;
	
	int terrListCnt;
	
	public float roughness = 5f;
	
	[SerializeField]
	public int maxX = 2; //Number of terrains to generate on X
	
	[SerializeField]
	public int maxZ = 2; //Number of terrains to generate on Z
	
	int terrWidth; //Used to space the terrains when instantiating
	int terrLength;  
	int heightmapWidth; //The size of an individual heightmap
	int heightmapHeight;
	int width; //Total size of heightmaps combined
	int height;
	
	void Start() 
	{
		GenerateTerrains();  
		FractalTerrains();  
	}
	
	void GenerateTerrains()
	{
		int cntX;
		int cntZ;
		terrData = new TerrainData[maxX * maxZ];
		terrList = new GameObject[maxX * maxZ];
		terrListCnt = 0;
		for(cntZ = 0; cntZ < maxZ; cntZ++)
		{
			for(cntX = 0; cntX < maxX; cntX++)
			{    
				if(cntZ == 0 && cntX == 0)
				{
					terrData[0] = GetComponent<Terrain>().terrainData;
					terrList[0] = this.gameObject;
					terrList[terrListCnt].name = "Terrain" + terrListCnt;
					terrListCnt++;
					cntX++;
				}
				terrData[terrListCnt] = new TerrainData() as TerrainData;
				terrData[terrListCnt].heightmapResolution = terrData[0].heightmapResolution;
				terrData[terrListCnt].size = terrData[0].size;
				terrList[terrListCnt]=Terrain.CreateTerrainGameObject(terrData[terrListCnt]);
				terrList[terrListCnt].name = "Terrain" + terrListCnt;
				terrWidth=(int)terrList[0].GetComponent<Terrain>().terrainData.size.x;
				terrLength=(int)terrList[0].GetComponent<Terrain>().terrainData.size.z;
				terrList[terrListCnt].transform.Translate(cntX * terrWidth, 0f, cntZ * terrLength);
				terrListCnt++;
			}
		}
		terrWidth = (int)terrList[0].GetComponent<Terrain>().terrainData.size.x;
		terrLength = (int)terrList[0].GetComponent<Terrain>().terrainData.size.z;
		heightmapWidth = terrList[0].GetComponent<Terrain>().terrainData.heightmapWidth;
		heightmapHeight = terrList[0].GetComponent<Terrain>().terrainData.heightmapHeight;
		int zCnt = 0;
		int xCnt = 0;
		for (int tileCnt = 0; tileCnt < maxX * maxZ; tileCnt++)
		{
			bool left = false;
			bool right = false;
			bool top = false;
			bool bottom = false;
			Terrain leftTerr;
			Terrain rightTerr;
			Terrain topTerr;
			Terrain bottomTerr;
			if(xCnt != 0)
			{
				left = true;
			}
			if(xCnt != maxX - 1)
			{
				right = true;
			}
			if(zCnt != 0)
			{
				bottom = true;
			}
			if(zCnt != maxZ - 1)
			{
				top = true;
			}
			if(left)
			{
				leftTerr = terrList[tileCnt - 1].GetComponent<Terrain>();
			}
			else
			{
				leftTerr = null;
			}
			if(right)
			{
				rightTerr=terrList[tileCnt+1].GetComponent<Terrain>();
			}
			else
			{
				rightTerr=null;
			}
			if(top)
			{
				topTerr = terrList[tileCnt + maxX].GetComponent<Terrain>();
			}
			else
			{
				topTerr = null;
			}
			if(bottom)
			{
				bottomTerr = terrList[tileCnt - maxX].GetComponent<Terrain>();
			}
			else
			{
				bottomTerr = null;
			}  
			terrList[tileCnt].GetComponent<Terrain>().SetNeighbors(leftTerr, topTerr, rightTerr, bottomTerr);   
			terrList[tileCnt].GetComponent<Terrain>().Flush();
			if(xCnt != maxX - 1)
			{
				xCnt++;
			}
			else
			{
				xCnt = 0;
				zCnt++;
			}
		}
	}
	void FractalTerrains()
	{
		width = heightmapWidth * maxX;
		height = heightmapHeight * maxZ;
		float[,] grayArrayFull = new float[width, height];  
		float[,] grayArrayChunk = new float[heightmapWidth, heightmapHeight];
		terrNeighbours = new bool[maxX * maxZ, 4];
		int tileX = 0;
		int tileZ = 0;
		int zCnt = 0;
		int xCnt = 0;
		for(int tileCnt = 0; tileCnt < maxX * maxZ; tileCnt++)
		{
			bool left = false;
			bool right = false;
			bool top = false;
			bool bottom = false;
			if(xCnt != 0)
			{
				left = true;
			}
			if(xCnt != maxX - 1)
			{
				right = true;
			}
			if(zCnt != 0)
			{
				bottom = true;
			}
			if(zCnt != maxZ - 1)
			{
				top = true;
			}
			terrNeighbours[tileCnt,0] = left;
			terrNeighbours[tileCnt,1] = top;
			terrNeighbours[tileCnt,2] = right;
			terrNeighbours[tileCnt,3] = bottom;
			if(xCnt != maxX - 1)
			{
				xCnt++;
			}
			else
			{
				xCnt = 0;
				zCnt++;
			}
		}
		grayArrayFull = Generate(height, width, roughness); 
		int modX = 0;
		int modZ = 0;
		for(int tileCnt = 0; tileCnt < maxX * maxZ; tileCnt++)
		{
			for(int z = 0; z < heightmapHeight; z++)
			{    
				for(int x = 0; x < heightmapWidth; x++)
				{
					if(x == 0 && z == 0)
					{
						modZ = (heightmapHeight * (tileX)) - tileX;
						modX = (heightmapWidth * (tileZ)) - tileZ;
						//Debug.Log(tileCnt+"    "+tileX+"    "+tileZ+"    "+modX+"    "+modZ);
					}
					grayArrayChunk[x,z] = grayArrayFull[x + modX, z + modZ];
				}
			}
			terrData[tileCnt].SetHeights(0, 0, grayArrayChunk);
			terrList[tileCnt].GetComponent<Terrain>().Flush();
			tileX++;
			if(tileX == maxX)
			{
				tileX = 0;
				tileZ++;
			}
		}
	}
	
	float gRoughness;
	float gBigSize;
	
	public float[,] Generate(int iWidth, int iHeight, float iRoughness)
	{
		float c1, c2, c3, c4;
		float[,] points = new float[iWidth + 1, iHeight + 1];
		
		//Assign the four corners of the intial grid random color values
		//These will end up being the colors of the four corners
		c1 = UnityEngine.Random.value;
		c2 = UnityEngine.Random.value;
		c3 = UnityEngine.Random.value;
		c4 = UnityEngine.Random.value;
		
		gRoughness = iRoughness;
		gBigSize = iWidth + iHeight;
		DivideGrid(ref points, 0, 0, iWidth, iHeight, c1, c2, c3, c4);
		return points;
	}
	
	public void DivideGrid(ref float[,] points, float dX, float dY, float dwidth, float dheight, float c1, float c2, float c3, float c4)
	{
		float Edge1, Edge2, Edge3, Edge4, Middle;
		float newWidth = (float)Math.Floor(dwidth / 2);
		float newHeight = (float)Math.Floor(dheight / 2);
		if(dwidth > 1 || dheight > 1)
		{
			Middle = ((c1 + c2 + c3 + c4) / 4) + Displace(newWidth + newHeight); //Randomly displace the midpoint!
			Edge1 = ((c1 + c2) / 2);
			Edge2 = ((c2 + c3) / 2);
			Edge3 = ((c3 + c4) / 2);
			Edge4 = ((c4 + c1) / 2);
			
			//Make sure that the midpoint doesn't accidentally "randomly displaced" past the boundaries!
			Middle= Rectify(Middle);
			Edge1 = Rectify(Edge1);
			Edge2 = Rectify(Edge2);
			Edge3 = Rectify(Edge3);
			Edge4 = Rectify(Edge4);
			
			//Do the operation over again for each of the four new grids.
			DivideGrid(ref points, dX, dY, newWidth, newHeight, c1, Edge1, Middle, Edge4);
			DivideGrid(ref points, dX + newWidth, dY, dwidth - newWidth, newHeight, Edge1, c2, Edge2, Middle);
			DivideGrid(ref points, dX + newWidth, dY + newHeight, dwidth - newWidth, dheight - newHeight, Middle, Edge2, c3, Edge3);
			DivideGrid(ref points, dX, dY + newHeight, newWidth, dheight - newHeight, Edge4, Middle, Edge3, c4);
		}
		else
		{
			//The four corners of the grid piece will be averaged and drawn as a single pixel.
			float c = (c1 + c2 + c3 + c4) / 4;
			points[(int)(dX), (int)(dY)] = c;
			if(dwidth == 2)
			{
				points[(int)(dX + 1), (int)(dY)] = c;
			}
			if(dheight == 2)
			{
				points[(int)(dX), (int)(dY + 1)] = c;
			}
			if((dwidth == 2) && (dheight == 2))
			{
				points[(int)(dX + 1), (int)(dY + 1)] = c;
			}
		}
	}
	
	private float Rectify(float iNum)
	{
		if(iNum < 0)
		{
			iNum = 0;
		}
		else if (iNum > 1.0)
		{
			iNum = 1.0f;
		}
		return iNum;
	}
	
	private float Displace(float SmallSize)
	{
		float Max = SmallSize/gBigSize * gRoughness;
		return (float)(UnityEngine.Random.value - 0.5) * Max;
	}   
}