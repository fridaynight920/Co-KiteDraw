using UnityEngine;
using UnityEditor;
using System.IO;

public class ImageToMaterialAndPrefabConverter : MonoBehaviour
{
    [SerializeField] private string inputFolderPath; // ����ͼƬ�ļ���·��
    [SerializeField] private string outputFolderPath; // ���Ԥ�����ļ���·��
    [SerializeField] private GameObject fbxModel; // FBXģ��

    void Update()
    {
        // ��ȡ�����ļ����µ�����ͼƬ�ļ�
        string[] imageFilePaths = Directory.GetFiles(inputFolderPath, "*.png");

        foreach (string imagePath in imageFilePaths)
        {
            // ��ȡͼƬ�ļ�����������չ����
            string imageName = Path.GetFileNameWithoutExtension(imagePath);

            // ���Ԥ�����ļ������Ƿ��Ѿ�����ͬ����Ԥ����
            string prefabPath = Path.Combine(outputFolderPath, imageName + ".prefab");
            if (!File.Exists(prefabPath))
            {
                // ��������
                Material material = new Material(Shader.Find("Unlit/Texture"));

                // ��������
                Texture2D texture = new Texture2D(2048, 2048);
                byte[] imageBytes = File.ReadAllBytes(imagePath);
                texture.LoadImage(imageBytes);

                // ������ֵ������
                material.mainTexture = texture;

                // ����Ԥ����
                GameObject prefab = PrefabUtility.SaveAsPrefabAsset(fbxModel, prefabPath);
                Renderer prefabRenderer = prefab.GetComponent<Renderer>();

                // ����Ԥ����Ĳ���
                prefabRenderer.sharedMaterial = material;

                Debug.Log("New prefab created: " + imageName);
            }
        }

        // ˢ��AssetDatabase��ȷ���´�����Ԥ������Unity�༭���пɼ�
        AssetDatabase.Refresh();
    }
}


