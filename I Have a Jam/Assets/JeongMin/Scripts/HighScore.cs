using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    [SerializeField] Text scoreText;
    private static int highScore;
    void Start()
    {
        Load();
        scoreText.text = $"{highScore}";
    }

    private void Load()
    {
        highScore = PlayerPrefs.GetInt("Score");
    }

    public static void Save(int num)
    {
        if(num > highScore)
        {
            PlayerPrefs.SetInt("Score", num);

        }
    }
}
