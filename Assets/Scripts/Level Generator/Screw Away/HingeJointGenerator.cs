#if UNITY_EDITOR
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using static GameEnum;

public class HingeJointGenerator : EditorWindow
{
    private GameObject target;
    private GameObject screwPrefab;



    private Vector3 dimensionSize;
    private Vector3 distance;
    private float expectedScale;



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
        screwPrefab = (GameObject)EditorGUILayout.ObjectField("Screw Prefab", screwPrefab, typeof(GameObject), true);

        dimensionSize = EditorGUILayout.Vector3Field("Dimension Size", dimensionSize);
        distance = EditorGUILayout.Vector3Field("Distance", distance);
        expectedScale = EditorGUILayout.FloatField("Expected Scale", expectedScale);

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

        if (GUILayout.Button("Flip Horizontally"))
        {
            FlipHorizontally(Selection.activeTransform);
        }

        if (GUILayout.Button("Flip Vertically"))
        {
            FlipVertically(Selection.activeTransform);
        }

        if (GUILayout.Button("Auto Assign Screw Faction"))
        {
            AutoAssignScrewFaction(prefabPath);
        }

        if (GUILayout.Button("Spawn Screw"))
        {
            SpawnScrew(prefabPath);
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

    private void FlipHorizontally(Transform target)
    {
        target.transform.Rotate(0, 180f, 0);
    }

    private void FlipVertically(Transform target)
    {
        target.transform.Rotate(180f, 0, 0);
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

    private void AutoAssignScrewFaction(string path)
    {
        GameObject levelPrefab = PrefabUtility.LoadPrefabContents(path);

        BaseScrew[] screws = GetComponentsFromAllChildren<BaseScrew>(levelPrefab.transform).ToArray();

        GameFaction[] factions = new GameFaction[5] { GameFaction.Blue, GameFaction.Red, GameFaction.Green, GameFaction.Purple, GameFaction.Orange };

        int currentFaction = 0;

        for (int i = 0; i < screws.Length; i++)
        {
            screws[i].Faction = factions[currentFaction];

            if (i > 0 && (i + 1) % 3 == 0)
            {
                currentFaction++;

                if (currentFaction >= factions.Length)
                {
                    currentFaction = 0;
                }
            }
        }

        EditorUtility.SetDirty(levelPrefab);

        PrefabUtility.SaveAsPrefabAsset(levelPrefab, path);
    }

    private void SpawnScrew(string path)
    {
        GameObject levelPrefab = PrefabUtility.LoadPrefabContents(path);

        Vector3 position = new Vector3();

        int currentX = 0;
        int currentY = 0;
        int currentZ = 0;

        for (int i = 0; i < dimensionSize.x * dimensionSize.y * dimensionSize.z; i++)
        {
            GameObject screw = (GameObject)PrefabUtility.InstantiatePrefab(screwPrefab, target.transform);

            position.x = (-(dimensionSize.x - 1) / 2f + currentX) * distance.x + target.transform.position.x;
            position.y = (-(dimensionSize.y - 1) / 2f + currentY) * distance.y + target.transform.position.y;
            position.z = (-(dimensionSize.z - 1) / 2f + currentZ) * distance.z + target.transform.position.z;

            currentX++;

            if (currentX >= dimensionSize.x)
            {
                currentY++;

                if (currentY >= dimensionSize.y)
                {
                    currentZ++;

                    currentY = 0;
                }

                currentX = 0;
            }

            // for (int j = 0; j < dimensionSize.x; j++)
            // {
            //     position.x = (-(dimensionSize.x - 1) / 2f + currentX) * distance.x + target.transform.position.x;
            // }

            // for (int j = 0; j < dimensionSize.y; j++)
            // {
            //     position.y = (-(dimensionSize.y - 1) / 2f + currentY) * distance.y + target.transform.position.y;
            // }

            // for (int j = 0; j < dimensionSize.z; j++)
            // {
            //     position.z = (-(dimensionSize.z - 1) / 2f + currentZ) * distance.z + target.transform.position.z;
            // }

            screw.transform.position = position;
            screw.transform.localScale = TransformUtil.ComponentWiseDivine(expectedScale * Vector3.one, target.transform.localScale);
        }




        // for (int i = 0; i < dimensionSize.x; i++)
        // {
        //     for (int j = 0; j < dimensionSize.y; j++)
        //     {
        //         GameObject screw = (GameObject)PrefabUtility.InstantiatePrefab(screwPrefab, target.transform);

        //         Vector3 position;

        //         position.x = (-(dimensionSize.x - 1) / 2f + j) * distance.x;
        //         position.y = (-(dimensionSize.y - 1) / 2f + i) * distance.y;
        //         position.z = (-(dimensionSize.y - 1) / 2f + i) * distance.z;

        //         position += target.transform.position;

        //         screw.transform.position = position;
        //         screw.transform.localScale = TransformUtil.ComponentWiseDivine(expectedScale * Vector3.one, target.transform.localScale);
        //     }
        // }

        EditorUtility.SetDirty(levelPrefab);

        PrefabUtility.SaveAsPrefabAsset(levelPrefab, path);
    }


    #region UTIL
    public List<T> GetComponentsFromAllChildren<T>(Transform parent) where T : Component
    {
        List<T> components = new List<T>();
        GetComponentsFromAllChildrenRecursive<T>(parent, components);
        return components;
    }

    private void GetComponentsFromAllChildrenRecursive<T>(Transform parent, List<T> components) where T : Component
    {
        T component = parent.GetComponent<T>();
        if (component != null)
        {
            components.Add(component);
        }

        foreach (Transform child in parent)
        {
            GetComponentsFromAllChildrenRecursive<T>(child, components);
        }
    }
    #endregion
}
#endif
