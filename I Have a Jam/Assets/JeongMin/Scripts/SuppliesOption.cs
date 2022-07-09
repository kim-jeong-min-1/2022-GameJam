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
    GunPowder
}

public class SuppliesOption : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public OptionType Option;

    private string[] optionNameList = new string[6];
    private string[] optionExplan = new string[6];

    [SerializeField] private Text NameText;
    [SerializeField] private Text ExplanText;
    [SerializeField] private GameObject anotherOption;

    private string tempChar = "";
    private bool isOneClick;
    private bool isCanClick = false;
    public static bool isGunPowder = false;

    private void Start()
    {
        TextSetting();
        Supplies.endSupplies += () =>
        {
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
        if(ExplanText.text == tempChar && isOneClick)
        {
            Buff(Option);
            anotherOption.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InQuad);

            transform.GetComponent<RectTransform>().DOAnchorPosX(0, 1.5f).SetEase(Ease.OutQuad).OnComplete(() => 
            {
                Supplies.endSupplies.Invoke();
            });          
        }
        else
        {
            isOneClick = false;
        }

        if (isCanClick)
        {
            ExplanText.text = optionExplan[(int)Option];
            isOneClick = true;
            tempChar = ExplanText.text;
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
                
                break;
            case OptionType.BulletDmgUP: 
                
                break;
            case OptionType.Food: 
                
                break;
            case OptionType.Grenade: 
                
                break;
            case OptionType.GunPowder:
                isGunPowder = true;
                break;
            case OptionType.Syringe: 
                
                break;
        } 
    }

    public void SetName()
    {
        NameText.text = optionNameList[(int)Option];
    }
    void TextSetting()
    {
        optionNameList[0] = "źâ �߰�";
        optionNameList[1] = "�Ѿ� ��ȭ";
        optionNameList[2] = "�ķ�";
        optionNameList[3] = "�ֻ��";
        optionNameList[4] = "����ź";
        optionNameList[5] = "ȭ��ź";

        optionExplan[0] = "�߻��ϴ� źȯ�� ���� 5 ���δ�.";
        optionExplan[1] = "źȯ�� �������� 10 ��½�Ų��.";
        optionExplan[2] = "ü���� 50 ȸ���Ѵ�.";
        optionExplan[3] = "�ִ�ü���� 10 ��½�Ų��.";
        optionExplan[4] = "ù�߷� ������ ����Ű�� ����ź�� �߻��Ѵ�.";
        optionExplan[5] = "���� ����� �� �ֺ��� ������ ����Ų��.";
    }
    void CanClick()
    {
        isCanClick = true;
    }
}
