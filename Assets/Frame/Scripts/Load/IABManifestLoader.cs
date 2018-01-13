/***
 * 
 *    Title: "" XXX项目
 *           主题： 
 *    Description: 
 *           功能：
 *                  
 *    Date: 2017
 *    Version: 0.1版本
 *    Modify Recoder: 
 *    
 *   
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IABManifestLoader
{
    private static IABManifestLoader _Instance;
 

    public static IABManifestLoader Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new IABManifestLoader();
            }
            return _Instance;
        }
    }
    private bool _IsLoadFinish;

    public AssetBundleManifest Manifest;
    public string ManifestPath;
    public AssetBundle ManifestLoader;


    public IABManifestLoader()
    {
        Manifest = null;
        ManifestLoader = null;
        _IsLoadFinish = false;
        ManifestPath = IPathTools.GetWWWAssetBundlePath()+"/"+IPathTools.GetPlatformFolderName(Application.platform);
    }

    public void SetManifestPath(string temPath)
    {
        ManifestPath = temPath;
    }

    public bool IsLoadFinish()
    {
        return _IsLoadFinish;
    }

    public IEnumerator LoadManifest()
    {
        WWW manifest = new WWW(ManifestPath);
        yield return manifest;
        if (!string.IsNullOrEmpty(manifest.error))
        {
            Debug.Log(manifest.error);
        }
        else
        {
            if (manifest.progress>=1.0f)
            {
                ManifestLoader = manifest.assetBundle;
                Manifest = ManifestLoader.LoadAsset("AssstBundleManifest") as AssetBundleManifest;
                _IsLoadFinish = true;
            }
        }
    }

    public string[] GetAllDependence(string name)
    {
        return Manifest.GetAllDependencies(name);
    }

    public void UnLoadManifest()
    {
        ManifestLoader.Unload(true);
    }
}
