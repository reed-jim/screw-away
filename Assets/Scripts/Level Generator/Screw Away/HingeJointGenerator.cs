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
    }

    private void Generate(string path)
    {
        GameObject levelPrefab = PrefabUtility.LoadPrefabContents(path);

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
}
#endif
