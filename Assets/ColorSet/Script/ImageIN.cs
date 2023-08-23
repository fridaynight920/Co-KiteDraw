using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;

namespace OpenCVForUnityExample
{
    public class ImagIN : MonoBehaviour
    {
        public int ImgWidth;  // 图片宽度
        public int ImgHeight;  // 图片高度
        private string filePath = "Assets/ColorSet/Img";  // 文件路径
        private Texture2D imgTexture;  // 图片纹理
        private int imageCount = 0;  // 新图片数量

        // Start is called before the first frame update
        void Start()
        {
            Utils.setDebugMode(true);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                FindImage();
                HandleImage();
            }
        }

        // 查找图片
        void FindImage()
        {
            string[] imageFiles = Directory.GetFiles(filePath, "*.png");
            int newImageCount = 0;

            foreach (string imagePath in imageFiles)
            {
                string imageName = Path.GetFileNameWithoutExtension(imagePath);
                if (!imageName.StartsWith("Img_"))
                {
                    newImageCount++;
                    File.Delete(imagePath);
                }
            }

            if (newImageCount > 1)
            {
                Debug.Log("新图片多余1个");
            }

            imageCount = newImageCount;
        }

        // 处理图片
        void HandleImage()
        {
            if (imageCount == 1)
            {
                string[] imageFiles = Directory.GetFiles(filePath, "*.png");
                int imgNum = 0;

                foreach (string imagePath in imageFiles)
                {
                    string imageName = Path.GetFileNameWithoutExtension(imagePath);
                    if (imageName.StartsWith("Img_"))
                    {
                        Texture2D imgTexture = LoadTexture(imagePath);
                        if (imgTexture != null)
                        {
                            Texture2D resizedTexture = ResizeTexture(imgTexture, ImgWidth, ImgHeight);
                            string newImagePath = Path.Combine(filePath, "Img_" + imgNum + ".png");
                            SaveTextureToFile(resizedTexture, newImagePath);
                            imgNum++;
                        }
                    }
                }
            }
        }

        // 加载纹理
        Texture2D LoadTexture(string imagePath)
        {
            byte[] imageData = File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(1, 1);
            if (texture.LoadImage(imageData))
            {
                return texture;
            }
            return null;
        }

        // 缩放纹理
        Texture2D ResizeTexture(Texture2D inputTexture, int width, int height)
        {
            Texture2D outputTexture = new Texture2D(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color newColor = inputTexture.GetPixelBilinear((float)x / width, (float)y / height);
                    outputTexture.SetPixel(x, y, newColor);
                }
            }
            outputTexture.Apply();
            return outputTexture;
        }

        // 保存纹理到文件
        void SaveTextureToFile(Texture2D texture, string filePath)
        {
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);
        }
    }
}
