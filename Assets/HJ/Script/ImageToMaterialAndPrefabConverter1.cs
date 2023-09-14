using UnityEngine;
using UnityEditor;
using System.IO;

public class ImageToMaterialAndPrefabConverter1 : MonoBehaviour
{
    [SerializeField] private string inputFolderPath2; // ����ͼƬ�ļ���·��
    [SerializeField] private string outputFolderPath2; // ���Ԥ�����ļ���·��
    [SerializeField] private GameObject fbxModel; // FBXģ��
    public float refreshInterval = 60f; // ˢ�¼��ʱ��

    void Update()
    {
        InvokeRepeating("Refresh", 60f, refreshInterval);
        // ˢ��AssetDatabase��ȷ���´�����Ԥ������Unity�༭���пɼ�
        
    }

    void Refresh()
    {
        // ��ȡ�����ļ����µ�����ͼƬ�ļ�
        string[] imageFilePaths = Directory.GetFiles(inputFolderPath2, "*.png");

        foreach (string imagePath in imageFilePaths)
        {
            // ��ȡͼƬ�ļ�����������չ����
            string imageName = Path.GetFileNameWithoutExtension(imagePath);

            // ���Ԥ�����ļ������Ƿ��Ѿ�����ͬ����Ԥ����
            string prefabPath = Path.Combine(outputFolderPath2, imageName + ".prefab");
            if (!File.Exists(prefabPath))
            {
                // ��������
                Material mat = new Material(Shader.Find("Unlit/Texture"));

                // ��������
                Texture2D texture = new Texture2D(2048, 2048);
                byte[] imageBytes = File.ReadAllBytes(imagePath);
                texture.LoadImage(imageBytes);

                // ������ֵ������
                mat.mainTexture = texture;

                // ����Ԥ����
                GameObject prefab = PrefabUtility.SaveAsPrefabAsset(fbxModel, prefabPath);
                Renderer prefabRenderer = prefab.GetComponent<MeshRenderer>();

                // ����Ԥ����Ĳ���
                prefabRenderer.sharedMaterial = mat;

                Debug.Log("New prefab created: " + imageName);
            }
            AssetDatabase.Refresh();

        }

        
    }
}


