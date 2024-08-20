// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;

// [ExecuteInEditMode]
// public class FixPrefab : MonoBehaviour
// {
//     public List<string> prefabs;

//     public List<string> names;

//     public bool fixNow = false;

//     private void Update()
//     {
//         if (fixNow)
//         {
//             fixNow = false;
//             for (int i = 0; i < names.Count; ++i)
//             {
//                 // ������� ���� � ������ �������
//                 string prefabPath = prefabs[i];
//                 GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

//                 if (prefab == null)
//                 {
//                     Debug.LogError("Prefab not found at " + prefabPath);
//                     return;
//                 }

//                 // ������� ��� ������� � ����� � ����� �� ������, ��� � �������
//                 GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

//                 foreach (GameObject obj in allObjects)
//                 {
//                     if (obj.name == names[i])
//                     {
//                         // �������� ������� � ������� �������
//                         Vector3 position = obj.transform.position;
//                         Quaternion rotation = obj.transform.rotation;
//                         Vector3 scale = obj.transform.localScale;

//                         // ������ ������ ������
//                         //DestroyImmediate(obj);

//                         // �������� ����� ��������� ������� �� ����� ������� �������
//                         GameObject newPrefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
//                         newPrefabInstance.transform.position = position;
//                         newPrefabInstance.transform.rotation = rotation;
//                         newPrefabInstance.transform.localScale = scale;
//                     }
//                 }
//             }
//         }
//     }
// }
