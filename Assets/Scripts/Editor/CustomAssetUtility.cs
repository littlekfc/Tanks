using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// A class contains the utilities for manipulating custom assets in a convenient way.
/// </summary>
public static class CustomAssetUtility
{
    /// <summary>
    /// Create an asset instance of a given type.
    /// </summary>
    /// <typeparam name="T">The type of asset to create</typeparam>
    public static void CreateAsset<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}