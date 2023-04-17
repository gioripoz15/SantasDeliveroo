using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RemoveAllCollidersEditor : EditorWindow
{
    GameObject colliderfather;
    bool button;

    [MenuItem("Utility/RemoveAllCollidersFrom")]
    static void Init()
    {
        var window = GetWindowWithRect<RemoveAllCollidersEditor>(new Rect(0, 0, 165, 100));
        window.Show();
    }

   void Remove()
    {
        int count = 0;
        foreach (var col in colliderfather.GetComponentsInChildren<Collider>())
        {
            DestroyImmediate(col);
            count++;
        }
        Debug.Log($"removed {count} colliders");
    }

    void AddBoxColliderToAllRenderer()
    {
        //Remove();
        var childList = colliderfather.GetComponentsInChildren<Transform>();
        int count = 0;
        for (int i = 0; i < childList.Length; i++)
        {
            if (childList[i].gameObject.GetComponent<MeshRenderer>())
            {
                childList[i].gameObject.AddComponent<BoxCollider>();
                count++;
            }
        }
        Debug.Log($"Added {count} colliders");
    }

    void AddBoxColliderToAllRendererFather()
    {
        //Remove();
        var childList = colliderfather.GetComponentsInChildren<Transform>();
        int count = 0;
        for (int i = 0; i < childList.Length; i++)
        {
            if (childList[i].gameObject.GetComponent<MeshRenderer>())
            {
                if (childList[i].parent != colliderfather)
                {
                    var childcoll = childList[i].gameObject.AddComponent<BoxCollider>();
                    var parentcoll = childList[i].parent.gameObject.AddComponent<BoxCollider>();
                    Vector3 scale = parentcoll.size;
                    scale.x = childcoll.size.x * childcoll.transform.localScale.x;
                    scale.y = childcoll.size.y * childcoll.transform.localScale.y;
                    scale.z = childcoll.size.z * childcoll.transform.localScale.z;
                    parentcoll.size = scale;
                    parentcoll.center = childcoll.center + childcoll.transform.position;
                    DestroyImmediate(childcoll);
                    count++;
                }
                
            }
        }
        Debug.Log($"Added {count} colliders");
    }


    private void OnGUI()
    {
        colliderfather = (GameObject)EditorGUILayout.ObjectField(colliderfather, typeof(GameObject));
        if (GUILayout.Button("RemoveAll"))
        {
            Remove();
        }
        if (GUILayout.Button("AddBoxToAll"))
        {
            AddBoxColliderToAllRenderer();
        }
        if (GUILayout.Button("AddBoxToAllFathers"))
        {
            AddBoxColliderToAllRendererFather();
        }
    }
}
