using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Supplies : MonoBehaviour
{
    public static System.Action endSupplies;

    [SerializeField] private Vector2[] previousPos;
    [SerializeField] private List<Sprite> SuppliesDatas = new List<Sprite>();
    [SerializeField] private List<GameObject> Option = new List<GameObject>();
    [SerializeField] private GameObject Options;
    [SerializeField] private GameObject SuppliesObj;
    private Animator anim => GetComponent<Animator>();

    private int[] OptionNumber = new int[2];

    private void Start()
    {
        endSupplies += () => { Options.SetActive(false); };
        endSupplies += () => { anim.SetBool("isEnd", true); };
    }

    public void StartSupplies()
    {
        StartSetting();
        anim.SetBool("isEnd", false);
    }
    public void EndSupplies()
    {
        for (int i = 0; i < Option.Count; i++)
        {
            Option[i].GetComponent<SuppliesOption>().isOneclick = false;
        }
        SuppliesObj.SetActive(false);
    }

    public void RandomSupplies()
    {
        Options.SetActive(true);
        StartCoroutine(SuppliesRoulette());
    }
    private IEnumerator SuppliesRoulette()
    {
        float StartTime = Time.time;
        float CurTime = 0;
        while (CurTime < StartTime + 2f)
        {
            CurTime = Time.time;

            for (int i = 0; i < Option.Count; i++)
            {
                SoundManager.Instance.PlaySound(SoundEffect.Random);
                int rand = Random.Range(1, 101);
                Option[i].transform.GetChild(0).GetComponent
                    <Image>().sprite = SuppliesDatas[GetOption(rand)];

                OptionNumber[i] = GetOption(rand);
                yield return new WaitForSeconds(0.03f);
            }         
        }

        for (int i = 0; i < Option.Count; i++)
        {
            Option[i].GetComponent<SuppliesOption>().Option = (OptionType)OptionNumber[i];
            Option[i].GetComponent<SuppliesOption>().SetName();
        }
    }

    private int GetOption(int rand)
    {
        if (rand <= 20)
        {
            return 0;
        }
        else if (rand <= 40)
        {
            return 1;
        }
        else if (rand <= 60)
        {
            return 2;
        }
        else if(rand <= 80)
        {
            return 3;
        }
        else if(rand <= 90)
        {
            return 4;
        }
        else
        {
            if (SuppliesOption.isGunPowder == true) return Random.Range(0, 5);
            return 5;
        }
    }
    private void StartSetting()
    {
        for (int i = 0; i < Option.Count; i++)
        {
            Option[i].GetComponent<RectTransform>().anchoredPosition = previousPos[i];
            Option[i].transform.localScale = Vector3.one;
        }
    }
}
