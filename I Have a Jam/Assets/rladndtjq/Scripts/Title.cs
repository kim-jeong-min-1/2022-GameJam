using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Title : MonoBehaviour
{
    [SerializeField] private GameObject Logo;
    [SerializeField] private Text TouchText;

    private Color touchTextColor;
    private bool isLogoMove;

    void Start()
    {
        touchTextColor = TouchText.GetComponent<Text>().color;

        TouchTextFade();
        LogoMove();
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
            Logo.transform.DOMoveY(2f, 1.3f).SetEase(Ease.OutQuad);
        }
        else
        {
            Logo.transform.DOMoveY(1.5f, 1.3f).SetEase(Ease.OutQuad);
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
        while (touchTextColor.a < 1)
        {
            float Fade = touchTextColor.a += 0.01f;
            touchTextColor = new Color(0, 0, 0, Fade);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(FadeOutText());
    }
    private IEnumerator FadeOutText()
    {
        while (touchTextColor.a > 0)
        {
            float Fade = touchTextColor.a -= 0.01f;
            touchTextColor = new Color(0, 0, 0, Fade);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(FadeInText());
    }
}
