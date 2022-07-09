using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance { get; private set; }
    public int spawnAmount;
    public GameObject[] prefebs;
    Dictionary<string, Queue<GameObject>> poolingObject = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        for (int i = 0; i < prefebs.Length; i++)
        {
            var poolingObj = new GameObject(prefebs[i].name + "Pool");
            poolingObj.transform.SetParent(this.gameObject.transform);
        }
    }
    private void Start()
    {
        Initialized();
    }

    private void Initialized()
    {
        for (int i = 0; i < prefebs.Length; i++)
        {
            for (int j = 0; j < spawnAmount; j++)
            {
                if (!poolingObject.ContainsKey(prefebs[i].name + "Pool"))
                {
                    Queue<GameObject> Queue = new Queue<GameObject>();
                    poolingObject.Add(prefebs[i].name + "Pool", Queue);
                }
                var newObj = CreateObject(prefebs[i]);
                poolingObject[prefebs[i].name + "Pool"].Enqueue(newObj);
            }
            Debug.Log(poolingObject[prefebs[i].name + "Pool"].Count);
        }
    }
    public GameObject CreateObject(GameObject obj)
    {
        GameObject newObj = Instantiate(obj, transform.Find(obj.name + "Pool").transform);
        newObj.name = newObj.name.Substring(0, newObj.name.IndexOf("(Clone)"));
        newObj.SetActive(false);
        return newObj;
    }

    public static GameObject GetObject(GameObject Object, Transform parent)
    {
        if (instance.poolingObject.ContainsKey(Object.name + "Pool"))
        {
            if (instance.poolingObject[Object.name + "Pool"].Count < 1)
            {
                GameObject newObj = instance.CreateObject(Object);
                newObj.SetActive(true);
                newObj.transform.SetParent(parent);
                return newObj;
            }
            else
            {
                var obj = instance.poolingObject[Object.name + "Pool"].Dequeue();
                obj.SetActive(true);
                obj.transform.SetParent(parent);
                return obj;
            }
        }
        else
        {
            return null;
        }
    }

    public static void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(instance.transform.Find(obj.name + "Pool").transform);
        instance.poolingObject[obj.name + "Pool"].Enqueue(obj);
    }
}
