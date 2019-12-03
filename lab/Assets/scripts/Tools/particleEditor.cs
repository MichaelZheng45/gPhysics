using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class particleEditor : EditorWindow
{
    public GameObject particleObj;
    particle3D particleData;
    CollisionManager3D colManager3D;

    bool createNewParticle = false;
    bool switchParticleHull = false;
    bool forceOptions = false;
    [MenuItem("Window/ParticleEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(particleEditor));
    }

    // Update is called once per frame
    void Update()
    {
        if (colManager3D == null)
        {
            colManager3D = GameObject.FindGameObjectWithTag("CollisionManager").GetComponent<CollisionManager3D>();
        }
    }

    private void OnGUI()
    {
        particleObj = EditorGUILayout.ObjectField("Particle Object", particleObj, typeof(GameObject), true) as GameObject;

        if(!particleObj)
        {
            GUILayout.Label("No Particle Game Object:");
            createNewParticle = EditorGUILayout.Toggle("Create New Particle", createNewParticle);

            if(createNewParticle)
            {
                if(chooseCreateParticle())
                {
                    //starting stuff
                    particleData = particleObj.GetComponent<particle3D>();
                    particleData.size = new Vector3(1, 1, 1);
                    colManager3D.addNew(particleObj);
                    createNewParticle = false;
                }
            }
        }
        else
        {
            particleData = particleObj.GetComponent<particle3D>();
            particleObj.name = EditorGUILayout.TextField("Name", particleObj.name);
            GUILayout.Label("Particle Options", EditorStyles.boldLabel);
            particleModding();
        }
    }

    void particleModding()
    {
        GUILayout.Label("ParticleModes");
        particleData.positionMode = (positionUpdate)EditorGUILayout.EnumPopup("PositionMode", particleData.positionMode);
        particleData.rotationMode = (rotationUpdate)EditorGUILayout.EnumPopup("RotationMode", particleData.rotationMode);
        particleData.i_mode = (InertiaTypes3D)EditorGUILayout.EnumPopup("Inertia Mode", particleData.i_mode);
        particleData.updateInertia();

        GUILayout.Space(20);

        GUILayout.Label("Physics Data");
        particleData.startingMass = EditorGUILayout.FloatField("Starting Mass", particleData.startingMass);
        particleData.setMass(particleData.startingMass);

        particleData.position = EditorGUILayout.Vector3Field("Starting Position", particleData.position);
        particleObj.transform.position = particleData.position;

        particleObj.transform.eulerAngles = EditorGUILayout.Vector3Field("Starting Rotation", particleData.transform.eulerAngles);
        particleData.size = EditorGUILayout.Vector3Field("Size/Scale", particleData.size);
        particleObj.transform.localScale = particleData.size;

        if(GUILayout.Button("RemoveParticle"))
        {
            colManager3D.removeOld(particleObj);
            DestroyImmediate(particleObj);
        }
    }

    bool chooseCreateParticle()
    {

        if(GUILayout.Button("GenerateCircle"))
        {
            particleObj = Instantiate(Resources.Load("Prefabs/Sphere")) as GameObject;
            return true;
        }

        if (GUILayout.Button("GenerateAAB"))
        {
            particleObj = Instantiate(Resources.Load("Prefabs/AAB")) as GameObject;
            return true;
        }

        if (GUILayout.Button("GenerateOBB"))
        {
            particleObj = Instantiate(Resources.Load("Prefabs/OBB")) as GameObject;
            return true;
        }

        return false;
    }
}
