using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{

    public float speed = 5f;
    public float jumppower = 700;
    public LayerMask groundLayer;
    public GameObject mainCamera;
    public GameObject bullet;
    public Life Life;
    private new Rigidbody2D rigidbody2D;
    private Animator anim;
    private bool isGrounded;
    private new Renderer renderer;
    private bool gameClear;
    public Text clearText;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        isGrounded = Physics2D.Linecast(
            transform.position + transform.up * 1,
            transform.position - transform.up * 0.05f,
            groundLayer);

        if (!gameClear)
        {
            //if (CrossPlatformInputManager.GetButtonDown("Jump"))
            if (Input.GetKeyDown("space"))
            {
                if (isGrounded)
                {
                    anim.SetBool("Dash", false);
                    anim.SetTrigger("Jump");

                    isGrounded = false;

                    rigidbody2D.AddForce(Vector2.up * jumppower);
                }
            }
        }
        float velY = rigidbody2D.velocity.y;

        bool isJumping = velY > 0.1f ? true : false;
        bool isFalling = velY < -0.1f ? true : false;

        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isFalling", isFalling);

        if (!gameClear)
        {
            //if(CrossPlatformInputManager.GetButtonDown("v"))
            if (Input.GetKeyDown("v"))
            {
                anim.SetTrigger("Shot");
                Instantiate(bullet, transform.position + new Vector3(0f, 0.9f, 0f), transform.rotation);
            }
            //現在のカメラの位置から8低くした位置を下回った時
            if (gameObject.transform.position.y < Camera.main.transform.position.y - 8)
            {
                //LifeScriptのGameOverメソッドを実行する
                Life.GameOver();
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameClear)
        {
            /*int x = 0;

            if (CrossPlatformInputManager.GetAxisRaw("Horizontal") >= 0.01f)
            {
                x = 1;
            }
            else if(CrossPlatformInputManager.GetAxisRaw("Horizotal") <= -0.01f)
            {
                x = -1;
            }*/
            float x = Input.GetAxisRaw("Horizontal");

            if (System.Math.Abs(x) > 0)
            {
                rigidbody2D.velocity = new Vector2(x * speed, rigidbody2D.velocity.y);

                Vector2 temp = transform.localScale;
                temp.x = x;
                transform.localScale = temp;

                anim.SetBool("Dash", true);

                if (transform.position.x > mainCamera.transform.position.x - 4)
                {
                    Vector3 cameraPos = mainCamera.transform.position;
                    cameraPos.x = transform.position.x + 4;
                    mainCamera.transform.position = cameraPos;
                }
                Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
                Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

                Vector2 pos = transform.position;
                pos.x = Mathf.Clamp(pos.x, min.x + 0.5f, max.x);
                transform.position = pos;
            }
            else
            {
                rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
                anim.SetBool("Dash", false);
            }
        }
        else
        {
            clearText.enabled = true;
            anim.GetBool("Dash");
            rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
            Invoke("CallTitle", 5);
        }
    }
    

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            StartCoroutine("Damage");
        }
    }

    IEnumerator Damage()
    { 
        gameObject.layer = LayerMask.NameToLayer("PlayerDamage");

        int count = 10;
        while (count > 0)
        {
            //透明にする
            renderer.material.color = new Color(1, 1, 1, 0);
            //0.05秒待つ
            yield return new WaitForSeconds(0.05f);
            //元に戻す
            renderer.material.color = new Color(1, 1, 1, 1);
            //0.05秒待つ
            yield return new WaitForSeconds(0.05f);
            count--;
        }
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "ClearZone")
        {
            gameClear = true;
        }
    }

    void CallTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
