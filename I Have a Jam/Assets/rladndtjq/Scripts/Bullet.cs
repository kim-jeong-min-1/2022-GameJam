using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] float GranadeDamage;
    [SerializeField] int boundAmount;
    [SerializeField] GameObject GranadeEffect;
    [SerializeField] GameObject HitEffect;
    [HideInInspector] public bool fireBullet = false;
    public int bulletType;

    Rigidbody2D rb;
    GameObject Player;
    public float NormalDamage;
    bool canShoot = true;
    bool isGranadeBomb = false;
    float bound;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.Find("Player");
        bound = boundAmount;
    }

    void Update()
    {
        if (isGranadeBomb == true)
        {
            RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, 1.0f, Vector2.zero ,Mathf.Infinity,LayerMask.GetMask("Enemy"));

            foreach (RaycastHit2D hit2 in hit)
            {
                hit2.collider.GetComponent<Enemy>().currentHp -= GranadeDamage;
            }
            
            isGranadeBomb = false;
            disable();
        }
        if (bound <= 0)
        {
            disable();
        }

        if (this.gameObject.activeSelf)
        {
            if (canShoot == true)
            {
                transform.rotation = Arrow.Instance.transform.rotation;
                rb.AddForce(transform.right * Speed);
                canShoot = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            bound--;
        }

        if (collision.collider.CompareTag("Enemy"))
        {
            bound--;
            collision.gameObject.GetComponent<Enemy>().Hit();
            if (Arrow.Instance.isFireBullet)
            {
                collision.gameObject.GetComponent<Enemy>().isFire = true;
            }

            if(fireBullet == true)
            {
                collision.collider.GetComponent<Enemy>().isFire = true;
            }

            if ((bulletType == (int)BulletType.Granade))
            {
                SoundManager.Instance.PlaySound(SoundEffect.Boom);
                Instantiate(GranadeEffect, transform.position, Quaternion.identity);
                isGranadeBomb = true;
            }
            else
            {
                Instantiate(HitEffect, collision.transform.position, Quaternion.identity);
                collision.collider.GetComponent<Enemy>().currentHp -= NormalDamage;
            }
        }

        if (collision.collider.CompareTag("bulletDisable"))
        {
            if (Arrow.Instance.shootBack == 1)
            {
                Player.transform.DOMove(new Vector3(transform.position.x, Player.transform.position.y, 0), 1f).SetEase(Ease.InOutSine);
            }
            disable();
        }
    }

    private void OnDrawGizmos()
    {
        if (bulletType == (int)BulletType.Granade)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 1.0f);
        }
    }

    void disable()
    {
        canShoot = true;
        isGranadeBomb = false;
        bulletType = 0;
        Arrow.Instance.shootBack++;
        bound = boundAmount;
        ObjectPool.ReturnObject(this.gameObject);
    }
}
