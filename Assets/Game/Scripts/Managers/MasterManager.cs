using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Singletons/MasterManager", fileName = "Master Manager")]
public class MasterManager : SingletonScriptableObject<MasterManager>
{
    [SerializeField] private GameSettings _gameSettings;

    public static GameSettings GameSettings
    {
        get { return Instance._gameSettings; }
    }
}
