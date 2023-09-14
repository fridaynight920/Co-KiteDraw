using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ImageToModelConverter : MonoBehaviour
{
    public string imageFolderPath; // 图片文件夹路径
    public string modelFolderPath; // 模型预制体文件夹路径
    public GameObject modelPrefab; // 模型
    public float checkInterval = 1f; // 监测间隔时间，单位为秒

    private Dictionary<string, GameObject> modelDictionary;
    private DirectoryInfo dirInfo;

    void Start()
    {
        modelDictionary = new Dictionary<string, GameObject>();
        StartCoroutine(CheckForNewImages());
    }

    IEnumerator CheckForNewImages()
    {
        while (true)
        {
            if (!Directory.Exists(imageFolderPath))
            {
                Debug.LogWarning("Image folder path is not exist: " + imageFolderPath);
                yield return new WaitForSeconds(checkInterval);
                continue;
            }

            if (dirInfo == null)
            {
                dirInfo = new DirectoryInfo(imageFolderPath);
            }
            FileInfo[] imageFiles = dirInfo.GetFiles("*.png", SearchOption.TopDirectoryOnly);

            foreach (FileInfo imageFile in imageFiles)
            {
                string modelName = Path.GetFileNameWithoutExtension(imageFile.Name) + ".prefab";

                // 检查是否为已有预制体，避免重复加载
                if (!modelDictionary.ContainsKey(modelName))
                {
                    // 创建材质
                    Material material = new Material(Shader.Find("Unlit/Texture"));

                    // 加载纹理
                    Texture2D texture = new Texture2D(2048, 2048);
                    byte[] imageBytes = File.ReadAllBytes(imageFolderPath);
                    texture.LoadImage(imageBytes);

                    // 将纹理赋值给材质
                    material.mainTexture = texture;

                    // 创建预制体
                    GameObject prefab = PrefabUtility.SaveAsPrefabAsset(modelPrefab, modelFolderPath);
                    Renderer prefabRenderer = prefab.GetComponent<Renderer>();

                    // 更新预制体的材质
                    prefabRenderer.sharedMaterial = material;

                    // 添加到字典中
                    modelDictionary.Add(modelName, modelPrefab);

                    Debug.Log("Converted image to model: " + modelName);
                }
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }
   
    
}



