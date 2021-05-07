using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
}

public static class GameObjectExtension
{
    public static void Show(this MonoBehaviour m)
    {
        m.gameObject.SetActive(true);
    }

    public static void Hide(this MonoBehaviour m)
    {
        m.gameObject.SetActive(false);
    }

    public static void Show(this GameObject m)
    {
        m.SetActive(true);
    }

    public static void Hide(this GameObject m)
    {
        m.SetActive(false);
    }
}
