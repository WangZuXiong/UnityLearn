using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TextureImport : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        if (assetPath.Contains("Assets"))
        {
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            TextureImporterPlatformSettings settings = new TextureImporterPlatformSettings
            {
                overridden = true,
                name = "iPhone",
                format = TextureImporterFormat.ASTC_RGBA_4x4
            };
            importer.SetPlatformTextureSettings(settings);
            Debug.LogError(assetPath);
        }
    }
}
