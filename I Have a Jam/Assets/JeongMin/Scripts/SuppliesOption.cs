using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public enum OptionType
{
    BulletCountUP,
    BulletDmgUP,
    Food,
    Syringe,
    Grenade,
    GunPowder,
    nonCheck
}

public class SuppliesOption : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public OptionType Option = OptionType.nonCheck;

    private string[] optionNameList = new string[7];
    private string[] optionExplan = new string[7];

    [SerializeField] private Text NameText;
    [SerializeField] private Text ExplanText;
    [SerializeField] private GameObject anotherOption;

    [HideInInspector]
    public bool isOneclick = false;
    [HideInInspector]
    public bool isCanClick = false;
    public static bool isGunPowder = false;

    private void Start()
    {
        TextSetting();
        Supplies.endSupplies += () =>
        {
            GameManager.instance.isSelect = false;
            NameText.text = "";
            ExplanText.text = "";
            isCanClick = false;
        };
    }

    private void OnEnable()
    {
        Invoke("CanClick", 2f);     
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (Option != OptionType.nonCheck && isOneclick)
        {
            //StopCoroutine(Choose());
            StartCoroutine(Choose());
            isOneclick = false;
        }
        if (isCanClick) 
        {
            ExplanText.text = optionExplan[(int)Option];
            anotherOption.GetComponent<SuppliesOption>().isOneclick = false;
            isOneclick = true;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 0.95f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }

    void Buff(OptionType supplies)
    {
        switch (supplies)
        {
            case OptionType.BulletCountUP:
                Arrow.Instance.shootAmount += 3;
                break;
            case OptionType.BulletDmgUP:
                Arrow.Instance.BulletDmg += 5;
                break;
            case OptionType.Food:
                Arrow.Instance.currentHp += 50;
                if(Arrow.Instance.currentHp > Arrow.Instance.Hp)
                {
                    Arrow.Instance.currentHp = Arrow.Instance.Hp;
                }
                break;
            case OptionType.Grenade:
                Arrow.Instance.granadeBulletAmount++;
                break;
            case OptionType.GunPowder:
                Arrow.Instance.isFireBullet = true;
                isGunPowder = true;
                break;
            case OptionType.Syringe:
                Arrow.Instance.Hp += 20;
                break;
        } 
    }

    public void SetName()
    {
        NameText.text = optionNameList[(int)Option];
    }
    void TextSetting()
    {
        optionNameList[0] = "???? ????";
        optionNameList[1] = "???? ????";
        optionNameList[2] = "????";
        optionNameList[3] = "??????";
        optionNameList[4] = "??????";
        optionNameList[5] = "??????";

        optionExplan[0] = "???????? ?????? ???? 3 ??????.";
        optionExplan[1] = "?????? ???????? 5 ??????????.";
        optionExplan[2] = "?????? 50 ????????.";
        optionExplan[3] = "?????????? 20 ??????????.";
        optionExplan[4] = "?????? ?????? ???????? ???????? ????????.";
        optionExplan[5] = "???? ?????? ?? ?????? ?????? ????????.";
    }
    void CanClick()
    {
        isCanClick = true;
    }

    IEnumerator Choose()
    {
        Buff(Option);
        anotherOption.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InQuad);

        Option = OptionType.nonCheck;
        anotherOption.GetComponent<SuppliesOption>().Option = OptionType.nonCheck;
        gameObject.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 1.5f).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(1.5f);
        Supplies.endSupplies();
    }
}
