using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject particle;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(particle,collision.transform.position,Quaternion.identity);
        SoundManager.Instance.PlaySound(SoundEffect.P_Hit);
        collision.gameObject.GetComponentInChildren<Arrow>().currentHp -= 20;
        Destroy(this.gameObject);
    }
}
