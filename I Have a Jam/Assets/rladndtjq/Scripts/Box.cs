using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("bullet"))
        {
            SoundManager.Instance.PlaySound(SoundEffect.Destruction);
            GameManager.instance.isBreakSupplies = true;
            Destroy(this.gameObject);
        }
    }
}
