using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponentInChildren<Arrow>().currentHp -= 20;
        Destroy(this.gameObject);
    }
}
