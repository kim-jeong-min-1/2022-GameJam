using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour
{
    public static Drag instance { get; private set; }

    [SerializeField] float shootAmount;
    bool canShoot = true;
    SpriteRenderer spriteRenderer;
    Vector2 mouseDir;
    float dir;
    void Start()
    {
        instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(canShoot)
        {
            if(Input.GetMouseButton(0))
            {
                spriteRenderer.enabled = true;
                mouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                dir = Mathf.Atan2(mouseDir.x, mouseDir.y) * -Mathf.Rad2Deg + 90;
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(dir, 20, 160));
            }

            if(Input.GetMouseButtonUp(0))
            {
                StartCoroutine(Shoot());
                spriteRenderer.enabled = false;
                canShoot = false;
            }
        }
    }

    private IEnumerator Shoot()
    {
        for(int i = 0; i < shootAmount; i++)
        {
            ObjectPool.GetObject(ObjectPool.instance.prefebs[0].gameObject, null);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        canShoot = true;
    }
    
   
    
}
