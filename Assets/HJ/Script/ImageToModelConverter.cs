using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ImageToModelConverter : MonoBehaviour
{
    public string imageFolderPath; // ͼƬ�ļ���·��
    public string modelFolderPath; // ģ��Ԥ�����ļ���·��
    public GameObject modelPrefab; // ģ��
    public float checkInterval = 1f; // �����ʱ�䣬��λΪ��

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

                // ����Ƿ�Ϊ����Ԥ���壬�����ظ�����
                if (!modelDictionary.ContainsKey(modelName))
                {
                    // ��������
                    Material material = new Material(Shader.Find("Unlit/Texture"));

                    // ��������
                    Texture2D texture = new Texture2D(2048, 2048);
                    byte[] imageBytes = File.ReadAllBytes(imageFolderPath);
                    texture.LoadImage(imageBytes);

                    // ������ֵ������
                    material.mainTexture = texture;

                    // ����Ԥ����
                    GameObject prefab = PrefabUtility.SaveAsPrefabAsset(modelPrefab, modelFolderPath);
                    Renderer prefabRenderer = prefab.GetComponent<Renderer>();

                    // ����Ԥ����Ĳ���
                    prefabRenderer.sharedMaterial = material;

                    // ��ӵ��ֵ���
                    modelDictionary.Add(modelName, modelPrefab);

                    Debug.Log("Converted image to model: " + modelName);
                }
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }
   
    
}



