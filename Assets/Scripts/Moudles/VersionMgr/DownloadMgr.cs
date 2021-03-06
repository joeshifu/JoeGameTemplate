﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

/// <summary>
/// 下载任务
/// </summary>
public class DownloadTask
{
    public string Url { get; set; }//路径

    public string FileName { get; set; }//文件名

    public string MD5 { get; set; }//md5值

    public int Size { get; set; }//文件大小
}

/// <summary>
/// 下载器
/// </summary>
public class DownloadMgr
{
    private static DownloadMgr m_inst;
    public static DownloadMgr Inst
    {
		get{
        if (m_inst == null)
            m_inst = new DownloadMgr();
        return m_inst;
		}
    }

    private readonly WebClient mWebClient = new WebClient();
    private Action<string> asynDownloadTxtCallBack;//异步下载txt的完成的回调
    private Action<string, int, int, int> downloadProgressChangedCallBack;//下载进度的回调
    private Action<string> oneTaskFinished;//一个任务完成的回调

    public DownloadMgr()
    {
        this.mWebClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.webClient_DownloadProgressChanged);
        this.mWebClient.DownloadFileCompleted += new AsyncCompletedEventHandler(this.webClient_DownloadFileCompleted);
        this.mWebClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.webClient_DownloadStringCompleted);
    }

    public void InitDownloadCallBacks(Action<string> _asynDownloadTxtCallBack, Action<string, int, int, int> _downloadProgressChangedCallBack, Action<string> _oneTaskFinished)
    {
        this.asynDownloadTxtCallBack = _asynDownloadTxtCallBack;
        this.downloadProgressChangedCallBack = _downloadProgressChangedCallBack;
        this.oneTaskFinished = _oneTaskFinished;
    }

    private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        string url = e.UserState.ToString();
        int progress = e.ProgressPercentage;
        int received = (int)e.BytesReceived;
        int total = (int)e.TotalBytesToReceive;

        if (this.downloadProgressChangedCallBack != null)
        {
            this.downloadProgressChangedCallBack(url, progress, received, total);
        }
        Thread.Sleep(100);
    }

    private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
    {
        if (e.Error != null)
        {
            this.HandleDownloadError(e.Error);
        }
        else
        {
            if (this.oneTaskFinished != null)
            {
                this.oneTaskFinished(e.UserState.ToString());
            }
        }
    }

    private void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
    {
        if (this.asynDownloadTxtCallBack != null)
        {
            this.asynDownloadTxtCallBack(e.Result);
        }
    }

    /// <summary>
    /// 同步下载Text,我这里用来下载version.txt
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public string DownLoadText(string url)
    {
        try
        {
            return this.mWebClient.DownloadString(url);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            return string.Empty;
        }
    }


    /// <summary>
    /// 异步下载Text
    /// </summary>
    public void AsynDownLoadText(DownloadTask task)
    {
        try
        {
            this.mWebClient.DownloadStringAsync(new Uri(task.Url), task.Url);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    /// <summary>
    /// 异步下载file
    /// </summary>
    /// <param name="task"></param>
    public void AsynDownLoadFile(DownloadTask task)
    {
        try
        {
            this.mWebClient.DownloadFileAsync(new Uri(task.Url), task.FileName, task.Url);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    /// <summary>
    /// 显示下载异常情况
    /// </summary>
    /// <param name="e"></param>
    /// <param name="mycontinue"></param>
    /// <param name="again"></param>
    /// <param name="finished"></param>
    void HandleDownloadError(Exception e)
    {
        if (e != null)
        {
            if ((e.Message.Contains("ConnectFailure") || e.Message.Contains("NameResolutionFailure")) || e.Message.Contains("No route to host"))
            {
                Debug.LogError("-----------------Webclient ConnectFailure-------------");
            }
            else if (e.Message.Contains("(404) Not Found") || e.Message.Contains("403"))
            {
                Debug.LogError("-----------------WebClient NotFount-------------");

            }
            else if (e.Message.Contains("Disk full"))
            {
                Debug.LogError("-----------------WebClient Disk full-------------");
            }
            else if (e.Message.Contains("timed out") || e.Message.Contains("Error getting response stream"))
            {
                Debug.LogError("-----------------WebClient timed out-------------");
            }
            else if (e.Message.Contains("Sharing violation on path"))
            {
                Debug.LogError("-----------------WebClient Sharing violation on path-------------");
            }
            else
            {
                Debug.LogError(e.Message);
            }
        }
    }

    /// <summary>
    /// 关闭
    /// </summary>
    public void Destroy()
    {
        this.mWebClient.CancelAsync();
        this.mWebClient.Dispose();
    }

}
