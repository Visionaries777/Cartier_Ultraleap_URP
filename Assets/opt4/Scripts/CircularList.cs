using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CircularList : MonoBehaviour
{
    public Opt4Display main;
    public GameObject font;
    public float grid,pop,speed;
    public int last,current;
    public Color selectedColor,normalColor;
    private GameObject[] timeline;
    private Vector3[] targetPos;


    void Awake()
    {
        timeline = new GameObject[main.options.Length];
        targetPos = new Vector3[timeline.Length];
    }

    private void Start()
    {
        for (int i = 0; i < timeline.Length; i++)
        {
            var o = Instantiate(font, this.transform.position, Quaternion.identity, this.transform);
            timeline[i] = o;
            timeline[i].GetComponent<TextMesh>().text = main.options[i].year;
            timeline[i].transform.localPosition = new Vector3(pop * (i - current), grid * (i - current), pop * (i - current));
            timeline[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

        current = main.current;
        if (last != current)
        {
            for (int i = current; i < timeline.Length; i++)
            {
                //timeline[i].transform.localPosition = new Vector3(pop * (i - current), grid * (i - current), pop * (i - current));
                targetPos[i] = new Vector3(pop * (i - current), grid * (i - current), pop * (i - current));
                timeline[i].SetActive(true);
            }
            for (int i = 1; i <= current; i++)
            {
                //timeline[current - i].transform.localPosition = new Vector3(pop * i, -grid * i, pop * i);
                targetPos[current - i] = new Vector3(pop * i, -grid * i, pop * i);
                timeline[i].SetActive(true);
            }
            if (current > 3)
            {
                for (int i = 0; i < current - 2; i++)
                {
                    timeline[i].SetActive(false);
                }
            }
            if (current < timeline.Length - 3)
            {
                for (int i = timeline.Length - 1; i > current + 2; i--)
                {
                    timeline[i].SetActive(false);
                }
            }
            timeline[current].GetComponent<TextMesh>().color = selectedColor;
            if(last!=-1)
                timeline[last].GetComponent<TextMesh>().color = normalColor;
            last = current;
        }
        Move();
    }

    void Move()
    {
        for(int i = 0; i < timeline.Length; i++)
        {
            if (Vector3.Distance(timeline[i].transform.localPosition, targetPos[i]) >= 0.01)
            {
                timeline[i].transform.localPosition = Vector3.Lerp(timeline[i].transform.localPosition,targetPos[i],speed*Time.deltaTime);
            }
        }
    }


}
