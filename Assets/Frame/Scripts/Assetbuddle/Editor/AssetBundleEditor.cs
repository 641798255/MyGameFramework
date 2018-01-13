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
using UnityEditor;
using System.IO;

public class AssetBundleEditor : Editor {

    [MenuItem("ITools/BuildAssetBundle")]
    public static void BuildAssetBundle()
    {
        string outPath = IPathTools.GetAssetBundlePath();//Application.streamingAssetsPath + "/Assetbundle";
        BuildPipeline.BuildAssetBundles(outPath, 0, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }

    [MenuItem("ITools/MarkAssetBundle")]
    public static void MarkAssetBundle()
    {
        AssetDatabase.RemoveUnusedAssetBundleNames();
        string path = Application.dataPath + "/Art/Scenes/";
        DirectoryInfo info = new DirectoryInfo(path);
        FileSystemInfo[] fileInfos = info.GetFileSystemInfos();
        for (int i = 0; i < fileInfos.Length; i++)
        {
            if (fileInfos[i] is DirectoryInfo)
            {
                string temPath = Path.Combine(path,fileInfos[i].Name);
                Debug.Log(temPath);
                SceneOverview(temPath);
            }
        }
        string outPath = IPathTools.GetAssetBundlePath();
        CopyRecord(path, outPath);
        AssetDatabase.Refresh();
    }

    public static void CopyRecord(string srcPath,string disPath)
    {
        DirectoryInfo info = new DirectoryInfo(srcPath);
        if (!info.Exists)
        {
            Debug.LogError("记录文件夹不存在");
            return;
        }
        if (!Directory.Exists(disPath))
        {
            Directory.CreateDirectory(disPath);
        }
        FileSystemInfo[] files = info.GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file=files[i] as FileInfo;
            if (file != null && file.Extension == ".txt")
            {
                string srcFile = srcPath + file.Name;
                string disFile = disPath + "/" + file.Name;
                File.Copy(srcFile,disFile,true);
            }

        }

    }

    public static void SceneOverview(string scenesPath)
    {
        string txtRecord = "Record.txt";
        string recordPath = scenesPath+txtRecord;
        FileStream file = new FileStream(recordPath,FileMode.Create);
        StreamWriter bw=new StreamWriter(file);

        Dictionary<string, string> readDic = new Dictionary<string, string>();
        ChangeHead(scenesPath,readDic);
        foreach (string key in readDic.Keys)
        {
            Debug.Log(key);
            Debug.Log(readDic[key]);
            bw.Write(key);
            bw.Write(" ");
            bw.Write(readDic[key]);
            bw.Write("\n");
        }
        bw.Flush();
        bw.Close();
        file.Close();
    }

    public static void ChangeHead(string path, Dictionary<string, string> temWriter)
    {
        int temCount = path.IndexOf("Asset");
        int pathLength = path.Length;
        string temPath = path.Substring(temCount,pathLength-temCount);
        DirectoryInfo dir = new DirectoryInfo(path);
        if (dir != null)
        {
            ListFiles(dir, temPath, temWriter);
        }
        else
        {
            Debug.Log("this path is not exist");
        }
    }

    public static void ListFiles(FileSystemInfo info, string path, Dictionary<string, string> temWriter)
    {
      
        if (!info.Exists)
        {
            Debug.Log("this info is not exist");
            return;
        }
        DirectoryInfo dir = info as DirectoryInfo;
     
        FileSystemInfo[] files = dir.GetFileSystemInfos();
      
        for (int i = 0; i <files.Length; i++)
        {
            FileInfo file = files[i] as FileInfo;
            if (file != null)
            {

                ChangeMark(file,path,temWriter);
            }
            else
            {
           
                ListFiles(files[i],path,temWriter);
            }
        }
    }

    public static void ChangeMark(FileInfo file, string path, Dictionary<string, string> temWriter)
    {
        if (file.Extension==".meta")
        {
            return;
        }
        string markString = GetBundlePath(file,path);
        ChangeAssetMark(file,markString,temWriter);
    }

    public static void ChangeAssetMark(FileInfo file, string markString, Dictionary<string, string> temWriter)
    {
        string fullName = file.FullName;
        int assetCount = fullName.IndexOf("Asset");
        string assetName = fullName.Substring(assetCount,fullName.Length-assetCount);
        AssetImporter importer = AssetImporter.GetAtPath(assetName);
        importer.assetBundleName=markString;
        string modePath = null;
        if (file.Extension == ".unity")
        {
            importer.assetBundleVariant = "u3d";
            modePath = markString.ToLower() + "." + importer.assetBundleVariant;
        }
        else
        {
            importer.assetBundleVariant = "ld";
            modePath = markString.ToLower() + "." + importer.assetBundleVariant;
        }
        string modelName = "";
        string[] subMark = markString.Split("/".ToCharArray());
        if (subMark.Length > 1)
        {
            modelName = subMark[1];
        }
        else
        {
            modelName = markString;
        }
   
        if (!temWriter.ContainsKey(modelName))
        {
            temWriter.Add(modelName,modePath);
        }
    }

    public static string GetBundlePath(FileInfo file, string path)
    {
 
        string temPath = file.FullName;
        temPath = FixedPath(temPath);


        int assetCount = temPath.IndexOf(path);
        assetCount += path.Length + 1;

        int nameCount = temPath.LastIndexOf(file.Name);
        int temCount = path.LastIndexOf("/");

        string scenesHead = path.Substring(temCount+1,path.Length-temCount-1);
 
        int temLength = nameCount - assetCount;
        if (temLength>0)
        {
            string subString = temPath.Substring(assetCount,temPath.Length-assetCount);

            string[] result = subString.Split("/".ToCharArray());

            return scenesHead + "/" + result[0];
        }
        else
        {
            return scenesHead;
        }
    }

    public static string FixedPath(string path)
    {
        path = path.Replace("\\","/");
        return path;
    }
}
