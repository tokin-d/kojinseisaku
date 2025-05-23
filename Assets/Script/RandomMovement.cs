using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    private float chargeTime = 5.0f;
    private float timeCount;

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;

        transform.position += transform.forward * Time.deltaTime;

        if(timeCount > chargeTime)
        {
            Vector3 course = new Vector3(0, Random.Range(0, 180), 0);
            transform.localRotation = Quaternion.Euler(course);

            timeCount = 0;
        }
    }
}
