using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;




    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        rigid2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Translate(0, 1.0f, 0);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.Translate(0, -1.0f, 0);
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.Translate(-1.0f, 0, 0);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.Translate(1.0f, 0, 0);
        }
    }
}
