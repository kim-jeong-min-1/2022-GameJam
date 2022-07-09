using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int Hp;
    [HideInInspector] public float currentHp;

    TextMesh hptext;
    void Start()
    {
        currentHp = Hp;
        hptext = transform.Find("Hp").GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        hptext.text = currentHp.ToString();
        if(currentHp <= 0)
        {
            currentHp = Hp;
            ObjectPool.ReturnObject(this.gameObject);
        }
    }
}
