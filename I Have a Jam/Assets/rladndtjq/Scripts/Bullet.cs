using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] float Damage;

    Rigidbody2D rb;
    float boundAmount = 5;
    bool canShoot = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(boundAmount <= 0)
        {
            disable();
        }

        if(this.gameObject.activeSelf)
        {
            if(canShoot == true)
            {
                transform.rotation = Arrow.Instance.transform.rotation;
                rb.AddForce(transform.right * Speed);
                canShoot = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Wall"))
        {
            boundAmount--;
        }
        
        if(collision.collider.CompareTag("Enemy"))
        {
            boundAmount--;
            collision.gameObject.GetComponent<Enemy>().currentHp -= Damage;
        }

        if(collision.collider.CompareTag("bulletDisable"))
        {
            disable();
        }
    }

    void disable()
    {
        canShoot = true;
        boundAmount = 5;
        Arrow.Instance.shootBack++;
        ObjectPool.ReturnObject(this.gameObject);
    }
}
