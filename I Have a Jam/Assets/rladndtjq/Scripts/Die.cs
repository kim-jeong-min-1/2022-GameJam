using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Die : MonoBehaviour
{
    [SerializeField] GameObject[] texts;

    private void Start()
    {
        for(int i = 0; i < texts.Length - 1; i++)
        {
            texts[i].GetComponent<Text>().text = GameManager.instance.EnemyKillCount2[i].ToString();
        }
        texts[3].GetComponent<Text>().text = GameManager.instance.EnemyKillCount.ToString();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Title");
        }
    }
}
