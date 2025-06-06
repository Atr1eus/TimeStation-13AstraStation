using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class FinalManager
{

    public static int FinalNum = -1;
    public static bool IsWin()
    {
        if (RoundManager.Instance.currentRound >= 30)
        {
            Debug.Log("win");
            FinalNum = 0;
            return true;
        }
        return false;
    }
    public static bool IsLose()
    {
        if (PlayerController.Instance.IsLose())
        {

            Debug.Log("lose");
            FinalNum = 1;
            return true;
        }
        return false;
    }

    public static bool IsFull()
    {
        if (GameManager.Instance.currentLoseNpcList.Count >= 3)
        {
            Debug.Log("full");
            FinalNum = 2;
            return true;
        }
        return false;
    }


}