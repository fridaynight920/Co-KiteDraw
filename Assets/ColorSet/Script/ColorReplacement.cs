using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using UnityEngine.UI;

public class ColorReplacement : MonoBehaviour
{
    public Texture2D inputTexture;
    public RawImage outputImage;

    public Color32[] templateColors;
    public Color32[] finalColors;

    private Texture2D outputTexture;

    private void Start()
    {
        Mat inputMat = new Mat();
        Utils.texture2DToMat(inputTexture, inputMat);

        Mat outputMat = new Mat(inputMat.rows(), inputMat.cols(), CvType.CV_8UC4);

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

        outputTexture = new Texture2D(outputMat.cols(), outputMat.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(outputMat, outputTexture);

        outputImage.texture = outputTexture;
    }

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

    private float CalculateColorDistance(Color32 color1, Color32 color2)
    {
        int dr = color1.r - color2.r;
        int dg = color1.g - color2.g;
        int db = color1.b - color2.b;
        return Mathf.Sqrt(dr * dr + dg * dg + db * db);
    }
}
