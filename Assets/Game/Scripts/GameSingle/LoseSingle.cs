using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseSingle : MonoBehaviour
{
    [SerializeField] private Button _button, backBtn;
    [SerializeField] private GameObject startPannel;
    
    // Start is called before the first frame update
    void Start()
    {
        _button.onClick.AddListener((() =>
        {
            this.Hide();
            GamePlaySingle.Ins.Reload();
        }));
        
        backBtn.onClick.AddListener(() =>
        {
            this.Hide();
            startPannel.Show();
        });
    }
}
