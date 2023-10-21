using UnityEngine;
using UnityEditor;
using System.IO;

public class ImageToMaterialAndPrefabConverter2 : MonoBehaviour
{
    //[serializefield] public string inputfolderpath; // 输入图片文件夹路径
    //[serializefield] public string outputfolderpath; // 输出预制体文件夹路径
    //[serializefield] private gameobject fbxmodel; // fbx模型
    //[serializefield] private float refreshinterval = 1f; // 刷新间隔时间（以秒为单位）

    //private void Start()
    //{
    //    // 启动协程
    //    StartCoroutine(ConvertImages());
    //}

    //private System.Collections.IEnumerator ConvertImages()
    //{
    //    while (true)
    //    {
    //        // 获取输入文件夹下的所有图片文件
    //        string[] imageFilePaths = Directory.GetFiles(inputFolderPath, "*.png");
    //        if (imageFilePaths == null || imageFilePaths.Length == 0)
    //        {
    //            // 当列表为空时不执行任何行为
    //            yield return null;
    //            continue;
    //        }
    //        else
    //        {
    //            foreach (string imagePath in imageFilePaths)
    //            {
    //                // 获取图片文件名（不带扩展名）
    //                string imageName = Path.GetFileNameWithoutExtension(imagePath);

    //                // 检查预制体文件夹下是否已经存在同名的预制体
    //                string prefabPath = Path.Combine(outputFolderPath, imageName + ".prefab");
    //                if (!File.Exists(prefabPath))
    //                {
    //                    // 创建材质
    //                    Material material = new Material(Shader.Find("Unlit/Texture"));

    //                    // 加载纹理
    //                    Texture2D texture = new Texture2D(2048, 2048);
    //                    byte[] imageBytes = File.ReadAllBytes(imagePath);
    //                    texture.LoadImage(imageBytes);

    //                    // 将纹理赋值给材质
    //                    material.mainTexture = texture;

    //                    // 创建预制体
    //                    GameObject prefab = PrefabUtility.SaveAsPrefabAsset(fbxModel, prefabPath);
    //                    Renderer prefabRenderer = prefab.GetComponent<Renderer>();

    //                    // 更新预制体的材质
    //                    prefabRenderer.sharedMaterial = material;

    //                    Debug.Log("New prefab created: " + imageName);
    //                }
    //            }
    //        }

    //        // 刷新AssetDatabase，确保新创建的预制体在Unity编辑器中可见
    //        AssetDatabase.Refresh();

    //        // 等待指定的刷新间隔时间
    //        yield return new WaitForSeconds(refreshInterval);
    //    }
    //}



    [SerializeField] public string inputFolderPath; // 输入图片文件夹路径
    [SerializeField] public string outputFolderPath; // 输出预制体文件夹路径
    [SerializeField] private GameObject fbxModel; // FBX模型

    private void Update()
    {
        // 获取输入文件夹下的所有图片文件
        string[] imageFilePaths = Directory.GetFiles(inputFolderPath, "*.png");
        if (imageFilePaths == null || imageFilePaths.Length == 0)
        {
            return; // 当列表为空时不执行任何行为
        }

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

                // 更新预制体的材质
                prefabRenderer.sharedMaterial = material;

                Debug.Log("New prefab created: " + imageName);
            }
        }

        // 刷新AssetDatabase，确保新创建的预制体在Unity编辑器中可见
        AssetDatabase.Refresh();
    }

}
