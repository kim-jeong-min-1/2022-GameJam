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
        optionNameList[0] = "탄창 추가";
        optionNameList[1] = "총알 강화";
        optionNameList[2] = "식량";
        optionNameList[3] = "주사기";
        optionNameList[4] = "수류탄";
        optionNameList[5] = "화약탄";

        optionExplan[0] = "발사하는 탄환의 수를 5 높인다.";
        optionExplan[1] = "탄환의 데미지를 10 상승시킨다.";
        optionExplan[2] = "체력을 50 회복한다.";
        optionExplan[3] = "최대체력을 10 상승시킨다.";
        optionExplan[4] = "첫발로 폭발을 일으키는 수류탄을 발사한다.";
        optionExplan[5] = "적이 사망할 때 주변에 폭발을 일으킨다.";
    }
}
