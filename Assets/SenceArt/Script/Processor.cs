using UnityEditor;
using System.IO;
using UnityEngine;

using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;

public class Processor : MonoBehaviour
{
    public string imageFolderPath; // ͼƬ�ļ���·��
    public string processedFolderPath; // �����ͼƬ������ļ���·��
   public float checkInterval = 60f; // �����ʱ�䣬��λΪ��

    private DirectoryInfo dirInfo;


    public string folderPath; // ָ���ļ���·��


    public Texture2D inputTexture; // ����� Texture2D ����
   // public RawImage outputImage; // ������ʾ����� RawImage

    public Color32[] templateColors; // ģ����ɫ����
    public Color32[] finalColors; // ������ɫ����

    private Texture2D outputTexture; // ����� Texture2D ����

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

            // ����Ѿ���������ļ���������
            if (File.Exists(processedFilePath))
                continue;

            // ���غʹ�������
            Texture2D processedTexture = ProcessImage(imageFile.FullName);

            // ���洦����ͼƬ
            byte[] bytes = processedTexture.EncodeToPNG();
            File.WriteAllBytes(processedFilePath, bytes);

            Debug.Log("Processed and saved image: " + processedFilePath);
            // ���� AssetDatabase
            AssetDatabase.Refresh();

            // ���Ĵ����߼�
            // ...
        }
    }

    Texture2D ProcessImage(string imagePath)
    {
        // TODO: ͼƬ�������
       

      
       
            inputTexture = LoadTexture(imagePath); // ������ʾ����ֱ�Ӷ���ͼƬ�ļ�
            // ����һ���� inputTexture ��С��ͬ�� Mat ����
            Mat inputMat = new Mat(inputTexture.height, inputTexture.width, CvType.CV_8UC4);
            Utils.texture2DToMat(inputTexture, inputMat);

            Mat outputMat = new Mat(inputMat.rows(), inputMat.cols(), CvType.CV_8UC4);

            // ����Ĵ������...
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

            // �� outputMat ת��Ϊ Texture2D
            outputTexture = new Texture2D(outputMat.cols(), outputMat.rows(), TextureFormat.RGBA32, false);
            Utils.matToTexture2D(outputMat, outputTexture);


            inputTexture = outputTexture;
            

            
        


        // ��ȡ��ӽ���ɫ������
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

        // ������ɫ֮��ľ���
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
    //ֹͣ��������
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

