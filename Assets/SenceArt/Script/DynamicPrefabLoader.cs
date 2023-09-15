using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class DynamicPrefabLoader : MonoBehaviour
{
    
    public string prefabFolderPath; // 预制体所在的文件夹路径
    public float loadInterval = 1f; // 加载间隔时间
    private Dictionary<Object, GameObject> instantiatedPrefabs = new Dictionary<Object, GameObject>(); // 已实例化的预制体字典
    
    public Vector3 positon;
    public static GameObject[] Target;
    private void Start()
    {
        StartCoroutine(LoadPrefabs());
    }

    public void Update()
    {
        positon = transform.position;
        positon.x = Random.Range(4.27f, -1.3f);
        positon.y = Random.Range(4.27f, -1.3f);
        positon.z = Random.Range(4.27f, -10f);
        
        Target = GameObject.FindGameObjectsWithTag("input");
        //Debug.Log(target.Length);
    }

    private IEnumerator LoadPrefabs()
    {
        while (true)
        {
            // 加载所有预制体文件
            Object[] prefabFiles = Resources.LoadAll(prefabFolderPath, typeof(GameObject));

            // 遍历加载的预制体文件并进行初始化
            foreach (Object prefabFile in prefabFiles)
            {
                if (!IsAlreadyInstantiated(prefabFile))
                {
                    GameObject loadedPrefab = Instantiate(prefabFile as GameObject,positon,Quaternion.identity);
                    SetGameObjectTag(loadedPrefab , "input");
                    instantiatedPrefabs.Add(prefabFile, loadedPrefab);
                    // 在这里可以对预制体进行其他逻辑操作
                }
            }

            // 等待一段时间再继续加载新预制体
            yield return new WaitForSeconds(loadInterval);
        }
    }

    private bool IsAlreadyInstantiated(Object prefab)
    {
        return instantiatedPrefabs.ContainsKey(prefab);
    }
    
    public static void SetGameObjectTag(GameObject gameObject, string tag)
    {
        if (!UnityEditorInternal.InternalEditorUtility.tags.Equals(tag)) //如果tag列表中没有这个tag
        {
            UnityEditorInternal.InternalEditorUtility.AddTag(tag); //在tag列表中添加这个tag
        }
    
        gameObject.tag = tag;
        
    }
}

