using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
enum BulletType
{
    Normal,
    Granade,
}
public class Arrow : MonoBehaviour
{
    [SerializeField] Vector2 Offset;
    [SerializeField] Slider HpBar;
    [SerializeField] GameObject Die;
    public static Arrow Instance { get; private set; }

    public float BulletDmg = 10;
    public int granadeBulletAmount;
    public bool isFireBullet = false;
    bool isSoundPlayd = false;
    public int Hp;
    [HideInInspector] public float currentHp;
    public int shootAmount;
    public int shootBack = 0;
    [HideInInspector] public bool canShoot = false;
    public int shootCount = 0;
    [SerializeField] Text hpText;

    LineRenderer lineRenderer;
    GameObject circle;
    SpriteRenderer spriteRenderer;
    Vector2 mouseDir;
    Vector2 spawnPosition;
    float dir;
    void Start()
    {
        Instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        lineRenderer = transform.Find("Line").GetComponent<LineRenderer>();
        circle = transform.Find("Circle").gameObject;
        currentHp = Hp;
    }

    // Update is called once per frame
    void Update()
    {
        HpBar.value = currentHp / Hp;
        hpText.text = $"{currentHp}";
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, Mathf.Infinity, LayerMask.GetMask("Wall") | LayerMask.GetMask("Enemy"));
        Debug.DrawLine(transform.position, hit.point, Color.red);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, hit.point);
        lineRenderer.material.mainTextureOffset = new Vector2(Time.time * Offset.x, Time.time * Offset.y);
        circle.transform.position = hit.point;

        hpText.text = currentHp.ToString();
        if(shootBack >= shootAmount)
        {
            GameManager.instance.isShootEnd = true;
            shootBack = 0;
            shootCount = 0;
            Debug.Log("shootEnd");
        }
        
        if(currentHp <= 0)
        {
            HighScore.Save(GameManager.instance.Score);
            Die.SetActive(true);
            GameManager.instance.isBreakSupplies = false;
            Time.timeScale = 0f;
            gameObject.transform.parent.gameObject.SetActive(false);
        }

        if(canShoot)
        {
            if(Input.GetMouseButton(0) && GameManager.instance.isSelect == false)
            {
                if (isSoundPlayd == false)
                {
                    SoundManager.Instance.PlaySound(SoundEffect.Load);
                    isSoundPlayd = true;
                }
                spriteRenderer.enabled = true;
                lineRenderer.enabled = true;
                circle.GetComponent<SpriteRenderer>().enabled = true;
                mouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                dir = Mathf.Atan2(mouseDir.x, mouseDir.y) * -Mathf.Rad2Deg + 90;
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(dir, 20, 160));
            }

            if(Input.GetMouseButtonUp(0) && GameManager.instance.isSelect == false)
            {
                spawnPosition = transform.position;
                shootBack = 0;
                shootCount = 0;
                StartCoroutine(Shoot());
                spriteRenderer.enabled = false;
                lineRenderer.enabled = false;
                circle.GetComponent<SpriteRenderer>().enabled = false;
                canShoot = false;
                isSoundPlayd = false;
            }
        }
    }

    private IEnumerator Shoot()
    {
        for(int i = 0; i < shootAmount; i++)
        {
            SoundManager.Instance.PlaySound(SoundEffect.Shot, 0.5f);
            var bullet = ObjectPool.GetObject(ObjectPool.instance.prefebs[0].gameObject, null);
            bullet.GetComponent<Bullet>().NormalDamage = BulletDmg;

            if(shootCount < granadeBulletAmount)
                bullet.GetComponent<Bullet>().bulletType = (int)BulletType.Granade;   

            bullet.transform.position = new Vector3(spawnPosition.x,spawnPosition.y, 0);
            shootCount++;
            
            yield return new WaitForSeconds(0.1f);
        }
    }
    
   
    
}
