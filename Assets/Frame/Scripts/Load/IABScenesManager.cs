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
using System;
using System.IO;

public class IABScenesManager
{
    private IABManager _ABManager;
    //public IABScenesManager(string scenesName)
    //{
    //    _ABManager = new IABManager(scenesName);
    //}

    private Dictionary<string, string> AllAsset = new Dictionary<string, string>();

    public void ReadConfig(string scenesName)
    {
        string path = IPathTools.GetAssetBundlePath()  + scenesName + "Record.txt";
        _ABManager = new IABManager(scenesName);
        ReadConfigPath(path);
    }

    private void ReadConfigPath(string path)
    {
        FileStream fs = new FileStream(path,FileMode.Open);
        StreamReader br = new StreamReader(fs);
        int allCount = br.ReadToEnd().Split('\n').Length-1;
        if (allCount > 0)
        {
            for (int i = 0; i < allCount; i++)
            {
                string temStr = br.ReadLine();
                string[] temArr = temStr.Split(" ".ToCharArray());
                AllAsset.Add(temArr[0], temArr[1]);
            }
            br.Close();
            fs.Close();
        }
    }

    public void LoadAsset(string bundleName,LoadProgress progress,LoadAssetBundleCallBack callBack)
    {
        if (AllAsset.ContainsKey(bundleName))
        {
            string temName = AllAsset[bundleName];
            _ABManager.LoadAssetBundle(temName, progress, callBack);
        }
        else
        {
            Debug.LogError("not contain bundlename======="+bundleName);
        }
    }

    public string GetBundleName(string bundleName)
    {
        if (AllAsset.ContainsKey(bundleName))
        {
            string temName = AllAsset[bundleName];
            return temName;
        }
        return null;
    }

    #region 由下层提供API

    public IEnumerator LoadAssetAsync(string bundleName)
    {
        yield return _ABManager.LoadAssetBundles(bundleName);
    }

    public UnityEngine.Object GetSingleRes(string bundleName,string resName)
    {
        if (AllAsset.ContainsKey(bundleName))
        {
            return _ABManager.GetSingleRes(AllAsset[bundleName], resName);
        }
        else
        {
            Debug.LogError("not contain bundlename=======" + bundleName);
            return null;
        }
    }

    public UnityEngine.Object[] GetMutiRes(string bundleName, string resName)
    {
        if (AllAsset.ContainsKey(bundleName))
        {
            return _ABManager.GetMutiRes(AllAsset[bundleName], resName);
        }
        else
        {
            Debug.LogError("not contain bundlename=======" + bundleName);
            return null;
        }
    }

    public void DisposeObjRes(string bundleName, string resName)
    {
        if (AllAsset.ContainsKey(bundleName))
        {
            _ABManager.DisposeObjs(AllAsset[bundleName],resName);
        }
        else
        {
            Debug.LogError("not contain bundlename=======" + bundleName);
         
        }
    }

    public void DisposeBundleRes(string bundleName)
    {
        if (AllAsset.ContainsKey(bundleName))
        {
            _ABManager.DisposeSingleBundleObjs(bundleName);
        }
        else
        {
            Debug.LogError("not contain bundlename=======" + bundleName);

        }
    }

    public void DisposeAllBundleObjs()
    {
        _ABManager.DisposeAllObjs();
    }

    public void DisposeBundle(string bundleName)
    {
        if (AllAsset.ContainsKey(bundleName))
        {
            _ABManager.DisposeBundle(bundleName);
        }
    }

    public void DisposeAllBundles()
    {
        _ABManager.DisposeAllBundle();
        AllAsset.Clear();
    }

    public void DisposeAllBundleAndRes()
    {
        _ABManager.DisposeAllAssetBundleAndRes();
        AllAsset.Clear();

    }

    public void DebugAllAssetBundle()
    {
        List<string> keys = new List<string>();
        keys.AddRange(AllAsset.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            _ABManager.DebugAssetBundle(AllAsset[keys[i]]);
        }
    }

    public bool IsLoadFinish(string bundleName)
    {
        if (AllAsset.ContainsKey(bundleName))
        {
            return _ABManager.IsLoadingFinish(AllAsset[bundleName]);
        }
        return false;
    }

    public bool IsLoadingAssetBundle(string bundleName)
    {
        if (AllAsset.ContainsKey(bundleName))
        {
            return _ABManager.IsLoadingAssetBundle(AllAsset[bundleName]);
        }
        return false;
    }

    #endregion


}


