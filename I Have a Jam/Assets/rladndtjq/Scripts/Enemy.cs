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
    [SerializeField] GameObject bloodParticle;
    [SerializeField] GameObject FireEffect;
    Vector2 enemyPosition;
    public bool isFire = false;
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
        enemyPosition = new Vector2(transform.position.x + 0.15f, transform.position.y - 0.35f);
        animator.SetInteger("Type", Enemytype);

        hptext.text = currentHp.ToString();
        if (currentHp <= 0)
        {
            currentHp = Hp + (100 * ((int)(GameManager.instance.turn / 5) + 1));
            if (isFire)
            {
                Instantiate(FireEffect, transform.position, Quaternion.identity);
                RaycastHit2D[] horizontalHit = Physics2D.LinecastAll(new Vector2(enemyPosition.x - 0.85f, enemyPosition.y), new Vector2(enemyPosition.x + 0.85f, enemyPosition.y), LayerMask.GetMask("Enemy"));
                RaycastHit2D[] verticalHit = Physics2D.LinecastAll(new Vector2(enemyPosition.x, enemyPosition.y - 0.85f), new Vector2(enemyPosition.x, enemyPosition.y + 0.85f), LayerMask.GetMask("Enemy"));

                foreach (RaycastHit2D hit in horizontalHit)
                    hit.collider.GetComponent<Enemy>().currentHp -= 10;

                foreach (RaycastHit2D hit in verticalHit)
                    hit.collider.GetComponent<Enemy>().currentHp -= 10;
                isFire = false;
            }
            GameManager.instance.EnemyKillCount++;
            GameManager.instance.EnemyKillCount2[Enemytype]++;
            GameManager.instance.Score += 100;
            ObjectPool.ReturnObject(this.gameObject);
        }
    }

    public void Shoot()
    {
        var bullet = Instantiate(EnemyBullet, transform.position, transform.rotation);
        bullet.transform.DOMove(Arrow.Instance.transform.position, 1);
        animator.Play("Plague-attack");
    }

    public IEnumerator Attack()
    {
        if (Enemytype == 0)
            animator.Play("Normal-attack");
        else if (Enemytype == 1)
            animator.Play("Plague-attack");
        else
            animator.Play("Worker-attack");
        yield return new WaitForSeconds(0.7f);
        currentHp = 0;
        Arrow.Instance.currentHp -= 30;
        SoundManager.Instance.PlaySound(SoundEffect.P_Hit);
        Instantiate(bloodParticle, Arrow.Instance.transform.position, Quaternion.identity);
    }

    public void Hit()
    {
        if (Enemytype == 0)
            animator.Play("Normal-hit");
        else if (Enemytype == 1)
            animator.Play("Plague-hit");
        else
            animator.Play("Worker-hit");
        SoundManager.Instance.PlaySound(SoundEffect.E_Hit);
    }

    private void OnDrawGizmos()
    {
        if (isFire)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(new Vector2(enemyPosition.x - 0.85f, enemyPosition.y), new Vector2(enemyPosition.x + 0.85f, enemyPosition.y));
            Gizmos.DrawLine(new Vector2(enemyPosition.x, enemyPosition.y - 0.85f), new Vector2(enemyPosition.x, enemyPosition.y + 0.85f));
        }
    }
}
