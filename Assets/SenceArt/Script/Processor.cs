using UnityEditor;
using System.IO;
using UnityEngine;

using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;

public class Processor : MonoBehaviour
{
    public string imageFolderPath; // 图片文件夹路径
    public string processedFolderPath; // 处理后图片保存的文件夹路径
   public float checkInterval = 60f; // 监测间隔时间，单位为秒

    private DirectoryInfo dirInfo;


    public string folderPath; // 指定文件夹路径


    public Texture2D inputTexture; // 输入的 Texture2D 对象
   // public RawImage outputImage; // 用于显示输出的 RawImage

    public Color32[] templateColors; // 模板颜色数组
    public Color32[] finalColors; // 最终颜色数组

    private Texture2D outputTexture; // 输出的 Texture2D 对象

    //void Start()
    //{
    //    StartCoroutine(CheckForNewImages());
    //}


    //IEnumerator CheckForNewImages()

    //{
    //    while (true)
    //    {
    //        if (!Directory.Exists(imageFolderPath))
    //        {
    //            Debug.LogWarning("Image folder path is not exist: " + imageFolderPath);
    //            yield return new WaitForSeconds(checkInterval);
    //            continue;
    //        }

    //        if (dirInfo == null)
    //        {
    //            dirInfo = new DirectoryInfo(imageFolderPath);
    //        }

    //        FileInfo[] imageFiles = dirInfo.GetFiles("*.png", SearchOption.TopDirectoryOnly);

    //        foreach (FileInfo imageFile in imageFiles)
    //        {
    //            string processedFilePath = Path.Combine(processedFolderPath, imageFile.Name);


    //            Texture2D processedTexture = ProcessImage(imageFile.FullName);


    //            byte[] bytes = processedTexture.EncodeToPNG();
    //            File.WriteAllBytes(processedFilePath, bytes);

    //            Debug.Log("Processed and saved image: " + processedFilePath);
    //        }

    //        yield return new WaitForSeconds(checkInterval);
    //    }
    //}
    void Start()
    {
        if (!Directory.Exists(processedFolderPath))
        {
            Directory.CreateDirectory(processedFolderPath);
        }

        dirInfo = new DirectoryInfo(imageFolderPath);
    }

    void Update()
    {
        if (!Directory.Exists(imageFolderPath))
        {
            Debug.LogWarning("Image folder path is not exist: " + imageFolderPath);
            return;
        }

        if (dirInfo == null)
        {
            dirInfo = new DirectoryInfo(imageFolderPath);
        }

        FileInfo[] imageFiles = dirInfo.GetFiles("*.png", SearchOption.TopDirectoryOnly);

        foreach (FileInfo imageFile in imageFiles)
        {
            string processedFilePath = Path.Combine(processedFolderPath, imageFile.Name);

            // 如果已经保存过该文件，则跳过
            if (File.Exists(processedFilePath))
                continue;

            // 加载和处理纹理
            Texture2D processedTexture = ProcessImage(imageFile.FullName);

            // 保存处理后的图片
            byte[] bytes = processedTexture.EncodeToPNG();
            File.WriteAllBytes(processedFilePath, bytes);

            Debug.Log("Processed and saved image: " + processedFilePath);
            // 更新 AssetDatabase
            AssetDatabase.Refresh();

            // 您的代码逻辑
            // ...
        }
    }

    Texture2D ProcessImage(string imagePath)
    {
        // TODO: 图片处理操作
       

      
       
            inputTexture = LoadTexture(imagePath); // 这里是示例，直接读入图片文件
            // 创建一个与 inputTexture 大小相同的 Mat 对象
            Mat inputMat = new Mat(inputTexture.height, inputTexture.width, CvType.CV_8UC4);
            Utils.texture2DToMat(inputTexture, inputMat);

            Mat outputMat = new Mat(inputMat.rows(), inputMat.cols(), CvType.CV_8UC4);

            // 其余的处理代码...
            for (int y = 0; y < inputMat.rows(); y++)
            {
                for (int x = 0; x < inputMat.cols(); x++)
                {
                    Color32 originalColor = new Color32(
                        (byte)inputMat.get(y, x)[0],
                        (byte)inputMat.get(y, x)[1],
                        (byte)inputMat.get(y, x)[2],
                        (byte)inputMat.get(y, x)[3]
                    );

                    int closestColorIndex = GetClosestColorIndex(originalColor, templateColors);

                    Color32 replacedColor = finalColors[closestColorIndex];
                    Color32 outputColor = new Color32(replacedColor.r, replacedColor.g, replacedColor.b, 255);

                    double[] outputScalar = new double[] { outputColor.r, outputColor.g, outputColor.b, 255 };
                    outputMat.put(y, x, outputScalar);
                }
            }

            // 将 outputMat 转换为 Texture2D
            outputTexture = new Texture2D(outputMat.cols(), outputMat.rows(), TextureFormat.RGBA32, false);
            Utils.matToTexture2D(outputMat, outputTexture);


            inputTexture = outputTexture;
            

            
        


        // 获取最接近颜色的索引
         int GetClosestColorIndex(Color32 targetColor, Color32[] colorArray)
        {
            float minDistance = float.MaxValue;
            int closestIndex = -1;

            for (int i = 0; i < colorArray.Length; i++)
            {
                float distance = CalculateColorDistance(targetColor, colorArray[i]);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestIndex = i;
                }
            }

            return closestIndex;
        }

        // 计算颜色之间的距离
        float CalculateColorDistance(Color32 color1, Color32 color2)
        {
            int dr = color1.r - color2.r;
            int dg = color1.g - color2.g;
            int db = color1.b - color2.b;
            return Mathf.Sqrt(dr * dr + dg * dg + db * db);
        }
        return inputTexture;
    }

    Texture2D LoadTexture(string imagePath)
    {
        byte[] imageBytes = File.ReadAllBytes(imagePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageBytes);
        return texture;
    }

    /// <summary>
    //停止运行销毁
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

