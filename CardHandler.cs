using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{

    public string cardName = "BOLT";
    public Sprite sprite;
    public int dmg, cost;
    public float crit = 0f;
    public Text text;

    //Sets the name to the name assigned
    void Awake()
    {
        this.gameObject.name = cardName;

        text.text = cardName;
        float x = this.transform.position.x, y = this.transform.position.y;
        text.rectTransform.localPosition = new Vector3(x - 2.5f, y -5f, 0);
    }
}
