using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{
    public string GameVersion = "0.0.0";
    [SerializeField]
    private string _nickName = "Punfish";

    public string NickName
    {
        get
        {
            int value = Random.Range(0, 9999);
            return _nickName + value;
        }
    }
}
