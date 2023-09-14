using UnityEngine;
using UnityEditor;
using System.IO;

public class ImageToMaterialAndPrefabConverter1 : MonoBehaviour
{
    [SerializeField] private string inputFolderPath2; // 输入图片文件夹路径
    [SerializeField] private string outputFolderPath2; // 输出预制体文件夹路径
    [SerializeField] private GameObject fbxModel; // FBX模型
    public float refreshInterval = 60f; // 刷新间隔时间

    void Update()
    {
        InvokeRepeating("Refresh", 60f, refreshInterval);
        // 刷新AssetDatabase，确保新创建的预制体在Unity编辑器中可见
        
    }

    void Refresh()
    {
        // 获取输入文件夹下的所有图片文件
        string[] imageFilePaths = Directory.GetFiles(inputFolderPath2, "*.png");

        foreach (string imagePath in imageFilePaths)
        {
            // 获取图片文件名（不带扩展名）
            string imageName = Path.GetFileNameWithoutExtension(imagePath);

            // 检查预制体文件夹下是否已经存在同名的预制体
            string prefabPath = Path.Combine(outputFolderPath2, imageName + ".prefab");
            if (!File.Exists(prefabPath))
            {
                // 创建材质
                Material mat = new Material(Shader.Find("Unlit/Texture"));

                // 加载纹理
                Texture2D texture = new Texture2D(2048, 2048);
                byte[] imageBytes = File.ReadAllBytes(imagePath);
                texture.LoadImage(imageBytes);

                // 将纹理赋值给材质
                mat.mainTexture = texture;

                // 创建预制体
                GameObject prefab = PrefabUtility.SaveAsPrefabAsset(fbxModel, prefabPath);
                Renderer prefabRenderer = prefab.GetComponent<MeshRenderer>();

                // 更新预制体的材质
                prefabRenderer.sharedMaterial = mat;

                Debug.Log("New prefab created: " + imageName);
            }
            AssetDatabase.Refresh();

        }

        
    }
}


