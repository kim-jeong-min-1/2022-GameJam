using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    private string tempChar = "";
    private bool isOneClick;


    private void Start()
    {
        TextSetting();
        Supplies.endSupplies += () =>
        {
            NameText.text = "";
            ExplanText.text = "";
        };
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(ExplanText.text == tempChar && isOneClick)
        {
            Buff(Option);
            Supplies.endSupplies.Invoke();
        }
        else
        {
            isOneClick = false;
        }

        ExplanText.text = optionExplan[(int)Option];
        isOneClick = true;
        tempChar = ExplanText.text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    void Buff(OptionType supplies)
    {
        switch (supplies)
        {
            case OptionType.BulletCountUP:
                print("1"); break;
            case OptionType.BulletDmgUP: 
                print("2"); break;
            case OptionType.Food: 
                print("3"); break;
            case OptionType.Grenade: 
                print("4"); break;
            case OptionType.GunPowder: 
                print("5"); break;
            case OptionType.Syringe: 
                print("6"); break;
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
}
