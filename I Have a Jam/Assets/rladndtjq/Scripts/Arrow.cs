using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Arrow : MonoBehaviour
{
    public static Arrow Instance { get; private set; }
    [SerializeField] int Hp;
    [HideInInspector] public float currentHp;
    [SerializeField] int shootAmount;
    [HideInInspector] public int shootBack = 0;
    [HideInInspector] public bool canShoot = true;
    [SerializeField] TextMesh hpText;

    LineRenderer lineRenderer;
    SpriteRenderer spriteRenderer;
    Vector2 mouseDir;
    float dir;
    void Start()
    {
        Instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        lineRenderer = transform.Find("Line").GetComponent<LineRenderer>();
        currentHp = Hp;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, Mathf.Infinity, LayerMask.GetMask("Wall"));
        Debug.DrawLine(transform.position, hit.point, Color.red);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, hit.point);

        hpText.text = currentHp.ToString();
        if(shootBack == shootAmount)
        {
            GameManager.instance.isShootEnd = true;
            shootBack = 0;
            Debug.Log("shootEnd");
        }
        if(canShoot)
        {
            if(Input.GetMouseButton(0))
            {
                spriteRenderer.enabled = true;
                lineRenderer.enabled = true;
                mouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                dir = Mathf.Atan2(mouseDir.x, mouseDir.y) * -Mathf.Rad2Deg + 90;
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(dir, 20, 160));
            }

            if(Input.GetMouseButtonUp(0))
            {
                StartCoroutine(Shoot());
                spriteRenderer.enabled = false;
                lineRenderer.enabled = false;
                canShoot = false;
            }
        }
    }

    private IEnumerator Shoot()
    {
        for(int i = 0; i < shootAmount; i++)
        {
            var bullet = ObjectPool.GetObject(ObjectPool.instance.prefebs[0].gameObject, null);
            bullet.transform.position = new Vector3(transform.position.x,transform.position.y + 0.5f, 0);
            yield return new WaitForSeconds(0.1f);
        }
    }
    
   
    
}
