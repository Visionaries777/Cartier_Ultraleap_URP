using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CircularList : MonoBehaviour
{
    public Opt4Display main;
    public GameObject font;
    public float grid,pop;
    public int last,current;
    public GameObject[] timeline;


    void Awake()
    {
        timeline = new GameObject[main.options.Length];
    }

    private void Start()
    {
        for (int i = 0; i < timeline.Length; i++)
        {
            var o = Instantiate(font, this.transform.position, Quaternion.identity, this.transform);
            timeline[i] = o;
            timeline[i].GetComponent<TextMesh>().text = main.options[i].year;
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
                timeline[i].transform.localPosition = new Vector3(pop * (i - current), grid * (i - current), pop * (i - current));
                timeline[i].SetActive(true);
            }
            for (int i = 1; i <= (current - 0); i++)
            {
                timeline[current - i].transform.localPosition = new Vector3(pop * i, -grid * i, pop * i);
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
            last = current;
        }
    }
}
