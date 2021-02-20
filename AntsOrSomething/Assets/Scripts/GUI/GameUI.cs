using System;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;
    
    public Text TimerText;
    public Text CountdownText;

    private void Start()
    {
        Instance = this;
    }
}