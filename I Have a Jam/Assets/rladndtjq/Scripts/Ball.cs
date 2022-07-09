using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    [SerializeField] Image[] Appearence;
    Rigidbody2D rb;

    [SerializeField]
    float Speed;

    bool canShoot = true;
    Vector2 mouseDir;
    float dir;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.activeSelf)
        {
            if(canShoot)
            {
                mouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                transform.rotation = Drag.instance.transform.rotation;
                rb.AddForce(transform.right * Speed);
                canShoot = false;
            }
        }
        else
        {
            canShoot = true;
        }
    }
}
