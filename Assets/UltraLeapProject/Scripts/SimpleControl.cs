using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleControl : MonoBehaviour
{
    public GameObject obj;
    public float speed;
    public Vector3 targetPos;
    public float[] z;
    public int current=0;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = obj.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(current<z.Length-1)
                current++;
            else
                current=0;
            targetPos = new Vector3(0,0,z[current]);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (current > 0)
                current--;
            else
                current = z.Length-1;
            targetPos = new Vector3(0, 0, z[current]);
        }
        Move();
    }

    void Move()
    {
        if (Vector3.Distance(obj.transform.localPosition, targetPos) > 0.01f)
        {
            obj.transform.localPosition = Vector3.Lerp(obj.transform.localPosition,targetPos,speed*Time.deltaTime);
        }
    }
}
