using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSurface {

	const float SPRING_RATE_DIVIDED_BY_MASS = 2.0f;
	const float DAMPENING = 0.2f;
	const float WAVE_PROPAGATION_SPEED = 5.0f;

	const float POSITION_THRESHOLD = 0.0000001f;

	private float[,] SPREAD_MATRIX = new float[3, 3] {
		{0.103553391f, 0.146446609f, 0.103553391f}, // Magic...
		{0.146446609f, 0.000000000f, 0.146446609f}, 
		{0.103553391f, 0.146446609f, 0.103553391f}
	};

	private float[,] X_GRADIENT_MATRIX = new float[3, 3] {
		{-1f/6f, 0f, 1f/6f}, // Previtt
		{-1f/6f, 0f, 1f/6f}, 
		{-1f/6f, 0f, 1f/6f}
	};

	private float[,] Z_GRADIENT_MATRIX = new float[3, 3] {
		{-1f/6f, -1f/6f, -1f/6f}, // Previtt
		{0f, 0f, 0f}, 
		{1f/6f, 1f/6f, 1f/6f}
	};

	public int gridSizeX;
	public int gridSizeZ;

	public float[,] vertPos;
	public float[,] vertVel;

	public bool[,] obstactleMap;

	public ModelSurface(int sizeX, int sizeZ) {
		gridSizeX = sizeX;
		gridSizeZ = sizeZ;

		vertPos = new float[sizeX, sizeZ];
		vertVel = new float[sizeX, sizeZ];
		obstactleMap = new bool[sizeX, sizeZ];

		Array.Clear(vertPos, 0, sizeX * sizeZ);
		Array.Clear(vertVel, 0, sizeX * sizeZ);
		Array.Clear(obstactleMap, 0, sizeX * sizeZ);

		for(int x = 0; x < gridSizeX; x++) {
			for(int z = 0; z < gridSizeZ; z++) {
				obstactleMap[x, z] = (x == 0 || x == gridSizeX - 1 || z == 0 || z == gridSizeZ - 1);
			}
		}

		vertPos[(int)sizeX / 2 + 10, (int)sizeZ / 2 - 10] = 2f;
	}

	public void update(float deltaTime) {
		// Propagation prepare
		float[,] vertDeltas = new float[gridSizeX, gridSizeZ];
		for(int x = 0; x < gridSizeX; x++) {
			for(int z = 0; z < gridSizeZ; z++) {
				vertDeltas[x, z] = getDeltaAtPoint(x, z);
			}
		}

		for(int x = 0; x < gridSizeX; x++) {
			for(int z = 0; z < gridSizeZ; z++) {
				if(obstactleMap[x, z]) {
					continue;
				}

				if(Math.Abs(vertPos[x, z]) >= POSITION_THRESHOLD || getDeltaAtPoint(x, z) >= POSITION_THRESHOLD) {
					float vertAcc = -SPRING_RATE_DIVIDED_BY_MASS * vertPos[x, z] - DAMPENING * vertVel[x, z];
					vertVel[x, z] += vertAcc * deltaTime + WAVE_PROPAGATION_SPEED * vertDeltas[x, z];
					vertPos[x, z] += vertVel[x, z] * deltaTime;
				}
			}
		}
	}

	private float getDeltaAtPoint(int xPos, int zPos) {
		float sumDelta = 0.0f;

		if(obstactleMap[xPos, zPos]) {
			return sumDelta;
		}

		for(int i = 0; i < 3; i++) {
			for(int j = 0; j < 3; j++) {
				int lookAtIndexX = xPos + i - 1;
				int lookAtIndexZ = zPos + j - 1;

				if(!obstactleMap[lookAtIndexX, lookAtIndexZ]) {
					sumDelta += SPREAD_MATRIX[i, j] * vertPos[lookAtIndexX, lookAtIndexZ];
				}
			}
		}
		return (sumDelta - vertPos[xPos, zPos]);
	}

	public Vector3 getGradientAtPoint(int xPos, int zPos) {
		Vector3 gradient = new Vector3(0f, 0f, 0f); 

		if(obstactleMap[xPos, zPos]) {
			return gradient;
		}


		for(int i = 0; i < 3; i++) {
			for(int j = 0; j < 3; j++) {
				int lookAtIndexX = xPos + i - 1;
				int lookAtIndexZ = zPos + j - 1;

				if(!obstactleMap[lookAtIndexX, lookAtIndexZ]) {
					gradient.x += X_GRADIENT_MATRIX[i, j] * vertPos[lookAtIndexX, lookAtIndexZ];
					gradient.z += Z_GRADIENT_MATRIX[i, j] * vertPos[lookAtIndexX, lookAtIndexZ];
				}
			}
		}

		return gradient;
	}
}
