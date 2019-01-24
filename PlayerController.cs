using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float jmpHght;
    public float totalMana = 10;
    public float curMana;

    public bool combat = false;
    public bool edge = false;
    public EnemyController curEnemy;
    public GameObject manaBar, healthBar;

    private bool grounded = false;
    private Rigidbody2D rigi;
    private SpriteRenderer manaIn;


    private void Awake()
    {
        //Make it so if there is another player delete it. And make it so current player doesnt get deleted on new scene load
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        rigi = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Scale Mana bar to current mana 
        manaBar.transform.localScale = new Vector2(curMana,1);
        //If the player isn't in combat they can move
        if (!combat)
        {
            //Handles movement
            var movement = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            transform.position += movement * speed * Time.deltaTime;
            //Handles Jumping
            if (Input.GetKey(KeyCode.Space) && !grounded)
            {
                rigi.AddForce(Vector2.up * jmpHght);
                grounded = true;
            }
        }
    }
    //Checks collision entering. For this I check if he touches the ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
        }
        
    }
    //Checks if I have run into a trigger based collider. For this I check the enemies trigger range.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            combat = true;
            curEnemy = collision.gameObject.GetComponent<EnemyController>();
        }
        if (collision.gameObject.tag == "Edge")
        {
            Debug.Log("Edge");
            edge = true;
        }
    }
}
