using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hp : MonoBehaviour
{
    RectTransform position;
    Vector2 playerpos;
    void Update()
    {
        playerpos = Arrow.Instance.transform.position;
        position.position = Camera.main.WorldToScreenPoint(new Vector2(playerpos.x, playerpos.y - 1));
    }
}
