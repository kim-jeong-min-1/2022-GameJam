using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Supplies : MonoBehaviour
{
    public static System.Action endSupplies;
    
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
        SuppliesObj.SetActive(true);
        anim.SetBool("isEnd", false);
    }
    public void EndSupplies()
    {
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
                int rand = Random.Range(0, SuppliesDatas.Count);
                Option[i].transform.GetChild(0).GetComponent
                    <Image>().sprite = SuppliesDatas[rand];

                OptionNumber[i] = rand;
                yield return new WaitForSeconds(0.03f);
            }         
        }

        for (int i = 0; i < Option.Count; i++)
        {
            Option[i].GetComponent<SuppliesOption>().Option = (OptionType)OptionNumber[i];
            Option[i].GetComponent<SuppliesOption>().SetName();
        }
    }
}
