using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestionEditor : EditorWindow
{
    private Color _oldColor;
    private bool _extention;
    private Question _question;
    private string question;
    private bool isFirstLoad;

    private string[] toolBar = new string[]{"Create Question", "Edit Question"};
    private int idxBar;

    private QuestionData _data;
    Vector2 scrollPosition;
    
    // Extension
    private bool _extension;
    private string path;

    [MenuItem("Window/Question Editor")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        QuestionEditor window = (QuestionEditor)EditorWindow.GetWindow(typeof(QuestionEditor));
        window.Show();
    }

    void LoadDefaultData()
    {
        if (!isFirstLoad)
        {
            isFirstLoad = true;
            CreateNewQuestionData();

            _data = Resources.Load<QuestionData>("Question Data");
        }
    }
    
    private void OnGUI()
    {
        LoadDefaultData();
        _oldColor = GUI.color;

        idxBar = GUILayout.Toolbar(idxBar, toolBar, GUILayout.Width(600));
        GUILayout.Space(20);

        switch (idxBar)
        {
            case 0:
                GUICreateNewQuestion();
                break;
            case 1:
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, GUILayout.MaxHeight(1000), GUILayout.Width(650));
                GUIEditQuestion();
                GUILayout.EndScrollView();
                
//                if (GUILayout.Button("Save", GUILayout.Width(300)))
//                {
////                    LoadTextLevels();
//                }
                break;
        }
    }

    void GUICreateNewQuestion()
    {
        GUILayout.BeginVertical("Box", GUILayout.Width(600));
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Question", GUILayout.Width(100));
        GUILayout.Space(100);
        _question.question = GUILayout.TextArea(_question.question, GUILayout.Width(400));
        GUILayout.EndHorizontal();

        for (int i = 0; i < 4; i++)
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            _question.answers[i].isCorrect = EditorGUILayout.Toggle("Answer " + (i + 1), _question.answers[i].isCorrect);
            if (_question.answers[i].isCorrect)
            {
                for (int j = 0; j < 4; j++)
                {
                    if( j == i ) continue;

                    _question.answers[j].isCorrect = false;
                }
            }
            
            _question.answers[i].answer = GUILayout.TextArea(_question.answers[i].answer, GUILayout.Width(400));
            GUILayout.EndHorizontal();
        }
        
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Space(400);
        
        if (GUILayout.Button("Create New Question", GUILayout.Width(200)))
        {
            if( _question.question == "" ) return;

            bool isChecked = false;
            foreach (var val in _question.answers)
            {
                if( val.answer == "" ) return;

                if (val.isCorrect) isChecked = true;
            }
            
            if( !isChecked ) return;
            
            _data.questionData.Add(_question);
            EditorUtility.SetDirty(_data);

            CreateNewQuestionData();
        }

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        
        GUILayout.Space(20);
        _extension = GUILayout.Toggle(_extension, "Extension");
        
        if( _extension )
            GUIExtension();
    }

    void GUIEditQuestion()
    {
        var c = 1;
        foreach (var val in _data.questionData)
        {
            GUIQuestionItem(val, c);
            c++;
        }
    }

    void GUIQuestionItem(Question question, int numberQuestion)
    {
        GUILayout.BeginVertical("Box", GUILayout.Width(600));
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Question " + numberQuestion, GUILayout.Width(100));
        GUILayout.Space(100);
        question.question = GUILayout.TextArea(question.question, GUILayout.Width(400));
        GUILayout.EndHorizontal();

        for (int i = 0; i < 4; i++)
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            question.answers[i].isCorrect = EditorGUILayout.Toggle("Answer " + (i + 1), question.answers[i].isCorrect);
            if (question.answers[i].isCorrect)
            {
                for (int j = 0; j < 4; j++)
                {
                    if( j == i ) continue;

                    question.answers[j].isCorrect = false;
                }
            }
            
            question.answers[i].answer = GUILayout.TextArea(question.answers[i].answer, GUILayout.Width(400));
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
        
        GUILayout.Space(10);
    }
    
    void Save()
    {
        
    }
    
    void CreateNewQuestionData()
    {
        _question = new Question();
        _question.answers = new Answer[4];
    }

    void GUIExtension()
    {
        GUILayout.BeginVertical();
        
//        GUILayout.BeginHorizontal();
//        
//        GUILayout.Label("Path", GUILayout.Width(100));
//        path = GUILayout.TextField(path);
//        
//        GUILayout.EndHorizontal();
//        
        GUILayout.Space(10);

        if (GUILayout.Button("Generate from text file", GUILayout.Width(300)))
        {
            LoadTextLevels();
        }
        
        GUILayout.EndVertical();
    }
    
    void LoadTextLevels()
    {
        var mapText = Resources.Load<TextAsset>("Question");
        
        ProcesDataFromString(mapText.text);
    }
    
    void ProcesDataFromString(string mapText)
    {
        string[]  lines     = mapText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        Question question = new Question();
        question.answers = new Answer[4];

        int count = 0;
        foreach (string line in lines)
        {
            if (line.StartsWith("Q "))
            {
                var q = line.Remove(0, 2);
                question.question = q;
            }
            else
            {
                if (line.StartsWith("T"))
                    question.answers[count].isCorrect = true;
                else
                    question.answers[count].isCorrect = false;

                var a = line.Remove(0, 2);
                question.answers[count].answer = a;

                count++;
            }
            
            if (count == 4)
            {
                _data.AddListQuestion(question);
                count = 0;
                question         = new Question();
                question.answers = new Answer[4];
            }
//
//            string[] st = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
//
//            for (int i = 0; i < st.Length; i++)
//            {
//                int value = int.Parse(st[i].ToString());
//
//                if (value == 0 || value >= ballDatas.items.Count)
//                {
//                    continue;
//                }
//
//                levelData.rows[count].cols[i].kind        = CloneItem(ballDatas.items[value - 1]);
//                levelData.rows[count].cols[i].kind.isUsed = true;
//            }
//
//            count++;
        }

//        levelData.CheckMaxHeight();
//
//        SaveLevel(levelData);
//
//        if (level == currentLevel)
//        {
//            for (int i = 0; i < maxHeight; i++)
//            {
//                for (int j = 0; j < maxWidth; j++)
//                {
//                    itemKinds[i, j] = levelData.rows[i].cols[j].kind;
//                }
//            }
//        }
    }
}
