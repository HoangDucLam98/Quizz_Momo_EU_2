using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestionData))]
public class QuestionInspector : Editor
{
    private QuestionData data;
    private string     ID        = "1aC1ykSMlMbsPoxy2uEe8RUJTG8ZrQ7xdNvjYboBedM8";
    public  string     sheetName = "Question";
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        data = (QuestionData)target;

        GUILayout.Label("ID GG Shet");
        GUILayout.Space(20);

        GUILayout.BeginHorizontal();

        ID = EditorGUILayout.TextField("ID: ", ID);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        sheetName = EditorGUILayout.TextField("SheetName: ", sheetName);

        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Retrieving"))
        {
            RetrievingData();
        }
        
        if (GUILayout.Button("RetrievingNewData"))
        {
            RetrievingNewData();
        }

        GUILayout.EndHorizontal();
    }
    
    public void RetrievingData()
    {
        data.questionData = new List<Question>();
        
        ReadExcelOnline.Connect();
        ReadExcelOnline.GetTable(ID, sheetName, d =>
        {
            foreach (var key in d.Keys)
            {
                if (key < 1)
                    continue;
                
                Question q = new Question();
                q.answers = new Answer[4];
                
                var rows = d[key];
                q.question = rows[1];
                q.answers[0].answer = rows[2];
                q.answers[1].answer = rows[3];
                q.answers[2].answer = rows[4];
                q.answers[3].answer = rows[5];

                if (rows[6].Equals(rows[2]))
                {
                    q.answers[0].isCorrect = true;
                }
                else if( rows[6].Equals(rows[3]) )
                {
                    q.answers[1].isCorrect = true;
                }
                else if( rows[6].Equals(rows[4]) )
                {
                    q.answers[2].isCorrect = true;
                }
                else
                {
                    q.answers[3].isCorrect = true;
                }
                
                data.questionData.Add(q);
            }
        });

        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
    }

    void RetrievingNewData()
    {
//        data.questionData = new List<Question>();

        var originalList = Clone(data.questionData);

        int c = 1;

        foreach (var val in originalList)
        {
            for (int i = 0; i < 4; i++)
            {
                if( val.answers[i].isCorrect && i == 3 ) Debug.Log(c + " - " + i);
            }

            c++;
        }
        
//        var list = new List<Question>();
//        
//        ReadExcelOnline.Connect();
//        ReadExcelOnline.GetTable(ID, sheetName, d =>
//        {
//            foreach (var key in d.Keys)
//            {
//                if (key < 1)
//                    continue;
//                
//                if( key >= 24 ) return;
//                
//                Question q = new Question();
//                q.answers = new Answer[4];
//                
//                var rows = d[key];
////                Debug.Log(rows[2] + " - " + rows[3] + " - " + rows[4] + " - " + rows[5]);
//                q.question          = rows[1];
//                q.answers[0].answer = rows[2];
//                q.answers[1].answer = rows[3];
//                q.answers[2].answer = rows[4];
//                q.answers[3].answer = rows[5];
//                
//                if (rows[6].Equals(rows[2]))
//                {
//                    q.answers[0].isCorrect = true;
//                }
//                else if( rows[6].Equals(rows[3]) )
//                {
//                    q.answers[1].isCorrect = true;
//                }
//                else if( rows[6].Equals(rows[4]) )
//                {
//                    q.answers[2].isCorrect = true;
//                }
//                else
//                {
//                    q.answers[3].isCorrect = true;
//                }
//                
////                data.questionData.Add(q);
//                list.Add(q);
//            }
//        });
//        
//        list.AddRange(originalList);
//        
//        foreach (var val in list)
//        {
//            Debug.Log(val.question);
//        }

//        EditorUtility.SetDirty(data);
//        AssetDatabase.SaveAssets();
    }

    public List<Question> Clone(List<Question> list)
    {
        var l = new List<Question>();
        
        foreach (var item in list)
        {
            l.Add(item);
        }

        return l;
    }
}