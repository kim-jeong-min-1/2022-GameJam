using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

enum EnemyType
{
    Normal,
    Plague,
    Miner
}
public class Enemy : MonoBehaviour
{
    [SerializeField] int Hp;
    [HideInInspector] public float currentHp;
    [HideInInspector] public int Enemytype;
    [SerializeField] GameObject EnemyBullet;

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

    public void Shoot()
    {
        var bullet = Instantiate(EnemyBullet, transform.position, transform.rotation);
        bullet.transform.DOMove(Arrow.Instance.transform.position, 1);
    }
}
