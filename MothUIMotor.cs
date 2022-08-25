using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothUIMotor : MonoBehaviour
{
    // Start is called before the first frame update
    public RandomMoths rmoths;
    public GameObject levelArea;
    public float speed;
    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        /** Move this ship forward per frame, if it gets too far from the game area, destroy it **/

        transform.position += transform.up * (Time.deltaTime * speed);

        float distance = Vector3.Distance(transform.position, levelArea.transform.position);
        if (distance > rmoths.deathSpawnCircleRadius)
        {
            RemoveShip();
        }
    }

    void RemoveShip()
    {
        /** Update the total ship count and then destroy this individual ship. **/

        Destroy(gameObject);
        rmoths.mothCount -= 1;
    }
}
