using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Dictionary<Vector2, GameObject> objectInTile = new Dictionary<Vector2, GameObject>();
    Vector2[,] tile = new Vector2[11, 8];
    public static GameManager instance;
    [HideInInspector]
    public bool isShootEnd = false;
    bool cyclePlay = false;
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        for (int i = 0; i < 11; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                tile[i, j] = new Vector2(-2.134f + j * 0.876f, 3.631f + i * 0.850f);
                objectInTile.Add(tile[i,j], null);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isShootEnd && cyclePlay == false)
        {
            StartCoroutine(turnCycle());
            cyclePlay = true;
        }
    }

    IEnumerator turnCycle()
    {
        yield return new WaitForSeconds(0.3f);
        for (int y = 9; y > -1; y--)
        {
            for (int x = 5; x > -1; x--)
            {
                if (objectInTile[tile[y, x]] != null)
                {
                    if (!objectInTile[tile[y, x]].activeSelf)
                    {
                        objectInTile[tile[y, x]] = null;
                    }
                    else
                    {
                        objectInTile[tile[y, x]].transform.position = new Vector2(objectInTile[tile[y, x]].transform.position.x, objectInTile[tile[y, x]].transform.position.y - 0.876f);
                        objectInTile[tile[y + 1, x]] = objectInTile[tile[y, x]];
                        objectInTile[tile[y, x]] = null;
                    }
                }
            }
        }

        int x2;
        float random = Random.Range(1, 5.5f);
        for (int i = 0; i < random; i++)
        {
            x2 = Random.Range(0, 4);
            if (objectInTile[tile[0, x2]] == null)
            {
                var enemy = ObjectPool.GetObject(ObjectPool.instance.prefebs[1], null);
                objectInTile[tile[0,x2]] = enemy;
                enemy.transform.position = tile[0, x2];
            }
        }

        for(int i = 0; i < 6; i++)
        {
            if(objectInTile[tile[9, i]] != null)
            {
                objectInTile[tile[9, i]].GetComponent<Enemy>().currentHp = 0;
                Arrow.Instance.currentHp -= 10;
            }
        }
        yield return new WaitForSeconds(1f);
        Arrow.Instance.canShoot = true;
        isShootEnd = false;
        cyclePlay = false;
    }
}
