using UnityEditor;

public class TextureImporterSettings : AssetPostprocessor
{
    private void OnPreprocessTexture()
    {
        TextureImporter textureImporter = (TextureImporter)assetImporter;

        // 在这里设置其他的纹理导入设置，如果需要的话
        // textureImporter.filterMode = FilterMode.Bilinear;

        // 启用读写使能
        textureImporter.isReadable = true;
    }
}