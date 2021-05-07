using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSingle : MonoBehaviour
{
    private const string card1 = "Chúc mừng bạn đã nhận được vé bốc thăm may mắn";
    private const string card2 = "Chúc mừng bạn đã nhận IPhone 12 Promax";
    
    [SerializeField] private Button _button;
    [SerializeField] private Text _text;

    private bool isEnd;
    
    // Start is called before the first frame update
    void Start()
    {
        _button.onClick.AddListener((() =>
        {
            this.Hide();
            
            if( isEnd )
                GamePlaySingle.Ins.Reload();
            else
                GamePlaySingle.Ins.Continue();
        }));
    }

    public void OnShow(bool isEnd = false)
    {
        this.Show();
        
        AudioManager.Ins.PlaySound(SoundType.Win);
        
        this.isEnd = isEnd;
        
//        if (!isEnd)
//            _text.text = card1;
//        else
//            _text.text = card2;
    }
}
