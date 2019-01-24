using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public GameObject hand;
    public CardHandler card;
    public Scene[] scenes;

    public CardHandler[] deck = new CardHandler[5];

    private CardHandler bolt,fireBall;
    private float timeStart;
    private bool turnStart = false;
    // Start is called before the first frame update

    private void Awake()
    {
        //Handles Object loading between scenes same as the player
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameController");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }


    void Start()
    {
        player = player.GetComponent<PlayerController>();
        bolt = card.GetComponent<CardHandler>();
        player.curMana = player.totalMana;
        bolt.dmg = 1;
        bolt.cost = 1;
        bolt.crit = 50f;
        deck[0] = bolt;
    }

    // Update is called once per frame
    void Update()
    {

        //Starts Combat spawns cards and deals with enemy health.
        if (player.combat)
        {
            if (!turnStart)
            {
                turnStart = true;
                foreach (CardHandler c in deck)
                {
                    int prevX = 1;
                    Instantiate(c, new Vector3(hand.transform.position.x + prevX,hand.transform.position.y, 0), Quaternion.identity, hand.transform);
                    prevX += 2;
                }
            }
            //Check if the button has been pressed. If so cast a ray, and if it hits a card do.
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);

                if (hit.collider.gameObject.tag == "Card")
                {
                    //If it hits a card check if you can cast the card, if so do a crit check, then damage current enemy
                    CardHandler crd = hit.collider.gameObject.GetComponent<CardHandler>();
                    if (player.curMana - crd.cost >= 0)
                    {
                        player.curMana -= crd.cost;
                        float rand = Random.RandomRange(0, 100);
                        if (rand <= crd.crit)
                        {
                            player.curEnemy.health -= crd.dmg + (crd.crit * 0.03f);
                        }
                        else
                        {
                            player.curEnemy.health -= crd.dmg;
                        }
                        player.curEnemy.HealthMove(player.curEnemy.health);
                    }
                }
            }
            //If the enemy dies delete it, end combat, and get rid of the hand of cards.
            if (player.curEnemy.health <= 0)
            {
                turnStart = false;
                Destroy(player.curEnemy.gameObject);
                player.combat = false;
                foreach (Transform child in hand.transform)
                {
                    Destroy(child.gameObject);
                }
            }
            
        }
        //This handles mana regen
        if (player.curMana != player.totalMana && !player.combat)
        {
            StartCoroutine("ManaRegen",5f);
        }
        else
        {
            StopCoroutine("ManaRegen");
        }
        if (player.edge && SceneManager.GetActiveScene().name == "Scene1")
        {
            Debug.Log("Edge Check");
            SceneManager.LoadScene("Scene2");
            player.transform.position = new Vector2(0,0);
            player.edge = false;
        }

    }
    //This is the mana regen
    IEnumerator ManaRegen(float c)
    {
        yield return new WaitForSeconds(c);
        if (player.curMana <= player.totalMana)
        {
            player.curMana += 0.8f * Time.deltaTime;
        }
        else
            yield return null;
    }
}

