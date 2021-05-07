using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButtonSingle : MonoBehaviour
{
    private const string PRE_NORMAL_ANIM         = "Normal_Button";
    private const string PRE_ANSWER_CORRECT_ANIM = "Correct_Button";
    private const string PRE_ANSWER_WRONG_ANIM   = "Wrong_Button";
    private const string PRE_ANSWER_HIDE_ANIM    = "Hide_Button";
    [SerializeField] private Button    button;
    [SerializeField] private Text      _text;
    [SerializeField] private Animation _animation;
    private                  Answer    _answer;

    private bool isChecked;
    private bool isHided;

    private void Reset()
    {
        button = GetComponent<Button>();
        _text  = GetComponentInChildren<Text>();
        _animation = GetComponent<Animation>();
    }

    private void Start()
    {
        button.onClick.AddListener(CheckAnswer);
    }

    private void CheckAnswer()
    {
        if( GamePlaySingle.Ins.isChecked ) return;

        isChecked           = true;
        button.interactable = false;
        GamePlaySingle.Ins.CheckAnswer(_answer.isCorrect);
    }

    public void LoadAnswer(Answer answer)
    {
        _animation.Play(PRE_NORMAL_ANIM);
        isChecked           = false;
        isHided = false;
        button.interactable = true;
        _answer             = answer;
        _text.text          = answer.answer;
    }

    public bool CheckHide()
    {
        if (isHided || _answer.isCorrect) return false;

        isHided = true;
        _animation.Play(PRE_ANSWER_HIDE_ANIM);
        return true;
    }

    public bool CheckAnim()
    {
        if (_answer.isCorrect)
        {
            _animation.Play(PRE_ANSWER_CORRECT_ANIM);

            if (isChecked) return true;
        }

        if (isChecked)
            _animation.Play(PRE_ANSWER_WRONG_ANIM);

        return false;

    }
}
