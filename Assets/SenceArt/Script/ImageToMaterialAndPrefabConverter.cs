using UnityEngine;
using UnityEditor;
using System.IO;

public class ImageToMaterialAndPrefabConverter : MonoBehaviour
{
    [SerializeField] private string inputFolderPath; // 输入图片文件夹路径
    [SerializeField] private string outputFolderPath; // 输出预制体文件夹路径
    [SerializeField] private GameObject fbxModel; // FBX模型


    public string folderPath; // 指定文件夹路径

    void Update()
    {
        // 获取输入文件夹下的所有图片文件
        string[] imageFilePaths = Directory.GetFiles(inputFolderPath, "*.png");
        if (imageFilePaths == null || imageFilePaths.Length == 0)
        {
            // 当列表为空时不执行任何行为
            return;
        }
        else
        {
            foreach (string imagePath in imageFilePaths)
            {
                // 获取图片文件名（不带扩展名）
                string imageName = Path.GetFileNameWithoutExtension(imagePath);

                // 检查预制体文件夹下是否已经存在同名的预制体
                string prefabPath = Path.Combine(outputFolderPath, imageName + ".prefab");
                if (!File.Exists(prefabPath))
                {
                    // 创建材质
                    Material material = new Material(Shader.Find("Unlit/Texture"));

                    // 加载纹理
                    Texture2D texture = new Texture2D(2048, 2048);
                    byte[] imageBytes = File.ReadAllBytes(imagePath);
                    texture.LoadImage(imageBytes);

                    // 将纹理赋值给材质
                    material.mainTexture = texture;

                    // 创建预制体
                    GameObject prefab = PrefabUtility.SaveAsPrefabAsset(fbxModel, prefabPath);
                    Renderer prefabRenderer = prefab.GetComponent<Renderer>();

                    //缩放
                    Vector3 ini = prefab.transform.localScale;
                    Vector3 news = ini * 3;
                    prefab.transform.localScale = news;

                    // 更新预制体的材质
                    prefabRenderer.sharedMaterial = material;

                    
                    Debug.Log("New prefab created: " + imageName);

                    



                }
            }
        }

        // 刷新AssetDatabase，确保新创建的预制体在Unity编辑器中可见
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

            Debug.Log("文件夹下的所有文件已销毁：" + folderPath);
        }
        else
        {
            Debug.LogWarning("文件夹不存在：" + folderPath);
        }
    }
}


