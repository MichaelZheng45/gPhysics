using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class particleEditor : EditorWindow
{
    enum particleEditorMode
    {
        EDIT_PARTICLE,
        CREATE_NEW,
        CHANGE_HULL,
        FORCEGENERATOR
    }

    public GameObject particleObj;
    particle3D particleData;
    CollisionManager3D colManager3D;

    particleEditorMode editorMode = 0;
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
            GUILayout.Label("Create New Particle?");

            if (chooseCreateParticle())
            {
                //starting stuff
                particleData = particleObj.GetComponent<particle3D>();
                particleData.size = new Vector3(1, 1, 1);
                colManager3D.addNew(particleObj);
            }
            
        }
        else
        {
            particleData = particleObj.GetComponent<particle3D>();
            particleObj.name = EditorGUILayout.TextField("Name", particleObj.name);
            GUILayout.Label("Particle Options", EditorStyles.boldLabel);
            editorMode = (particleEditorMode)EditorGUILayout.EnumPopup("EditorOptions", editorMode);
            switch (editorMode)
            {
                case particleEditorMode.CREATE_NEW:
                    if (chooseCreateParticle())
                    {
                        //starting stuff
                        particleData = particleObj.GetComponent<particle3D>();
                        particleData.size = new Vector3(1, 1, 1);
                        colManager3D.addNew(particleObj);
                        editorMode = particleEditorMode.EDIT_PARTICLE;
                    }
                    break;
                case particleEditorMode.CHANGE_HULL:
                    changeHull();
                    break;
                case particleEditorMode.FORCEGENERATOR:
                    ForceStuff();
                    break;
                case particleEditorMode.EDIT_PARTICLE:
                    particleModding();
                    break;
                default:
                    break;
            }

        }
    }

    Transform target;
    void ForceStuff()
    {
        GUILayout.Space(20);
        particleData.initialForce = EditorGUILayout.Toggle("Initial Force", particleData.initialForce);
        if(particleData.initialForce)
        {
            target = EditorGUILayout.ObjectField("TargetDirection", target, typeof(Transform), true) as Transform;
            if(target)
            {
                Vector3 dir = (target.position - particleData.position).normalized;
                particleData.initialDir = dir;
                Debug.DrawLine(particleData.position, particleData.position + particleData.initialDir);
                target = null;
            }
            GUILayout.Label(particleData.initialDir.ToString());
            particleData.initialForceMagnitude = EditorGUILayout.FloatField("Force Magnitude", particleData.initialForceMagnitude);
        }

        GUILayout.Space(20);
        particleData.gravityOn = EditorGUILayout.Toggle("Activate Gravity", particleData.gravityOn);
        if (particleData.gravityOn)
        {
            particleData.gravityStrength = EditorGUILayout.FloatField("Gravity Magnitude", particleData.gravityStrength);
        }
    }

    void changeHull()
    {
        GUILayout.Space(20);
        GameObject curParticle = particleObj;
        if(chooseCreateParticle())
        {
            particleObj.GetComponent<particle3D>().setBase(curParticle.GetComponent<particle3D>());
            particleObj.name = curParticle.name;
            DestroyImmediate(curParticle);
            editorMode = particleEditorMode.EDIT_PARTICLE;
        }
    }

    void particleModding()
    {
        GUILayout.Space(20);
        GUILayout.Label("Edit Particle Data", EditorStyles.boldLabel);

        GUILayout.Label("ParticleModes");
        particleData.positionMode = (positionUpdate)EditorGUILayout.EnumPopup("PositionMode", particleData.positionMode);
        particleData.rotationMode = (rotationUpdate)EditorGUILayout.EnumPopup("RotationMode", particleData.rotationMode);
        particleData.i_mode = (InertiaTypes3D)EditorGUILayout.EnumPopup("Inertia Mode", particleData.i_mode);
        particleData.updateInertia();

        GUILayout.Space(20);

        GUILayout.Label("Physics Data");
        particleData.startingMass = EditorGUILayout.FloatField("Starting Mass", particleData.startingMass);
        particleData.setMass(particleData.startingMass);

        particleData.elasticity = EditorGUILayout.Slider(particleData.elasticity, 0, 1);

        GUILayout.Space(20);
        particleData.position = EditorGUILayout.Vector3Field("Starting Position", particleData.position);
        particleObj.transform.position = particleData.position;

        particleObj.transform.eulerAngles = EditorGUILayout.Vector3Field("Starting Rotation", particleData.transform.eulerAngles);
        particleData.size = EditorGUILayout.Vector3Field("Size/Scale", particleData.size);
        particleObj.transform.localScale = particleData.size;

        if (GUILayout.Button("CopyParticle"))
        {
            particleObj = Instantiate(particleObj) as GameObject;
            colManager3D.addNew(particleObj);
        }

        if (GUILayout.Button("RemoveParticle"))
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
