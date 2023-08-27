using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using UnityEngine.UI;

public class ColorReplacement : MonoBehaviour
{
    public Texture2D inputTexture; // 输入的 Texture2D 对象
    public RawImage outputImage; // 用于显示输出的 RawImage

    public Color32[] templateColors; // 模板颜色数组
    public Color32[] finalColors; // 最终颜色数组

    private Texture2D outputTexture; // 输出的 Texture2D 对象

    private void Start()
    {
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

        // 将 outputTexture 赋值给 RawImage
        outputImage.texture = outputTexture;
    }
    private void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DoColorReplacement(); 
        }
        
    }
    private void DoColorReplacement()
    {
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

        // 将 outputTexture 赋值给 RawImage
        outputImage.texture = outputTexture;
    }
        

    // 获取最接近颜色的索引
    private int GetClosestColorIndex(Color32 targetColor, Color32[] colorArray)
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
    private float CalculateColorDistance(Color32 color1, Color32 color2)
    {
        int dr = color1.r - color2.r;
        int dg = color1.g - color2.g;
        int db = color1.b - color2.b;
        return Mathf.Sqrt(dr * dr + dg * dg + db * db);
    }
}
