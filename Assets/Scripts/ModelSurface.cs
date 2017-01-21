using System;
using System.Collections;
using System.Collections.Generic;

public class ModelSurface {

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
		
	}
}
