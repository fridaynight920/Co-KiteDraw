using UnityEngine;
using UnityEditor;
using System.IO;

public class ImageToMaterialAndPrefabConverter : MonoBehaviour
{
    [SerializeField] private string inputFolderPath; // ����ͼƬ�ļ���·��
    [SerializeField] private string outputFolderPath; // ���Ԥ�����ļ���·��
    [SerializeField] private GameObject fbxModel; // FBXģ��


    public string folderPath; // ָ���ļ���·��

    void Update()
    {
        // ��ȡ�����ļ����µ�����ͼƬ�ļ�
        string[] imageFilePaths = Directory.GetFiles(inputFolderPath, "*.png");
        if (imageFilePaths == null || imageFilePaths.Length == 0)
        {
            // ���б�Ϊ��ʱ��ִ���κ���Ϊ
            return;
        }
        else
        {
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

                    //����
                    Vector3 ini = prefab.transform.localScale;
                    Vector3 news = ini * 3;
                    prefab.transform.localScale = news;

                    // ����Ԥ����Ĳ���
                    prefabRenderer.sharedMaterial = material;

                    
                    Debug.Log("New prefab created: " + imageName);

                    



                }
            }
        }

        // ˢ��AssetDatabase��ȷ���´�����Ԥ������Unity�༭���пɼ�
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// /////
    /// </summary>
    private void OnApplicationQuit()
    {
        DestroyFilesInFolder(folderPath);
    }

    private void DestroyFilesInFolder(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            DirectoryInfo directory = new DirectoryInfo(folderPath);
            FileInfo[] files = directory.GetFiles();

            foreach (var file in files)
            {
                file.Delete();

            }

            Debug.Log("�ļ����µ������ļ������٣�" + folderPath);
        }
        else
        {
            Debug.LogWarning("�ļ��в����ڣ�" + folderPath);
        }
    }
}


