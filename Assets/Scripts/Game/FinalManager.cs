using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class FinalManager
{

    public static int FinalNum = -1;
    public static bool IsWin()
    {
        if (RoundManager.Instance.currentRound >= 30)
        {
            FinalNum = 0;
            return true;
        }
        return false;
    }
    public static bool IsLose()
    {
        if (PlayerController.Instance.IsLose()) FinalNum = 0;
        return PlayerController.Instance.IsLose();
    }

    public static bool IsFull()
    {
        if (GameManager.Instance.currentLoseNpcList.Count >= 3)
        {
            FinalNum = 2;
            return true;
        }
        return false;
    }


}