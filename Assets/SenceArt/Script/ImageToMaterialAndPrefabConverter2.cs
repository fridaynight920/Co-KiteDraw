using UnityEngine;
using UnityEditor;
using System.IO;

public class ImageToMaterialAndPrefabConverter2 : MonoBehaviour
{
    //[serializefield] public string inputfolderpath; // ����ͼƬ�ļ���·��
    //[serializefield] public string outputfolderpath; // ���Ԥ�����ļ���·��
    //[serializefield] private gameobject fbxmodel; // fbxģ��
    //[serializefield] private float refreshinterval = 1f; // ˢ�¼��ʱ�䣨����Ϊ��λ��

    //private void Start()
    //{
    //    // ����Э��
    //    StartCoroutine(ConvertImages());
    //}

    //private System.Collections.IEnumerator ConvertImages()
    //{
    //    while (true)
    //    {
    //        // ��ȡ�����ļ����µ�����ͼƬ�ļ�
    //        string[] imageFilePaths = Directory.GetFiles(inputFolderPath, "*.png");
    //        if (imageFilePaths == null || imageFilePaths.Length == 0)
    //        {
    //            // ���б�Ϊ��ʱ��ִ���κ���Ϊ
    //            yield return null;
    //            continue;
    //        }
    //        else
    //        {
    //            foreach (string imagePath in imageFilePaths)
    //            {
    //                // ��ȡͼƬ�ļ�����������չ����
    //                string imageName = Path.GetFileNameWithoutExtension(imagePath);

    //                // ���Ԥ�����ļ������Ƿ��Ѿ�����ͬ����Ԥ����
    //                string prefabPath = Path.Combine(outputFolderPath, imageName + ".prefab");
    //                if (!File.Exists(prefabPath))
    //                {
    //                    // ��������
    //                    Material material = new Material(Shader.Find("Unlit/Texture"));

    //                    // ��������
    //                    Texture2D texture = new Texture2D(2048, 2048);
    //                    byte[] imageBytes = File.ReadAllBytes(imagePath);
    //                    texture.LoadImage(imageBytes);

    //                    // ������ֵ������
    //                    material.mainTexture = texture;

    //                    // ����Ԥ����
    //                    GameObject prefab = PrefabUtility.SaveAsPrefabAsset(fbxModel, prefabPath);
    //                    Renderer prefabRenderer = prefab.GetComponent<Renderer>();

    //                    // ����Ԥ����Ĳ���
    //                    prefabRenderer.sharedMaterial = material;

    //                    Debug.Log("New prefab created: " + imageName);
    //                }
    //            }
    //        }

    //        // ˢ��AssetDatabase��ȷ���´�����Ԥ������Unity�༭���пɼ�
    //        AssetDatabase.Refresh();

    //        // �ȴ�ָ����ˢ�¼��ʱ��
    //        yield return new WaitForSeconds(refreshInterval);
    //    }
    //}



    [SerializeField] public string inputFolderPath; // ����ͼƬ�ļ���·��
    [SerializeField] public string outputFolderPath; // ���Ԥ�����ļ���·��
    [SerializeField] private GameObject fbxModel; // FBXģ��

    private void Update()
    {
        // ��ȡ�����ļ����µ�����ͼƬ�ļ�
        string[] imageFilePaths = Directory.GetFiles(inputFolderPath, "*.png");
        if (imageFilePaths == null || imageFilePaths.Length == 0)
        {
            return; // ���б�Ϊ��ʱ��ִ���κ���Ϊ
        }

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
