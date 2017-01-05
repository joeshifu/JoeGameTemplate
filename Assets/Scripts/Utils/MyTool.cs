using System.Collections;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public class MyTool
{
    public static string Uid(string uid)
    {
        int position = uid.LastIndexOf('_');
        return uid.Remove(0, position + 1);
    }

    public static long GetTime()
    {
        TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
        return (long)ts.TotalMilliseconds;
    }

    /// <summary>
    /// 计算字符串的MD5值
    /// </summary>
    public static string MD5String(string source)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
        md5.Clear();

        string destString = "";
        for (int i = 0; i < md5Data.Length; i++)
        {
            destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
        }
        destString = destString.PadLeft(32, '0');
        return destString;
    }

    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    public static string MD5File(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }

    /// <summary>
    /// 取得文本内容
    /// </summary>
    public static string GetFileText(string path)
    {
        return File.ReadAllText(path);
    }


    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="path">文件创建目录</param>
    /// <param name="filename">文件的名称</param>
    /// <param name="info">写入的内容</param>
    public static void CreateFile(string path, string filename, string info)
    {
        DeleteFile(path, filename);
        FileStream fs = new FileStream(path + filename, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(info.Trim());
        sw.Flush();
        sw.Close();
        fs.Close();
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="path">删除文件的路径</param>
    /// <param name="name">删除文件的名称</param>
    public static void DeleteFile(string path, string name)
    {
        if (File.Exists(path + name))
            File.Delete(path + name);
    }




}
