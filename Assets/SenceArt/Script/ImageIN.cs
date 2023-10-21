using UnityEditor;
using System;
using System.IO;
using UnityEngine;
using OpenCVForUnity.UnityUtils;


namespace OpenCVForUnityExample
{
    public class ImagIN : MonoBehaviour
    {
       
        public int ImgWidth;  // 图片宽度
        public int ImgHeight;  // 图片高度
        public string filePath;  // 文件路径
        public string filePathOut;  // 输出文件夹路径
        private Texture2D imgTexture;  // 图片纹理
        private int imageCount = 0;  // 新图片数量

        // Start is called before the first frame update

        public string folderPath; // 指定文件夹路径


        void Start()
        {
            Utils.setDebugMode(true);

            //// 设置文件路径为相对路径
            //filePath = Path.Combine(Application.dataPath, "../Assets/ColorSet/Img");
            //filePathOut = Path.Combine(Application.dataPath, "../Assets/ColorSet/ImgOut");  // 新的输出文件夹路径
        }

        // Update is called once per frame
        void Update()
        {
           
                FindImage();
                HandleImage();

            // 更新 AssetDatabase
            AssetDatabase.Refresh();

           




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

                    // 获取新的文件名（避免重复）
                    string newImagePath = Path.Combine(filePath, "Img_" + DateTime.Now.Ticks + ".png");

                    // 重命名文件
                    File.Move(imagePath, newImagePath);
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
                Debug.Log("新图片为1个");

                foreach (string imagePath in imageFiles)
                {
                    string imageName = Path.GetFileNameWithoutExtension(imagePath);
                    if (imageName.StartsWith("Img_"))
                    {
                        Texture2D imgTexture = LoadTexture(imagePath);
                        if (imgTexture != null)
                        {
                            Texture2D resizedTexture = ResizeTexture(imgTexture, ImgWidth, ImgHeight);
                            string newImagePath = Path.Combine(filePathOut, "Img_" + imgNum + ".png");  // 使用新的输出路径
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



        //////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// 


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
}
