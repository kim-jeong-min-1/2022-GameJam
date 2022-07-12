using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using DG.Tweening;
public class GameManager : MonoBehaviour
{
    public Dictionary<int, GameObject> objectInTile = new Dictionary<int, GameObject>();
    public Vector2[] tilePos = new Vector2[54];
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

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                tilePos[i * 6 + j] = new Vector2(-2.25f + j * 0.86f, 4f - i * 0.850f);
                objectInTile.Add(i * 6 + j, null);
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
        bool isWait = false;
        yield return new WaitForSeconds(0.3f);

        for (int x = 0; x < 6; x++) //좀비 다 내려왔을때
        {
            if (objectInTile[x + 48] != null)
            {
                if (objectInTile[x + 48].activeSelf)
                {
                    isWait = objectInTile[x + 48].TryGetComponent(out Enemy enemy);
                    if (isWait)
                    {
                        enemy.StopCoroutine(enemy.Attack());
                        enemy.StartCoroutine(enemy.Attack());
                    }
                }
            }
        }

        if (isWait)
        {
            yield return new WaitForSeconds(1.0f);
            isWait = false;
        }

        for (int xy = 53; xy > -1; xy--)
        {
            if (objectInTile[xy] != null)
            {
                if (objectInTile[xy].activeSelf == false) //죽었을때
                {
                    objectInTile[xy] = null;
                    isWait = true;
                }
                else // 좀비 한칸 내려가기
                {
                    objectInTile[xy].GetComponent<SortingGroup>().sortingOrder = (int)Mathf.FloorToInt(xy / 6) + 3;
                    if (objectInTile[xy].CompareTag("Enemy"))
                        objectInTile[xy].transform.Find("Hp").GetComponent<SortingGroup>().sortingOrder = (int)Mathf.FloorToInt(xy / 6) + 5;
                    objectInTile[xy].transform.DOMove(new Vector3(objectInTile[xy].transform.position.x, objectInTile[xy].transform.position.y - 0.850f, 0), 0.8f).SetEase(Ease.InOutQuad);
                    objectInTile[xy + 6] = objectInTile[xy];
                    objectInTile[xy] = null;
                    isWait = true;
                }
            }

        }
        if (isWait)
        {
            yield return new WaitForSeconds(0.5f);
            isWait = false;
        }

        int spawnPosX;
        float spawnAmount = Random.Range(1, 5.5f);

        int randomType;
        for (int i = 0; i < spawnAmount; i++) // 좀비 랜덤 생성
        {
            randomType = Random.Range(1, 100);
            spawnPosX = Random.Range(1, 5);
            if (objectInTile[spawnPosX] == null)
            {
                var enemy = ObjectPool.GetObject(ObjectPool.instance.prefebs[1], null);

                if (randomType <= 70)
                    enemy.GetComponent<Enemy>().Enemytype = (int)EnemyType.Normal;
                else if (randomType > 70 && randomType <= 85)
                    enemy.GetComponent<Enemy>().Enemytype = (int)EnemyType.Plague;
                else if (randomType > 85 && randomType <= 100)
                    enemy.GetComponent<Enemy>().Enemytype = (int)EnemyType.Miner;

                enemy.transform.position = new Vector2(tilePos[spawnPosX].x, tilePos[spawnPosX].y + 3);
                enemy.GetComponent<SortingGroup>().sortingOrder = 2;
                enemy.transform.Find("Hp").GetComponent<SortingGroup>().sortingOrder = 4;
                enemy.transform.DOMove(tilePos[spawnPosX], 1.0f).SetEase(Ease.OutBounce);
                objectInTile[spawnPosX] = enemy;
            }
            yield return new WaitForSeconds(0.1f);
        }

        randomType = Random.Range(0, 2);
        spawnPosX = Random.Range(0, 5);
        if (objectInTile[spawnPosX] == null)
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

            objectInTile[spawnPosX] = Item;
            Item.transform.position = new Vector2(tilePos[spawnPosX].x, tilePos[spawnPosX].y + 3);
            Item.transform.DOMove(tilePos[spawnPosX], 1.0f).SetEase(Ease.OutBounce);
        }

        spawnPosX = Random.Range(0, 5);
        if (turn % 5 == 0)
        {
            if (objectInTile[spawnPosX] != null)
            {
                if (objectInTile[spawnPosX].TryGetComponent(out Enemy enemy))
                    enemy.currentHp = 0;
                else if (objectInTile[spawnPosX].TryGetComponent(out Item item))
                    ObjectPool.ReturnObject(item.gameObject);
            }
            var Box = Instantiate(box);
            Box.transform.position = new Vector2(tilePos[spawnPosX].x, tilePos[spawnPosX].y + 3);
            Box.transform.DOMove(tilePos[spawnPosX], 1.0f).SetEase(Ease.OutBounce);
            objectInTile[spawnPosX] = Box;
        }
        yield return new WaitForSeconds(0.5f);


        for (int y = 8; y > 0; y--)
        {
            for (int x = 5; x > -1; x--)
            {
                if (objectInTile[y * 6 + x] != null)
                {
                    if (objectInTile[y * 6 + x].TryGetComponent(out Enemy enemy))
                    {
                        if (enemy.Enemytype == (int)EnemyType.Plague)
                        {
                            objectInTile[y * 6 + x].GetComponent<Enemy>().Shoot();
                            SoundManager.Instance.PlaySound(SoundEffect.Wow);
                            isWait = true;
                        }
                        else if (enemy.Enemytype == (int)EnemyType.Miner)
                        {
                            int posX, posY;
                            do
                            {
                                posX = Mathf.Clamp(x + Random.Range(-5, 5), 0, 5);
                                posY = Mathf.Clamp(y + Random.Range(-5, 5), 1, 8);
                            } while (objectInTile[posY * 6 + posX] != null);
                            objectInTile[y * 6 + x].transform.position = tilePos[posY * 6 + posX];
                            objectInTile[posY * 6 + posX] = objectInTile[y * 6 + x];
                            objectInTile[y * 6 + x] = null;
                            enemy.GetComponent<SortingGroup>().sortingOrder = posY + 3;
                            enemy.transform.Find("Hp").GetComponent<SortingGroup>().sortingOrder = posY + 5;
                            SoundManager.Instance.PlaySound(SoundEffect.Mining);
                        }
                    }
                }
            }

        }
        if (isWait)
        {
            yield return new WaitForSeconds(1f);
            isWait = false;
        }

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