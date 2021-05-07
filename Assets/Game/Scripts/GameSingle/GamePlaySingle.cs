using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlaySingle : MonoBehaviour
{
    private string PRE_TIME_ANIM = "Time";

    public        QuestionData   QuestionData;
    public static GamePlaySingle Ins;

    [SerializeField] private Text                 question;
    [SerializeField] private AnswerButtonSingle[] _answerButtons;
    [SerializeField] private Text                 timeChangeQuestion;
    [SerializeField] private Text                 currentQuestionTxt;

    public  bool  isChecked,     isAnswerLoaded, isEnded, isNexted, isHided;
    private float timeLoadLevel, timeDelayLoad;

    private                  Question currentQuestion;
    [SerializeField] private int      numberRestQuestion;
    int                               restTime;

    // Help Box
    [SerializeField] private Button     hideQuestionBtn, audienceBtn, nextBtn, nextQuestionBtn, settingBtn;
    [SerializeField] private LoseSingle _loseSingle;
    [SerializeField] private CardSingle _cardSingle;

    [SerializeField] Image hideQuestionUsed, audienceUsed, nextUsed;

    private int RestTime
    {
        get => restTime;
        set
        {
            if (value != restTime)
            {
                restTime = value;
                timeAnimation.Play(PRE_TIME_ANIM);
            }
        }
    }

    [SerializeField] private Animation timeAnimation;

    private bool isRequiringAudience;

    private List<int> indexQuestionUsed;

    private bool isOpenSetting;

    public bool IsOpenSetting
    {
        get => isOpenSetting;
        set
        {
            isOpenSetting = value;
            _settingSingle.gameObject.SetActive(isOpenSetting);
        }
    }

    [SerializeField] private GameObject    startPannel;
    [SerializeField] private SettingSingle _settingSingle;

    // PC
    [SerializeField] private Image timeLine;
    private                  float maxTime;

    private void Awake()
    {
        Ins     = this;
        isEnded = true;

        IsOpenSetting = false;

        settingBtn.onClick.AddListener(() => IsOpenSetting = !isOpenSetting);
        // quitBtn.onClick.AddListener(() =>
        // {
        //     GoHome();
        // });
//        Reload();
    }

    public void GoHome()
    {
        isEnded = true;
        startPannel.Show();
    }

    public void Reload()
    {
        AudioManager.Ins.PlaySound(SoundType.StartGame);

        hideQuestionUsed.Hide();
        audienceUsed.Hide();
        nextUsed.Hide();
        nextQuestionBtn.Hide();
        IsOpenSetting                = false;
        timeLoadLevel                = Configs.Instance.timeLoadquestionTime_1;
        indexQuestionUsed            = new List<int>();
        numberRestQuestion           = Configs.Instance.numberQuestionPerPlay;
        isEnded                      = false;
        isNexted                     = false;
        hideQuestionBtn.interactable = true;
        audienceBtn.interactable     = true;
        nextBtn.interactable         = true;
        timeDelayLoad                = 0;
        maxTime                      = timeLoadLevel;

        LoadQuestion();
//        timeLoadLevel                = 0;
    }

    public void Continue()
    {
        isEnded = false;
    }

    public void CheckAnswer(bool result)
    {
        isChecked           = true;
        isRequiringAudience = false;

        timeLoadLevel = 1f;
    }

    private void Update()
    {
        if (numberRestQuestion < 0 || isRequiringAudience || isEnded) return;

        if (timeDelayLoad > 0)
        {
//            Debug.Log(timeDelayLoad);
            timeDelayLoad -= Time.deltaTime;
            return;
        }

        if (timeLoadLevel <= 0)
        {
            if (isAnswerLoaded)
            {
                isAnswerLoaded = false;
                isHided        = false;

                timeDelayLoad = Configs.Instance.timeDelayCheck;
                isEnded       = !CheckAnswerAnim();
            }

            if (timeDelayLoad > 0) return;

            if (numberRestQuestion - 1 < 0)
            {
                isEnded = true;
                _cardSingle.OnShow(true);
                return;
            }

            nextQuestionBtn.Show();

//            LoadQuestion();
//
//            timeLoadLevel = numberRestQuestion > Configs.Instance.numberRestQuestionChangeTimeLoadQuestion
//                ? Configs.Instance.timeLoadquestionTime_1
//                : Configs.Instance.timeLoadquestionTime_2;
        }

        timeLoadLevel -= Time.deltaTime;
        if (timeLine != null)
        {
            timeLine.fillAmount = timeLoadLevel / maxTime;
        }

        if (!isAnswerLoaded) return;

        var t = Mathf.CeilToInt(timeLoadLevel);
        RestTime                = t;
        timeChangeQuestion.text = t.ToString("00");
    }

    public void In_LoadNextQuestion()
    {
        LoadQuestion();

        timeLoadLevel = numberRestQuestion > Configs.Instance.numberRestQuestionChangeTimeLoadQuestion
            ? Configs.Instance.timeLoadquestionTime_1
            : Configs.Instance.timeLoadquestionTime_2;
    }

    void LoadQuestion()
    {
        nextQuestionBtn.Hide();
        numberRestQuestion--;
//
//        currentQuestionTxt.text = (Configs.Instance.numberQuestionPerPlay - numberRestQuestion).ToString("00");
//
//        if (numberRestQuestion < 0)
//        {
//            isEnded = true;
//            _cardSingle.OnShow(true);
//            return;
//        }

        isAnswerLoaded = true;
        isChecked      = false;
        int idxQuestion = 0;

        do
        {
            if (numberRestQuestion <= 3)
            {
                idxQuestion = Random.Range(Configs.Instance.maxMiddleIndex, QuestionData.questionData.Count);
            }
            else if (numberRestQuestion <= 10)
            {
                idxQuestion = Random.Range(Configs.Instance.maxEasyIndex, Configs.Instance.maxMiddleIndex);
            }
            else
            {
                idxQuestion = Random.Range(0, Configs.Instance.maxEasyIndex);
            }
        } while (indexQuestionUsed.Contains(idxQuestion));

        indexQuestionUsed.Add(idxQuestion);

        currentQuestion = QuestionData.questionData[idxQuestion];

        question.text = "Câu " + (Configs.Instance.numberQuestionPerPlay - numberRestQuestion) + ". " +
                        currentQuestion.question;
        for (int i = 0; i < _answerButtons.Length; i++)
        {
            _answerButtons[i].LoadAnswer(currentQuestion.answers[i]);
        }

        if (numberRestQuestion == Configs.Instance.numberRestQuestionChangeTimeLoadQuestion)
        {
            isEnded = true;
            _cardSingle.OnShow();
        }
    }

    bool CheckAnswerAnim()
    {
        for (int i = 0; i < _answerButtons.Length; i++)
        {
            if (_answerButtons[i].CheckAnim())
                return true;
        }

        if (isNexted)
        {
            isNexted = false;
            return true;
        }

        isEnded = true;
//        _loseSingle.Show();
        StartCoroutine(ShowLose());
        return false;
    }

    IEnumerator ShowLose()
    {
        yield return new WaitForSeconds(2f);
        AudioManager.Ins.PlaySound(SoundType.Lose);
        _loseSingle.Show();
    }

    public void In_CheckHideAnswer()
    {
        if (isChecked || isRequiringAudience || isNexted || isHided) return;

        isHided = true;
        hideQuestionUsed.Show();

        hideQuestionBtn.interactable = false;

        var  numberAnswerNeedHide = 2;
        bool check;
        do
        {
            check = _answerButtons[Random.Range(0, 4)].CheckHide();
            if (check) numberAnswerNeedHide--;
        } while (numberAnswerNeedHide > 0 || !check);
    }

    public void In_Audience()
    {
        if (isChecked || isRequiringAudience || isNexted || isHided) return;

        audienceUsed.Show();

        audienceBtn.interactable = false;

        isRequiringAudience = true;
    }

    public void In_NextQuestion()
    {
        if (isChecked || isRequiringAudience || isNexted || isHided) return;

        nextUsed.Show();

        nextBtn.interactable = false;

        timeLoadLevel = 1f;
        isNexted      = true;
    }
}