using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BXB.Core;
using UnityEditor;

public class ProjectManager : MiSingleton<ProjectManager>
{
    public enum AssetTypes
    {
        SystemStringAsset,

    }
    Dictionary<AssetTypes, string> assetPath = new Dictionary<AssetTypes, string>
    {
        { AssetTypes.SystemStringAsset, "Assets/Resources/SettingAsset/"},

    };
    public bool TryGetSettingAssets<T>(AssetTypes type, out T asset) where T : ScriptableObject
    {
        asset = null;
        bool isGet = false;
        try
        {
            var path = assetPath[type] + $"{type}.asset";
            asset = AssetDatabase.LoadAssetAtPath<T>(path);
        }
        catch (Exception exp)
        {

        }
        return isGet;
    }
}
