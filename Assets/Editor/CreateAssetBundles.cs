using UnityEditor;
using System.IO;
using UnityEngine;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "AssetBundle";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        AssetBundleBuild[] buildMap = new AssetBundleBuild[2];

        buildMap[0].assetBundleName = "enemybundle";

        string[] enemyAssets = new string[1];
        enemyAssets[0] = "Assets/Textures/char_enemy_alienShip.jpg";
        enemyAssets[1] = "Assets/Textures/char_enemy_alienShip-damaged.jpg";

        buildMap[0].assetNames = enemyAssets;
        //buildMap[1].assetBundleName = "herobundle";

        //string[] heroAssets = new string[1];
        //heroAssets[0] = "char_hero_beanMan";
        //buildMap[1].assetNames = heroAssets;

        BuildPipeline.BuildAssetBundles(assetBundleDirectory, buildMap , BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}
