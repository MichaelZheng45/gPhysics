using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SceneParticleWriter
{
    StreamWriter sw;
    const string filePath = "Assets/ParticleFileData/";
    void writeFile3D(string fileName, particle3D[] particles)
    {
        string fullPath = filePath + fileName;
        sw = new StreamWriter(fullPath);

        foreach(particle3D p in particles)
        {
            //Name
            sw.WriteLine(p.name);
            //Position
            sw.WriteLine(p.position.x);        
            sw.WriteLine(p.position.y);        
            sw.WriteLine(p.position.z);        
            sw.WriteLine(p.elasticity);

            sw.WriteLine(p.getMass());         
            sw.WriteLine(p.getInertiaType());  
            sw.WriteLine(p.getRotationType()); 
            sw.WriteLine(p.getPositionType());
            sw.WriteLine(p.getCollisionHullType());

            sw.WriteLine(p.size.x);
            sw.WriteLine(p.size.y);
            sw.WriteLine(p.size.z);
        }
    }

    void writeFile3D(string fileName, particle2D[] particles)
    {

    }
}
