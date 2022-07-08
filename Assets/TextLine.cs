using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextLine : MonoBehaviour
{
    [TextArea(4,40)]
    public string text;
    public string[] lines;
    public TextMesh font,textUI;
    public Transform pos;
    public float speed,colorSpeed;
    public float dropDownOffset,lineSpace,dropDownRange,showUpRange;

    private TextMesh[] _lines;

    // Start is called before the first frame update
    void Start()
    {
        lines = text.Split('\n');
        _lines = new TextMesh[lines.Length];


        for (int i = 0; i < lines.Length; i++)
        {
            var l = Instantiate(font, Vector3.zero, Quaternion.identity,pos);
            l.transform.localPosition = new Vector3(0,0,0);
            l.text = lines[i];
            l.color = new Color(l.color.r, l.color.g, l.color.b, 0);
            l.transform.localPosition = new Vector3(0, -lineSpace * i - dropDownOffset - dropDownRange*i, 0);
            _lines[i] = l;
        }
        StartCoroutine("TextAnimation");
    }

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            textUI.text = "";
            for (int i = 0; i < lines.Length; i++)
            {
                if(_lines[i] != null)
                    DestroyImmediate(_lines[i]);
                var l = Instantiate(font, Vector3.zero, Quaternion.identity, pos);
                l.transform.localPosition = new Vector3(0, 0, 0);
                l.text = lines[i];
                l.color = new Color(l.color.r, l.color.g, l.color.b, 0);
                l.transform.localPosition = new Vector3(0, -lineSpace * i - dropDownOffset - dropDownRange*i, 0);
                _lines[i] = l;
            }
            StartCoroutine("TextAnimation");
        }

    }

    IEnumerator TextAnimation()
    {
        while (_lines[_lines.Length-1].transform.localPosition.y<-lineSpace*(_lines.Length-1))
        {
            for (int i = 0; i < _lines.Length; i++)
            {
                if (_lines[i] != null)
                {
                    if (_lines[i].transform.localPosition.y <= -lineSpace * i)
                    {
                        if ((-lineSpace * i) - _lines[i].transform.localPosition.y <= showUpRange)
                        {
                            _lines[i].color = new Color(_lines[i].color.r, _lines[i].color.g, _lines[i].color.b, _lines[i].color.a + colorSpeed * Time.deltaTime);
                            
                        }
                        _lines[i].transform.localPosition += new Vector3(0, (speed - i * 0.01f) * Time.deltaTime, 0);

                    }
                    else
                    {
                        textUI.text += _lines[i].text + "\n";
                        Destroy(_lines[i].gameObject);
                    }
                }


            }
            yield return null;
        }

        textUI.text += _lines[_lines.Length - 1].text + "\n";
        Destroy(_lines[_lines.Length - 1].gameObject);


    }


}
