using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class particle3dImporter : EditorWindow
{
	CollisionManager3D colManager3D;
	SceneParticleReader reader;
	SceneParticleWriter writer;
	string fileName = "";

	[MenuItem("Window/particleExportImporter")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(particle3dImporter));
	}
	// Update is called once per frame
	void Update()
    {
		if (colManager3D == null)
		{
			colManager3D = GameObject.FindGameObjectWithTag("CollisionManager").GetComponent<CollisionManager3D>();
		}

		if (reader == null)
		{
			reader = new SceneParticleReader();
		}

		if(writer == null)
		{
			writer = new SceneParticleWriter();
		}
	}

	private void OnGUI()
	{
		fileName = EditorGUILayout.TextField("File Name in ParticleFileData", fileName);
		
		if(GUILayout.Button("Import Particle Stuff"))
		{
			if(reader.readFile3D(colManager3D, fileName))	
			{
				Debug.Log("File could not exported");
			}
		}

		if (GUILayout.Button("Export Particle Stuff"))
		{
			writer.writeFile3D(fileName, colManager3D.getAllParticles());
		}
	}
}
