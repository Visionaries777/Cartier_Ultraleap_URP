using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADWFWE : MonoBehaviour
{
    public GameObject obj;
    public float speed;
    public Transform a,b;
    Vector3 newPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        var positionA = new Vector3(-5, 3, 0);
        var positionB = new Vector3(5, 3, 0);

        if (Input.GetKeyDown(KeyCode.Q))
            newPosition = positionA;
        if (Input.GetKeyDown(KeyCode.E))
            newPosition = positionB;

        if(Vector3.Distance(newPosition,obj.transform.position)>=0.2)
            obj.transform.position = Vector3.Lerp(obj.transform.position, newPosition, speed * Time.deltaTime);
    }
}
