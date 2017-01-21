using System;
using System.Collections;
using System.Collections.Generic;

public class ModelSurface {

	const float SPRING_RATE_DIVIDED_BY_MASS = 5.0f;
	const float DAMPENING = 1.0f;
	const float WAVE_PROPAGATION_SPEED = 30.0f;

	private float[,] SPREAD_MATRIX = new float[3, 3] {
		{0.103553391f, 0.146446609f, 0.103553391f}, // Magic...
		{0.146446609f, 0.000000000f, 0.146446609f}, 
		{0.103553391f, 0.146446609f, 0.103553391f}
	};

	public int gridSizeX;
	public int gridSizeZ;

	public float[,] vertPos;
	public float[,] vertVel;

	public ModelSurface(int sizeX, int sizeZ) {
		gridSizeX = sizeX;
		gridSizeZ = sizeZ;

		vertPos = new float[sizeX, sizeZ];
		vertVel = new float[sizeX, sizeZ];

		Array.Clear(vertPos, 0, sizeX * sizeZ);
		Array.Clear(vertVel, 0, sizeX * sizeZ);

		vertPos[(int)sizeX / 2 + 1, (int)sizeZ / 2 + 1] = 2f;
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
				float vertAcc = -SPRING_RATE_DIVIDED_BY_MASS * vertPos[x, z] - DAMPENING * vertVel[x, z];
				vertVel[x, z] += vertAcc * deltaTime + WAVE_PROPAGATION_SPEED * vertDeltas[x, z];
				vertPos[x, z] += vertVel[x, z] * deltaTime;
			}
		}
	}

	private float getDeltaAtPoint(int xPos, int zPos) {
		float sumDelta = 0.0f;

		for(int i = 0; i < 3; i++) {
			for(int j = 0; j < 3; j++) {
				int lookAtIndexX = xPos + i - 1;
				int lookAtIndexZ = zPos + j - 1;

				if(lookAtIndexX < 0 || lookAtIndexX >= gridSizeX || lookAtIndexZ < 0 || lookAtIndexZ >= gridSizeZ) {
					continue;
				}
				sumDelta += SPREAD_MATRIX[i, j] * vertPos[lookAtIndexX, lookAtIndexZ];
			}
		}
		return (sumDelta - vertPos[xPos, zPos]);
	}
}
