using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject enemy;
    public float speed = 1.0f;
    Vector3 movePosition;
    Vector2 velocity;
    
    // Start is called before the first frame update
    void Start()
    {
        movePosition = moveRandomPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if(movePosition == enemy.transform.position)
        {
            movePosition = moveRandomPosition();
        }
        
        this.enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, movePosition, speed * Time.deltaTime);
    }

   
    private Vector3 moveRandomPosition()
    {
        Vector3 randomPos = new Vector3(Random.Range(-7.0f, 7.0f), Random.Range(-4.0f, 4.0f), 0);
        return randomPos;
    }
}
