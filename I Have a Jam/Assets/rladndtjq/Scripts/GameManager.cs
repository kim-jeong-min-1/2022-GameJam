using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class GameManager : MonoBehaviour
{
    public Dictionary<Vector2, GameObject> objectInTile = new Dictionary<Vector2, GameObject>();
    public Vector2[,] tile = new Vector2[11, 8];
    public static GameManager instance;
    [HideInInspector]
    public bool isShootEnd = false;
    public bool isSelect = false;
    public bool isBreakSupplies = false;
    bool cyclePlay = false;
    public int EnemyKillCount;
    public int[] EnemyKillCount2 = new int[3];

    [SerializeField] GameObject box;
    [SerializeField] private Text TurnText;
    [SerializeField] private GameObject Supplies;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject fireparticle;
    [SerializeField] Text ScoreText;

    public bool sound;
    public bool music;
    public bool restart;
    public int Score = 0;

    public int turn = 1;
    void Start()
    {

        SoundManager.Instance.PlayBGM();
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                tile[i, j] = new Vector2(-2.3f + j * 0.876f, 4f - i * 0.850f);
                objectInTile.Add(tile[i, j], null);
            }
        }

        StartCoroutine(turnCycle());
        cyclePlay = true;
    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.text = Score.ToString();
        if (isShootEnd && cyclePlay == false && !isSelect)
        {
            StartCoroutine(turnCycle());
            cyclePlay = true;
        }
    }

    IEnumerator turnCycle()
    {
        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < 6; i++) //좀비 다 내려왔을때
        {
            if (objectInTile[tile[8, i]] != null)
            {
                objectInTile[tile[8, i]].GetComponent<Enemy>().Attack();
                Arrow.Instance.currentHp -= 10;
            }
        }
        for (int y = 9; y > -1; y--)
        {
            for (int x = 5; x > -1; x--)
            {
                if (objectInTile[tile[y, x]] != null)
                {
                    if (!objectInTile[tile[y, x]].activeSelf) //좀비 죽었을때
                    {
                        objectInTile[tile[y, x]] = null;
                    }
                    else // 좀비 한칸 내려가기
                    {
                        objectInTile[tile[y, x]].GetComponent<SpriteRenderer>().sortingOrder = y + 2;
                        if (objectInTile[tile[y, x]].CompareTag("Enemy"))
                        {
                            objectInTile[tile[y, x]].transform.Find("Hp").GetComponent<MeshRenderer>().sortingOrder = y + 3;
                        }
                        objectInTile[tile[y, x]].transform.DOMove(new Vector3(objectInTile[tile[y, x]].transform.position.x, objectInTile[tile[y, x]].transform.position.y - 0.850f, 0), 0.8f).SetEase(Ease.InOutQuad);
                        objectInTile[tile[y + 1, x]] = objectInTile[tile[y, x]];
                        objectInTile[tile[y, x]] = null;
                    }
                }
            }
        }
        yield return new WaitForSeconds(1.3f);

        int spawnPosX;
        float spawnAmount = Random.Range(1, 5.5f);

        int randomType;
        for (int i = 0; i < spawnAmount; i++) // 좀비 랜덤 생성
        {
            randomType = Random.Range(1, 100);
            spawnPosX = Random.Range(1, 5);
            if (objectInTile[tile[0, spawnPosX]] == null)
            {
                var enemy = ObjectPool.GetObject(ObjectPool.instance.prefebs[1], null);

                if (randomType <= 70)
                    enemy.GetComponent<Enemy>().Enemytype = (int)EnemyType.Normal;
                else if (randomType > 70 && randomType <= 85)
                    enemy.GetComponent<Enemy>().Enemytype = (int)EnemyType.Plague;
                else if (randomType > 85 && randomType <= 100)
                    enemy.GetComponent<Enemy>().Enemytype = (int)EnemyType.Miner;

                objectInTile[tile[0, spawnPosX]] = enemy;
                enemy.transform.position = new Vector2(tile[0, spawnPosX].x, tile[0, spawnPosX].y + 3);
                enemy.transform.DOMove(tile[0, spawnPosX], 1.0f).SetEase(Ease.OutBounce);
            }

            yield return new WaitForSeconds(0.1f);
        }
        randomType = Random.Range(0, 2);
        spawnPosX = Random.Range(1, 5);
        if (objectInTile[tile[0, spawnPosX]] == null)
        {
            GameObject Item;
            if (randomType == 0)
            {
                Item = ObjectPool.GetObject(ObjectPool.instance.prefebs[2], null);
                Item.GetComponent<Item>().item = (int)ItemType.heal;
            }
            else
            {
                Item = ObjectPool.GetObject(ObjectPool.instance.prefebs[3], null);
                Item.GetComponent<Item>().item = (int)ItemType.bullet;
            }

            objectInTile[tile[0, spawnPosX]] = Item;
            Item.transform.position = new Vector2(tile[0, spawnPosX].x, tile[0, spawnPosX].y + 3);
            Item.transform.DOMove(tile[0, spawnPosX], 1.0f).SetEase(Ease.OutBounce);
        }

        if (turn % 5 == 0)
        {
            for (int i = 0; i < 5; i++)
            {
                if (objectInTile[tile[0, i]] == null)
                {
                    var Box = Instantiate(box);
                    Box.transform.position = tile[0, i];
                    objectInTile[tile[0, i]] = Box;
                    break;
                }
            }
        }
        yield return new WaitForSeconds(1.3f);


        for (int y = 9; y > 0; y--)
        {
            for (int x = 5; x > -1; x--)
            {
                if (objectInTile[tile[y, x]] != null)
                {
                    if (objectInTile[tile[y, x]].CompareTag("Enemy"))
                    {
                        if (objectInTile[tile[y, x]].GetComponent<Enemy>().Enemytype == (int)EnemyType.Plague)
                        {
                            objectInTile[tile[y, x]].GetComponent<Enemy>().Shoot();
                            SoundManager.Instance.PlaySound(SoundEffect.Wow);
                        }
                        else if (objectInTile[tile[y, x]].GetComponent<Enemy>().Enemytype == (int)EnemyType.Miner)
                        {
                            Vector2 randomPos;
                            int posx, posy;
                            do
                            {
                                randomPos = new Vector2(Mathf.Clamp(Random.Range(x - 3, x + 3), 0, 5), Mathf.Clamp(Random.Range(y - 3, y + 3), 1, 8));
                                posx = (int)randomPos.x;
                                posy = (int)randomPos.y;
                                Debug.Log(posx + "/" + posx);
                            } while (objectInTile[tile[posy, posx]] != null);
                            Debug.Log("moveto : " + tile[posy, posx]);
                            objectInTile[tile[y, x]].transform.position = tile[posy, posx];
                            objectInTile[tile[posy, posx]] = objectInTile[tile[y, x]];
                            objectInTile[tile[y, x]] = null;
                            SoundManager.Instance.PlaySound(SoundEffect.Mining);
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(1f);

        if (isBreakSupplies)
        {
            isSelect = true;
            Supplies.SetActive(true);
            Supplies.GetComponent<Supplies>().StartSupplies();
            isBreakSupplies = false;
        }


        Arrow.Instance.canShoot = true;
        isShootEnd = false;
        cyclePlay = false;
        TurnText.text = $"{turn++}";
    }

    public void Sound()
    {
        if (sound == false)
            sound = true;
        else
            sound = false;
    }

    public void Music()
    {
        if (music == false)
            music = true;
        else
            music = false;
    }

    public void Paused()
    {
        canvas.SetActive(true);

    }
    public void PasuedStart()
    {
        canvas.SetActive(false);
    }
}