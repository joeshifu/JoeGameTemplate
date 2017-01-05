﻿using UnityEngine;
using System.Collections;
using System.IO;
using LuaInterface;

/// <summary>
/// 继承自LuaFileUtils，重写里面的ReadFile，
/// </summary>
public class LuaLoader : LuaFileUtils
{
    public LuaLoader()
    {
        instance = this;
        beZip = AppConst.LuaBundleMode; //lua代码是不是打包成bundle的模式
    }

    /// <summary>
    /// 添加打入Lua代码的AssetBundle
    /// </summary>
    /// <param name="bundle"></param>
    public void AddBundle(string bundleName)
    {
        string url = MyTool_Unity.DataPath + bundleName.ToLower();
        if (File.Exists(url))
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(url);
            if (bundle != null)
            {
                bundleName = bundleName.Replace("lua/", "").Replace(".unity3d", "");
                base.AddSearchBundle(bundleName.ToLower(), bundle);
            }
        }
    }

    /// <summary>
    /// 当LuaVM加载Lua文件的时候，这里就会被调用，
    /// 用户可以自定义加载行为，只要返回byte[]即可。
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public override byte[] ReadFile(string fileName)
    {
        return base.ReadFile(fileName);
    }
}
