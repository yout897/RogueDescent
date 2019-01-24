using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 10;
    public GameObject hlth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Scale the health bar to the actual health total
    public void HealthMove(float h)
    {
        hlth.transform.localScale = new Vector3(h,1,0);
    }
}
