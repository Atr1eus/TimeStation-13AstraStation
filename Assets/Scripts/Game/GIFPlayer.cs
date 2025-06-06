using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GIFPlayer : MonoBehaviour
{
    public Image gifImage; 

    // GIF 帧数组
    public Sprite[] gifFrames;

    // GIF 播放设置
    public float frameRate = 0.1f; // 每帧间隔时间（秒），根据 GIF 帧率调整

    // 播放完成标志
    public bool isPlaying = false;

    private void Awake()
    {
        if (gifImage != null)
        {
            gifImage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 播放 GIF 并阻塞主进程直到完成
    /// </summary>
    /// <returns>IEnumerator 用于协程控制</returns>
    public IEnumerator PlayGIFBlocking()
    {
        
        //if (gifImage != null)
        //{
        //    gifImage.gameObject.SetActive(true);
        //}

        if (gifFrames == null || gifFrames.Length == 0)
        {
            Debug.LogError("GIF 帧数组为空，请检查 Sprite[] 配置！");
            yield break;
        }

        isPlaying = true;

        Debug.Log("开始播放");

        // 逐帧播放
        for (int i = 0; i < gifFrames.Length; i++)
        {
            if (gifImage != null)
            {
                gifImage.sprite = gifFrames[i]; // 更新 UI Image
            }
            yield return new WaitForSecondsRealtime(frameRate); // 阻塞指定时间
        }

        isPlaying = false;
        Debug.Log("GIF 播放完成");
    }

    /// <summary>
    /// 公共 API：启动 GIF 播放并阻塞主进程
    /// </summary>
    public void StartGIFAndBlock()
    {
        if (gifImage != null)
        {
            gifImage.gameObject.SetActive(true);
        }
        StartCoroutine(PlayGIFAndBlockCoroutine());
    }

    private IEnumerator PlayGIFAndBlockCoroutine()
    {
        yield return StartCoroutine(PlayGIFBlocking());
        if (gifImage != null)
        {
            gifImage.gameObject.SetActive(false); // 播放完成后隐藏
        }
        Debug.Log("主进程已解除阻塞");
    }

    void Start()
    {
        if (gifImage != null)
        {
            gifImage.gameObject.SetActive(false);
        }
    }
}
