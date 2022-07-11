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
    public int Enemytype;
    [SerializeField] GameObject EnemyBullet;

    TextMesh hptext;
    Animator animator;
    void Start()
    {
        currentHp = Hp + (100 * ((int)(GameManager.instance.turn / 5) + 1));
        hptext = transform.Find("Hp").GetComponent<TextMesh>();
        animator = GetComponent<Animator>();
        hptext.gameObject.GetComponent<MeshRenderer>().sortingOrder = 2;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("Type", Enemytype);

        hptext.text = currentHp.ToString();
        if(currentHp <= 0)
        {
            currentHp = Hp + (100 * ((int)(GameManager.instance.turn / 5) + 1));
            GameManager.instance.EnemyKillCount++;
            GameManager.instance.EnemyKillCount2[Enemytype]++;
            ObjectPool.ReturnObject(this.gameObject);
        }
    }

    public void Shoot()
    {
        var bullet = Instantiate(EnemyBullet, transform.position, transform.rotation);
        bullet.transform.DOMove(Arrow.Instance.transform.position, 1);
    }

    public IEnumerator Attack()
    {
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(0.5f);
        currentHp = 0;
    }

    public void Hit()
    {
        animator.SetTrigger("hit");
        SoundManager.Instance.PlaySound(SoundEffect.E_Hit);
    }
}
