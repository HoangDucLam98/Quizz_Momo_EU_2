using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Question", fileName = "Question Data")]
public class QuestionData : ScriptableObject
{
    public List<Question> questionData;

    #if UNITY_EDITOR
    
    public void AddListQuestion(Question question)
    {
        questionData.Add(question);
        EditorUtility.SetDirty(this);
    }
    
    #endif
}

[System.Serializable]
public struct Question
{
    public string question;
    public Answer[] answers;
}

[System.Serializable]
public struct Answer
{
    public bool isCorrect;
    public string answer;
}