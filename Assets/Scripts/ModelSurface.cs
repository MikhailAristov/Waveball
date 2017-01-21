using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSurface {

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
	float[,] vertDeltas;

	public bool[,] obstactleMap;
	public bool[,] oscillatorMap;

	public ModelSurface(int sizeX, int sizeZ) {
		gridSizeX = sizeX;
		gridSizeZ = sizeZ;

		vertPos = new float[sizeX, sizeZ];
		vertVel = new float[sizeX, sizeZ];
		vertDeltas = new float[sizeX, sizeZ];
		obstactleMap = new bool[sizeX, sizeZ];
		oscillatorMap = new bool[sizeX, sizeZ];

		for(int x = 0; x < gridSizeX; x++) {
			for(int z = 0; z < gridSizeZ; z++) {
				obstactleMap[x, z] = (x == 0 || x == gridSizeX - 1 || z == 0 || z == gridSizeZ - 1);
			}
		}
	}

	public void update(float deltaTime, float springRateDividedByMass, float dampening, float wavePropagationSpeed) {
		// Propagation prepare
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

				if(oscillatorMap[x, z]) {
					float vertAcc = -springRateDividedByMass * vertPos[x, z];
					vertVel[x, z] += vertAcc * deltaTime;
					vertPos[x, z] += vertVel[x, z] * deltaTime;
					continue;
				}

				if(Math.Abs(vertPos[x, z]) >= POSITION_THRESHOLD || getDeltaAtPoint(x, z) >= POSITION_THRESHOLD) {
					float vertAcc = -springRateDividedByMass * vertPos[x, z] - dampening * vertVel[x, z];
					vertVel[x, z] += vertAcc * deltaTime + wavePropagationSpeed * vertDeltas[x, z];
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

	public void setPulseAtPoint(int xPos, int zPos, float pulseForce) {
		vertVel[xPos, zPos] = -pulseForce;
		vertPos[xPos, zPos] -= POSITION_THRESHOLD * 10;
	}
		
	public void toggleOscillatorAtPosition(int xPos, int zPos, float amplitude) {
		if(oscillatorMap[xPos, zPos]) {
			oscillatorMap[xPos, zPos] = false;
		} else {
			vertPos[xPos, zPos] = amplitude;
			oscillatorMap[xPos, zPos] = true;
		}
	}
}
