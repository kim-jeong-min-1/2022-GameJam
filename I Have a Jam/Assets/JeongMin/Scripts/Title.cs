using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Title : MonoBehaviour
{
    [SerializeField] private GameObject Logo;
    [SerializeField] private Text TouchText;
    private bool isLogoMove;

    void Start()
    {
        SoundManager.Instance.PlayBGM();
        TouchTextFade();
        LogoMove();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SceneManager.LoadScene("InGame");
            SoundManager.Instance.PlaySound(SoundEffect.Click);
        }
    }

    void LogoMove()
    {
        isLogoMove = (Logo.transform.position.y == 1.5f) ? true : false;
        StartCoroutine(LogoMoveSystem(isLogoMove));
    }
    private IEnumerator LogoMoveSystem(bool isMove)
    {
        if (isMove)
        {
            Logo.transform.DOMoveY(2f, 1.3f).SetEase(Ease.InOutQuad);
        }
        else
        {
            Logo.transform.DOMoveY(1.5f, 1.3f).SetEase(Ease.InOutQuad);
        }
        yield return new WaitForSeconds(1.5f);
        LogoMove();
    }

    void TouchTextFade()
    {
        StartCoroutine(FadeOutText());
    }
    private IEnumerator FadeInText()
    {
        while (TouchText.color.a < 1)
        {
            float Fade = TouchText.color.a + 0.01f;
            TouchText.color = new Color(1, 1, 1, Fade);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(FadeOutText());
    }
    private IEnumerator FadeOutText()
    {
        while (TouchText.color.a > 0)
        {
            float Fade = TouchText.color.a - 0.01f;
            TouchText.color = new Color(1, 1, 1, Fade);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(FadeInText());
    }
}
