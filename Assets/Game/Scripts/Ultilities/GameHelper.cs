using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GameHelper
{
    public static void AddListener(this Button button, System.Action callback)
    {
        AudioManager.Ins.PlaySound(SoundType.Click);
        button.onClick.AddListener(() => callback());
    }
}
