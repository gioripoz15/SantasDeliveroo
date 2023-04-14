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
        foreach(var col in colliderfather.GetComponentsInChildren<Collider>())
        {
            DestroyImmediate(col);
        }
    }

    void AddBoxColliderToAllRenderer()
    {
        Remove();
        var childList = colliderfather.GetComponentsInChildren<Transform>();
        for (int i = 0; i < childList.Length; i++)
        {
            if (childList[i].gameObject.GetComponent<MeshRenderer>())
            {
                childList[i].gameObject.AddComponent<BoxCollider>();
            }
        }
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
    }
}
