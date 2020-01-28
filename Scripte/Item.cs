using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int healPoint = 20;
    private  Life Life;

    private void Start()
    {
        Life = GameObject.FindGameObjectWithTag("HP").GetComponent<Life>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //ユニティちゃんと衝突した時
        if (col.gameObject.tag == "UnityChan")
        {
            //LifeUpメソッドを呼び出す　引数はhealPoint
            Life.LifeUp(healPoint);
            //アイテムを削除する
            Destroy(gameObject);
        }
    }
}
