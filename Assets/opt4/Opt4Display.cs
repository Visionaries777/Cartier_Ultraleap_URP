using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Opt4Display : MonoBehaviour
{
    public LeapDisplayMain main;
    [System.Serializable]
    public class Option
    {
        public enum Goal
        {
            Normal,
            Spin
        }
        public GameObject display;
        [System.NonSerialized]
        public Vector3 displayCache;
        public GameObject preview;
        public Transform previewPos;
        public string year;
        public GameObject bg;
        public Goal goal;
        public GameObject nextPage;
    }
    public Option[] options;
    [Range(0f,1f)]
    public float alpha;
    public float waitTime;
    public Image loadingDot;
    public int last = 0;
    public Opt4Display lastPage;
    public bool inside;
    public float speed;
    public float size;
    private Color32 color;
    public int current = 0;
    private float focus = 0;
    public float c=0;
    public Color bgColor;
    public Material bg;
    private GameObject currentPreview;

    Coroutine animation;

    void Awake()
    {
        foreach (Option option in options)
        {
            option.displayCache = main.transform.InverseTransformPoint(option.display.transform.position); //wip missing forward offset
            option.displayCache.z = 0f;
            //option.bg.text = option.year;
        }
        //color = options[current].bg.color;

    }

    private void OnEnable()
    {

        focus = last = current;
        lastPinching = LeapPull.pinching;
        lastPinchDelta = LeapPull.GetPinchDelta();
        UpdateTransform();
        bg.SetColor("_Tint",bgColor);
        RenderSettings.skybox = bg;
    }
    private LeapPull.PinchPhase lastPinching;
    private Vector3 lastPinchDelta;

    void Update()
    {

        OutsideControl();
        lastPinching = LeapPull.pinching;
        lastPinchDelta = LeapPull.GetPinchDelta();

        if (animation == null)
        {
            UpdateTransform();
            if (last != current)
            {
                UpdatePreview();
                last = current;
            }
        }

        //options[current].bg.color = color;

    }

    void OutsideControl()
    {
        if (LeapPull.pinching != LeapPull.PinchPhase.None)
        {
            c = 0;
            loadingDot.fillAmount = 0;
            if (LeapPull.pinching == LeapPull.PinchPhase.Move)
            {
                if (lastPinching == LeapPull.PinchPhase.None)
                {
                }
                else
                {
                    focus = Mathf.Clamp(focus + (LeapPull.GetPinchDelta() - lastPinchDelta).y * -main.pinchMultiply + speed, -0.2f, options.Length - 1 + 0.2f);
                    speed = focus * (LeapPull.GetPinchDelta() - lastPinchDelta).y * -main.pinchMultiply;
                    current = Mathf.RoundToInt(Mathf.Clamp(focus, 0, options.Length - 1));
                }
            }
            else
            {
                speed = 0;
            }
        }
        else
        {
            if (options[current].nextPage != null)
            {
                c += Time.deltaTime;
                loadingDot.fillAmount += 1 / waitTime * Time.deltaTime;
                if (c >= waitTime)
                    OnEnter();
            }


        }
    }


    void UpdateTransform()
    {
        for (int i = 0; i < options.Length; i++)
        {
            Option option = options[i];
            option.display.transform.position = main.transform.TransformPoint(option.displayCache + new Vector3(0, main.watchHeight, (i - focus) * main.gridWidth * main.pinchMultiply));
            option.display.SetActive(true);

        }
        if (current > 0)
        {
            for (int i = 0; i < current; i++)
            {
                Option option = options[i];
                option.display.SetActive(false);

            }

        }

        if (c == 0)
        {
            var a = alpha;
            var s = size;
            for (int i = current; i < options.Length; i++)
            {
                //options[i].bg.color = new Color(color.r, color.g, color.b, a);
                var m = options[i].bg.GetComponent<Renderer>().material;
                options[i].bg.transform.localScale = new Vector3(s, s, s);
                m.color = new Color(m.color.r, m.color.g, m.color.b, a);
                a = a / 2;
                s = s - 1;

            }
        }
        else
        {
            for (int i = current+1; i < options.Length; i++)
            {

                var m = options[i].bg.GetComponent<Renderer>().material;
                m.color = new Color(m.color.r, m.color.g, m.color.b, m.color.a-c/10*Time.deltaTime);

            }
        }


    }

    void UpdatePreview()
    {
        if (currentPreview != null)
            Destroy(currentPreview);
        if (options[current].nextPage != null)
        {
            currentPreview = Instantiate(options[current].preview,options[current].previewPos.position,Quaternion.identity,this.transform);
            Debug.Log("hehe");
        }

    }

    public void OnEnter()
    {
        Option op = options[current];
        if (current >= 0)
        {
            // DisplayPage nextPage = next.GetComponent<DisplayPage>();
            if (op.goal == Option.Goal.Normal)
            {
                if (op.nextPage != null)
                {
                    GameObject next = op.nextPage;
                    next.GetComponent<HorizontalControl>().lastPage = this.gameObject;
                    animation = StartCoroutine("FadeLeft");
                    this.enabled = false;
                    //next.SendMessage("FadeFront", SendMessageOptions.DontRequireReceiver);

                }
                else
                {
                    return;
                }
            }
        }
    }

    public void OnCancel()
    {
        if (lastPage)
        {
            lastPage.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
            //lastPage.SendMessage("FadeBack", SendMessageOptions.DontRequireReceiver);
        }
    }

    IEnumerator FadeLeft()
    {
        var a = Instantiate(options[current].display, options[current].display.transform.position,Quaternion.identity);
        var b = Instantiate(currentPreview, currentPreview.transform.position,Quaternion.identity);
        currentPreview.SetActive(false);
        options[current].display.SetActive(false);
        GameObject next = options[current].nextPage;
        var c = bgColor;
        for (float t = 0; t < 1; t += Time.deltaTime / main.transitionTime)
        {
            float v = 1 - main.transitonCurve.Evaluate(t);
            a.transform.position += new Vector3(main.transitionDistance * (v)*Time.deltaTime , 0, 0);
            if (b.transform.position.x > 0)
            {
                b.transform.position = b.transform.position + new Vector3(-1f*Time.deltaTime,0, 0);
            }
            //if (bgColor.r > 0.25)
            //{
            //    c = new Color(c.r - 0.05f, c.g - 0.05f, c.b - 0.05f, c.a);
            //    bg.SetColor("_Tint", c);
            //    RenderSettings.skybox = bg;
            //}
            yield return null;
        }
        Destroy(a);
        Destroy(b);
        this.enabled = true;
        next.SetActive(true);
        this.gameObject.SetActive(false);
        currentPreview.SetActive(true);
        options[current].display.SetActive(true);
        animation = null;
    }

}
