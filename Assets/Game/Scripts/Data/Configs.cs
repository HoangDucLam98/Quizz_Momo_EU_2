using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Configs", fileName = "Configs")]
public class Configs : SingletonScriptableObject<Configs>
{
    public float timeLoadQuestion = 10f;
    public int numberQuestionOnTime = 5;
    public DataTimeConfig[] dataTime;

    public int CheckScore(float time)
    {
        for (int i = dataTime.Length - 1; i >= 0; i--)
        {
            if (dataTime[i].timeLimitForScore < time) return dataTime[i].scoreCorresponding;
        }
        
        return 0;
    }

    [Header("GamePlay Single")]
    public float timeLoadquestionTime_1 = 15f;
    public float timeLoadquestionTime_2 = 20f;
    public float timeDelayCheck = 2f;
    public int numberQuestionPerPlay = 15;

    public int numberRestQuestionChangeTimeLoadQuestion = 5;

    public int maxEasyIndex = 73;
    public int maxMiddleIndex = 97;
}

[System.Serializable]
public struct DataTimeConfig
{
    public float timeLimitForScore;
    public int scoreCorresponding;
}
