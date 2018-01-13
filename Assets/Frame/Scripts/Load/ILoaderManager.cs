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

public class ILoaderManager :MonoBehaviour
{
    public static ILoaderManager _Instance;
    private IABScenesManager ScenesManager;
    private Dictionary<string, IABScenesManager> LoadManager = new Dictionary<string, IABScenesManager>();

    private void Awake()
    {
        _Instance = this;
        StartCoroutine(IABManifestLoader.Instance.LoadManifest());
    }

    public void ReadConfig(string sceneName)
    {
        if (!LoadManager.ContainsKey(sceneName))
        {
            IABScenesManager temManager = new IABScenesManager();
            temManager.ReadConfig(sceneName);
            LoadManager.Add(sceneName, temManager);
        }
    }

    public void LoadCallBack(string sceneName,string bundleName)
    {
        if (LoadManager.ContainsKey(sceneName))
        {
            IABScenesManager temManager = LoadManager[sceneName];
            StartCoroutine(temManager.LoadAssetAsync(bundleName));
        }
        else
        {
            Debug.LogError("scenesManager not Contain ======"+sceneName);
        }
    }


    public void LoadAsset(string sceneName,string bundleName,LoadProgress progress)
    {
        if (!LoadManager.ContainsKey(sceneName))
        {
            ReadConfig(sceneName);
        }
        else
        {
            IABScenesManager temManager = LoadManager[sceneName];
            temManager.LoadAsset(bundleName,progress, LoadCallBack);
        }
    }

    public string GetBundleName(string sceneName,string bundleName)
    {
        IABScenesManager manager = LoadManager[sceneName];
        if (manager != null)
        {
            return manager.GetBundleName(bundleName);
        }
        return null;
    }

    #region 由下层提供API

    public UnityEngine.Object GetSingleRes(string sceneName, string bundleName, string resName)
    {
        if (LoadManager.ContainsKey(sceneName))
        {
            IABScenesManager temManager = LoadManager[sceneName];
            return temManager.GetSingleRes(bundleName, resName);
        }
        else
        {
            Debug.Log("scene or bundle is not load or 未加载完 =========" + sceneName);
            return null;
        }
    }

    public UnityEngine.Object[] GetMutiRes(string sceneName, string bundleName, string resName)
    {
        if (LoadManager.ContainsKey(sceneName))
        {
            IABScenesManager temManager = LoadManager[sceneName];
            return temManager.GetMutiRes(bundleName, resName);
        }
        else
        {
            Debug.Log("scene or bundle is not load or 未加载完 =========" + sceneName);
            return null;
        }
    }

    public void UnloadResObj(string sceneName, string bundleName, string resName)
    {
        if (LoadManager.ContainsKey(sceneName))
        {
            IABScenesManager temManager = LoadManager[sceneName];
            temManager.DisposeObjRes(bundleName,resName);
        }
    }


    public void UnLoadBundleRes(string sceneName, string bundleName)
    {
        if (LoadManager.ContainsKey(sceneName))
        {
            IABScenesManager temManager = LoadManager[sceneName];
            temManager.DisposeBundleRes(bundleName);
        }
    }

    public void UnLoadAllRes(string sceneName)
    {
        if (LoadManager.ContainsKey(sceneName))
        {
            IABScenesManager temManager = LoadManager[sceneName];
            temManager.DisposeAllBundleObjs();
        }
    }

    public void UnLoadAeestBundle(string sceneName, string bundleName)
    {
        if (LoadManager.ContainsKey(sceneName))
        {
            IABScenesManager temManager = LoadManager[sceneName];
            temManager.DisposeBundle(bundleName);
        }
    }

    public void UnLoadAllBundles(string sceneName)
    {
        if (LoadManager.ContainsKey(sceneName))
        {
            IABScenesManager temManager = LoadManager[sceneName];
            temManager.DisposeAllBundles();
            System.GC.Collect();
        }
    }

    public void UnLoadAllBundleAndRes(string sceneName)
    {
        if (LoadManager.ContainsKey(sceneName))
        {
            IABScenesManager temManager = LoadManager[sceneName];
            temManager.DisposeAllBundleAndRes();
        }
    }

    public void DebugAllAssetbundle(string sceneName)
    {
        if (LoadManager.ContainsKey(sceneName))
        {
            IABScenesManager temManger = LoadManager[sceneName];
            temManger.DebugAllAssetBundle();
        }
    }

    public bool IsLoadBundleFinish(string sceneName, string bundleName)
    {
        if (LoadManager.ContainsKey(sceneName))
        {
            IABScenesManager temManger = LoadManager[sceneName];
            return temManger.IsLoadFinish(bundleName);
        }
        return false;
    }

    public bool IsLoadingBundle(string sceneName, string bundleName)
    {
        if (LoadManager.ContainsKey(sceneName))
        {
            IABScenesManager temManger = LoadManager[sceneName];
            return temManger.IsLoadingAssetBundle(bundleName);
        }
        return false;
    }

    #endregion



    private void OnDestroy()
    {
        LoadManager.Clear();
        System.GC.Collect();
    }

}
