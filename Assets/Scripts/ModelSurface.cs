using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSurface {

	const float POSITION_THRESHOLD = 0.0000001f;

	public int gridSizeX;
	public int gridSizeZ;

	public float[,] vertPos;
	public float[,] vertVel;
	public float[,] vertDeltas;

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

	public void reset() {
		Array.Clear(vertPos, 0, gridSizeX * gridSizeZ);
		Array.Clear(vertVel, 0, gridSizeX * gridSizeZ);
		Array.Clear(vertDeltas, 0, gridSizeX * gridSizeZ);
		Array.Clear(oscillatorMap, 0, gridSizeX * gridSizeZ);
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

		int lookAtIndexX; int lookAtIndexZ; float distanceFactor;
		for(int i = -1; i <= 1; i++) {
			lookAtIndexX = xPos + i;
			for(int j = -1; j <= 1; j++) {
				lookAtIndexZ = zPos + j;
				distanceFactor = Mathf.Sqrt(Mathf.Abs(i) + Mathf.Abs(j));

				if(!obstactleMap[lookAtIndexX, lookAtIndexZ]) {
					sumDelta += distanceFactor * vertPos[lookAtIndexX, lookAtIndexZ];
				}
			}
		}
		sumDelta /= (4 + 4 * Mathf.Sqrt(2)); // the sum has to be normalized to the sum of the distanceFactors 
		return (sumDelta - vertPos[xPos, zPos]);
	}

	public Vector3 getGradientAtPoint(int xPos, int zPos) {
		Vector3 gradient = new Vector3(0f, 0f, 0f); 

		if(obstactleMap[xPos, zPos]) {
			return gradient;
		}

		int lookAtIndexX; int lookAtIndexZ;
		for(int i = -1; i <= 1; i++) {
			lookAtIndexX = xPos + i;
			for(int j = -1; j <= 1; j++) {
				lookAtIndexZ = zPos + j;

				if(!obstactleMap[lookAtIndexX, lookAtIndexZ]) {
					gradient.x += (float)i * vertPos[lookAtIndexX, lookAtIndexZ];
					gradient.z += (float)j * vertPos[lookAtIndexX, lookAtIndexZ];
				}
			}
		}

		return gradient / 6;
	}

	public void setPulseAtPoint(int xPos, int zPos, float pulseForce) {
		if(!oscillatorMap[xPos, zPos]) {
			vertVel[xPos, zPos] = -pulseForce;
			vertPos[xPos, zPos] -= POSITION_THRESHOLD * 10;
		}
	}
		
	public void toggleOscillatorAtPosition(int xPos, int zPos, float amplitude) {
		if(obstactleMap[xPos, zPos]) {
			return;
		}

		if(oscillatorMap[xPos, zPos]) {
			oscillatorMap[xPos, zPos] = false;
		} else {
			vertPos[xPos, zPos] = amplitude;
			oscillatorMap[xPos, zPos] = true;
		}
	}

	public float getTotalEnergy(float springRate) {
		float result = 0.0f;

		for(int x = 0; x < gridSizeX; x++) {
			for(int z = 0; z < gridSizeZ; z++) {
				result += springRate * vertPos[x, z] * vertPos[x, z] + vertVel[x, z] * vertVel[x, z];
			}
		}

		return result / 2;
	}
}
