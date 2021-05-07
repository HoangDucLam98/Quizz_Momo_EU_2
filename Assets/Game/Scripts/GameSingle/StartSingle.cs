using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSingle : MonoBehaviour
{
    [SerializeField] private Button _button, quitBtn, aboutBtn, closeBtn;
    [SerializeField] private GameObject about;

    private bool isShowAbout;

    public bool IsShowAbout
    {
        get => isShowAbout;
        set
        {
            isShowAbout = value;
            about.SetActive(isShowAbout);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        IsShowAbout = false;
        
        _button.onClick.AddListener(StartGame);
        aboutBtn.onClick.AddListener(() => IsShowAbout = !isShowAbout);
        quitBtn.onClick.AddListener(() => Application.Quit());
        closeBtn.onClick.AddListener(() => IsShowAbout = !isShowAbout);
    }

    void StartGame()
    {
        this.Hide();
        GamePlaySingle.Ins.Reload();
    }
}
