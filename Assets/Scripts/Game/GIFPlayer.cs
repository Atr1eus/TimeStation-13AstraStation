using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GIFPlayer : MonoBehaviour
{
    public Image gifImage;

    public Sprite[] gifFrames;

    public float frameRate = 0.1f; 

    public bool isPlaying = false;

    private void Awake()
    {
        if (gifImage != null)
        {
            gifImage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ???? GIF ??????????????????
    /// </summary>
    /// <returns>IEnumerator ????§¿?????</returns>
    public IEnumerator PlayGIFBlocking()
    {

        //if (gifImage != null)
        //{
        //    gifImage.gameObject.SetActive(true);
        //}

        if (gifFrames == null || gifFrames.Length == 0)
        {
            Debug.LogError("GIF ????????????? Sprite[] ?????");
            yield break;
        }

        isPlaying = true;

        Debug.Log("???????");

        // ???????
        for (int i = 0; i < gifFrames.Length; i++)
        {
            if (gifImage != null)
            {
                gifImage.sprite = gifFrames[i]; // ???? UI Image
            }
            yield return new WaitForSecondsRealtime(frameRate); // ??????????
        }

        isPlaying = false;
        Debug.Log("GIF ???????");
    }

    /// <summary>
    /// ???? API?????? GIF ???????????????
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
            gifImage.gameObject.SetActive(false); // ????????????
        }
        Debug.Log("??????????????");
    }

    void Start()
    {
        if (gifImage != null)
        {
            gifImage.gameObject.SetActive(false);
        }
    }
}