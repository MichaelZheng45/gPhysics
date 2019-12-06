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


    List<particle3D> readFile3D(string fileName)
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
                particles.Add(buildParticle3D(currentLine));
            }
        }

        sr.Close();
        return particles;
    }

    particle3D buildParticle3D(string name)
    {
        particle3D newParticle = new particle3D();

        newParticle.name = name;

        //Read rest of file
        float x = (float)Convert.ToDouble(sr.ReadLine()),
              y = (float)Convert.ToDouble(sr.ReadLine()),
              z = (float)Convert.ToDouble(sr.ReadLine());

        float elasticity = (float)Convert.ToDouble(sr.ReadLine());
        float sizeX = (float)Convert.ToDouble(sr.ReadLine()),
              sizeY = (float)Convert.ToDouble(sr.ReadLine()),
              sizeZ = (float)Convert.ToDouble(sr.ReadLine());

        newParticle.startingMass = (float)Convert.ToDouble(sr.ReadLine());
        int inertiaType = Convert.ToInt32(sr.ReadLine());
        int rotationType = Convert.ToInt32(sr.ReadLine());
        int positionType = Convert.ToInt32(sr.ReadLine());



        return newParticle;
    }

}
