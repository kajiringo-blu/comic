using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;
    public int speed = -3;
    public GameObject exploasion;
    public GameObject item;
    public int attackPoint = 10;
    private Life Life;
    private const string MAIN_CAMERA_TAG_NAME = "MainCamera";
    private bool _isRendered = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        Life = GameObject.FindGameObjectWithTag("HP").GetComponent<Life>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isRendered)
        {
            rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
        }
        if (gameObject.transform.position.y < Camera.main.transform.position.y - 8 ||
            gameObject.transform.position.x < Camera.main.transform.position.x - 10)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_isRendered)
        {

            if (col.tag == "Bullet")
            {
                Destroy(gameObject);
                Instantiate(exploasion, transform.position, transform.rotation);

                //四分の一の確率で回復アイテムを落とす
                if (Random.Range(0, 4) == 0)
                {
                    Instantiate(item, transform.position, transform.rotation);
                }
            }
        }
    }
    private void OnWillRenderObject()
    {
        if (Camera.current.tag == MAIN_CAMERA_TAG_NAME)
        {
            _isRendered = true;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //UnityChanとぶつかった時
        if (col.gameObject.tag == "UnityChan")
        {
            //LifeScriptのLifeDownメソッドを実行
            Life.LifeDown(attackPoint);
        }
    }
}
