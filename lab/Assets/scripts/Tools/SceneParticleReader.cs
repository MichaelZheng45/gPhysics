using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class SceneParticleReader
{
    StreamReader sr;
    const string filePath = "Assets/ParticleFileData/";
    List<particle3D> particles;
    List<particle2D> particle2ds;


    public bool readFile3D(CollisionManager3D colManager,string fileName)
    {
        List<particle3D> particles = new List<particle3D>();
        string fullPath = filePath + fileName;
        sr = new StreamReader(fullPath);

        string currentLine = "";

        while(currentLine != "EoF")
        {
            currentLine = sr.ReadLine();

            if(currentLine != "Eof")
            {
				particle3D particle = buildParticle3D(currentLine);

				particles.Add(particle);
				colManager.addNew(particle.gameObject);
            }
        }

        sr.Close();
		return true;
    }

    particle3D buildParticle3D(string name)
    {
        //Read rest of file
        float x = (float)Convert.ToDouble(sr.ReadLine()),
              y = (float)Convert.ToDouble(sr.ReadLine()),
              z = (float)Convert.ToDouble(sr.ReadLine());

        float elasticity = (float)Convert.ToDouble(sr.ReadLine());
        float sizeX = (float)Convert.ToDouble(sr.ReadLine()),
              sizeY = (float)Convert.ToDouble(sr.ReadLine()),
              sizeZ = (float)Convert.ToDouble(sr.ReadLine());

		float RotW = (float)Convert.ToDouble(sr.ReadLine()),
			  RotX = (float)Convert.ToDouble(sr.ReadLine()),
			  RotY = (float)Convert.ToDouble(sr.ReadLine()),
			  RotZ = (float)Convert.ToDouble(sr.ReadLine());

		float startingMass = (float)Convert.ToDouble(sr.ReadLine());
        int inertiaType = Convert.ToInt32(sr.ReadLine());
        int rotationType = Convert.ToInt32(sr.ReadLine());
        int positionType = Convert.ToInt32(sr.ReadLine());
		int hullType = Convert.ToInt32(sr.ReadLine());

		GameObject newParticle = null;
		switch ((CollisionHull3D.CollisionHullType3D)hullType)
		{
			case CollisionHull3D.CollisionHullType3D.hull_circle:
				newParticle = GameObject.Instantiate(Resources.Load("Prefabs/Sphere"), new Vector3(x, y, z), new Quaternion(RotW, RotX, RotY, RotZ)) as GameObject;
				break;
			case CollisionHull3D.CollisionHullType3D.hull_aabb:
				newParticle = GameObject.Instantiate(Resources.Load("Prefabs/AAB"), new Vector3(x, y, z), new Quaternion(RotW, RotX, RotY, RotZ)) as GameObject;
				break;
			case CollisionHull3D.CollisionHullType3D.hull_obb:
				newParticle = GameObject.Instantiate(Resources.Load("Prefabs/OBB"), new Vector3(x,y,z), new Quaternion(RotW, RotX,RotY,RotZ)) as GameObject;
				break;
		}
		newParticle.name = name;
		particle3D particleData = newParticle.GetComponent<particle3D>();
		particleData.position = new Vector3(x, y, z);
		particleData.rotation = new Quaternion4D(RotW, RotX, RotY, RotZ);
		particleData.size = new Vector3(sizeX, sizeY, sizeZ);
		particleData.startingMass = startingMass;
		particleData.setInertiaType(inertiaType);
		particleData.setRotationType(rotationType);
		particleData.setPositionType(positionType);

		return particleData;
    }

}
