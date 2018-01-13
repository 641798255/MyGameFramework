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

public class IPathTools {

    public static string GetPlatformFolderName(RuntimePlatform platform)
    {
        switch (platform)
        {
            case RuntimePlatform.Android:
                return "Android";
                break;
            case RuntimePlatform.IPhonePlayer:
                return "IOS";
                break;
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                return "Windows";
                break;
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
                return "OSX";
                break;
            default:
                return null;
                break;
        }
    }

    private static string GetAppFilePath()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return Application.persistentDataPath;
                break;
            case RuntimePlatform.IPhonePlayer:
                return Application.persistentDataPath;
                break;
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                return Application.streamingAssetsPath;
                break;
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
                return Application.persistentDataPath;
                break;
            default:
                return null;
                break;
        }
    }

    public static string GetAssetBundlePath()
    {
        string platFolder = GetPlatformFolderName(Application.platform);
        string allPath = GetAppFilePath()+"/"+platFolder;
        return allPath;
    }



    public static string GetWWWAssetBundlePath()
    {
        string temStr = "";
        if (Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.WindowsPlayer)
        {
            temStr = "file:///" + GetAssetBundlePath();
        }
        else
        {
            string temPath = GetAssetBundlePath();
#if UNITY_ANDRIOD
            temStr = "jar:file://" + temPath;

#elif UNITY_STANDALONE_WIN
            temStr = "file:///" + temPath;
#else
            temStr= "file://" + temPath;
#endif
        }
        return temStr;
    }
}
