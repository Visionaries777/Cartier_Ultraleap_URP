using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeLineTest : MonoBehaviour
{
    public Opt4Display opt;
    public GameObject prefab;
    public RectTransform dot;
    public Transform pos;
    public float grid;
    public float speed;
    public int last=-1;
    public int current=0;
    public Font highlightFont;
    public Color highlightColor;
    public Font normalFont;
    public Color normalColor;

    private TextMesh[] _obj;
    private float height,o;
    private int fontSize;
    // Start is called before the first frame update
    void Start()
    {
        _obj = new TextMesh[opt.options.Length];
        o= height = dot.anchoredPosition.x;
        var p = pos.position;
        for(int i = 0; i < _obj.Length; i++)
        {
            var o = Instantiate(prefab,p,Quaternion.identity,pos);
            _obj[i] = o.GetComponent<TextMesh>();
            _obj[i].text = opt.options[i].year;
            p = new Vector3(p.x, p.y + grid, p.z);

        }
        pos.rotation = Quaternion.Euler(45,0,0);
        fontSize = _obj[0].fontSize;
    }

    private void Update()
    {
        current = opt.current;
        if (current != last)
        {
            if (last == -1)
            {
                _obj[current].transform.position = new Vector3(_obj[current].transform.position.x, _obj[current].transform.position.y, _obj[current].transform.position.z);
                _obj[current].font = highlightFont;
                _obj[current].GetComponent<MeshRenderer>().materials = new Material[] {highlightFont.material};
                _obj[current].color = highlightColor;
                _obj[current].fontSize = fontSize+50;
            }
            else
            {
                _obj[current].font = highlightFont;
                _obj[current].GetComponent<MeshRenderer>().materials = new Material[] { highlightFont.material };
                _obj[current].color = highlightColor;
                _obj[current].fontSize = fontSize + 50;
                _obj[current].transform.position = new Vector3(_obj[current].transform.position.x, _obj[current].transform.position.y, _obj[current].transform.position.z);
                _obj[last].font = normalFont;
                _obj[last].GetComponent<MeshRenderer>().materials = new Material[] { normalFont.material };
                _obj[last].color = normalColor;
                _obj[last].fontSize = fontSize;
                _obj[last].transform.position = new Vector3(_obj[last].transform.position.x, _obj[last].transform.position.y, _obj[last].transform.position.z);
                height = o + grid*current;
            }


            last = current;
        }

        MoveDot();
    }

    private void MoveDot()
    {
            //dot.anchoredPosition = new Vector2(height, 0);
        if(dot.anchoredPosition.x != height)
        {
            var pos = dot.anchoredPosition;
            dot.anchoredPosition = pos + new Vector2((height-pos.x)*Time.deltaTime*speed,0);
        }
    }

}
