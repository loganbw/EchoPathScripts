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
        /** Move this moth forward per frame, if it gets too far from the game area, destroy it **/

        transform.position += transform.up * (Time.deltaTime * speed);

        float distance = Vector3.Distance(transform.position, levelArea.transform.position);
        if (distance > rmoths.deathSpawnCircleRadius)
        {
            Removemoth();
        }
    }

    void Removemoth()
    {
        /** Update the total moth count and then destroy this individual moth. **/

        Destroy(gameObject);
        rmoths.mothCount -= 1;
    }
}
