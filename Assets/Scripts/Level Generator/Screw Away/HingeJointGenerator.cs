#if UNITY_EDITOR
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class HingeJointGenerator : EditorWindow
{
    private GameObject target;

    private string prefabPath = "Assets/Prefabs/MyPrefab.prefab";

    [MenuItem("Saferio/Tools/Hinge Joint Generator")]
    public static void ShowWindow()
    {
        GetWindow<HingeJointGenerator>("Hinge Joint Generator");
    }

    private void OnGUI()
    {
        prefabPath = EditorGUILayout.TextField("Prefab Path", prefabPath);

        target = (GameObject)EditorGUILayout.ObjectField("Target", target, typeof(GameObject), true);

        if (GUILayout.Button("Generate"))
        {
            Generate(prefabPath);
        }

        if (GUILayout.Button("Generate All"))
        {
            GenerateHingeJointAll(prefabPath);
        }

        if (GUILayout.Button("Reset Local Rotation"))
        {
            ResetLocalRotation(target.transform);
        }
    }

    private void GetForwardVector(Transform target)
    {
        Debug.Log(target.forward);
    }

    private void ResetLocalRotation(Transform target)
    {
        target.localPosition = Vector3.zero;
        target.localRotation = Quaternion.Euler(Vector3.zero);
        target.localScale = new Vector3(1 / target.transform.parent.localScale.x, 1 / target.transform.parent.localScale.y, 1 / target.transform.parent.localScale.z);
    }

    private void Generate(string path)
    {
        GameObject levelPrefab = PrefabUtility.LoadPrefabContents(path);

        RemoveOldHingeJoints(target);

        for (int i = 0; i < target.transform.childCount; i++)
        {
            BasicScrew screw = target.transform.GetChild(i).GetComponent<BasicScrew>();

            if (screw != null)
            {
                HingeJoint joint = target.AddComponent<HingeJoint>();

                // WARNING: HingeJoint axis gizmos may not update correctly, so be sure to double-check it.

                joint.connectedBody = screw.GetComponent<Rigidbody>();
                joint.anchor = screw.transform.localPosition;

                // CRUCIAL
                joint.axis = screw.transform.localRotation * Vector3.forward;

                screw.Joint = joint;
            }
        }

        EditorUtility.SetDirty(target);

        PrefabUtility.SaveAsPrefabAsset(levelPrefab, path);
    }

    private void RemoveOldHingeJoints(GameObject objectPart)
    {
        // REMOVE OLD HINGE JOINTS
        HingeJoint[] hingeJoints = objectPart.GetComponents<HingeJoint>();

        for (int I = 0; I < hingeJoints.Length; I++)
        {
            DestroyImmediate(hingeJoints[I]);
        }
    }

    private void GenerateHingeJointAll(string path)
    {
        GameObject levelPrefab = PrefabUtility.LoadPrefabContents(path);

        for (int i = 0; i < levelPrefab.transform.childCount; i++)
        {
            GameObject target = levelPrefab.transform.GetChild(i).gameObject;

            BasicObjectPart objectPart = target.transform.GetComponent<BasicObjectPart>();

            // REMOVE OLD HINGE JOINTS
            RemoveOldHingeJoints(objectPart.gameObject);
            // HingeJoint[] hingeJoints = objectPart.GetComponents<HingeJoint>();

            // for (int j = 0; j < hingeJoints.Length; j++)
            // {
            //     DestroyImmediate(hingeJoints[j]);
            // }

            GenerateHingeJoint(target);
        }

        void GenerateHingeJoint(GameObject target)
        {
            for (int i = 0; i < target.transform.childCount; i++)
            {
                BasicScrew screw = target.transform.GetChild(i).GetComponent<BasicScrew>();

                if (screw != null)
                {
                    HingeJoint joint = target.AddComponent<HingeJoint>();

                    // WARNING: HingeJoint axis gizmos may not update correctly, so be sure to double-check it.

                    joint.connectedBody = screw.GetComponent<Rigidbody>();
                    joint.anchor = screw.transform.localPosition;

                    // CRUCIAL
                    joint.axis = screw.transform.localRotation * Vector3.forward;

                    screw.Joint = joint;
                }
            }
        }

        // for (int i = 0; i < target.transform.childCount; i++)
        // {
        //     BasicScrew screw = target.transform.GetChild(i).GetComponent<BasicScrew>();

        //     if (screw != null)
        //     {
        //         HingeJoint joint = target.AddComponent<HingeJoint>();

        //         // WARNING: HingeJoint axis gizmos may not update correctly, so be sure to double-check it.

        //         joint.connectedBody = screw.GetComponent<Rigidbody>();
        //         joint.anchor = screw.transform.localPosition;

        //         // CRUCIAL
        //         joint.axis = screw.transform.localRotation * Vector3.forward;

        //         screw.Joint = joint;
        //     }
        // }

        EditorUtility.SetDirty(levelPrefab);

        PrefabUtility.SaveAsPrefabAsset(levelPrefab, path);
    }
}
#endif
