using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalControl : MonoBehaviour
{
    public LeapDisplayMain main;
    [System.Serializable]
    public class Option
    {
        public bool interactive;
        public GameObject display;
        [System.NonSerialized]
        public Vector3 displayCache;
    }
    public Option[] options;
    public GameObject lastPage;
    public int last = 0;
    public float rotationOffset;
    private int current = 0;
    private float focus = 0;
    public Color bgColor;
    public Material bg;
    Coroutine animation;

    float pop = 0;
    public float speed=10;

    void Awake()
    {

        foreach (Option option in options)
        {
            option.displayCache = main.transform.InverseTransformPoint(option.display.transform.position); //wip missing forward offset
            option.displayCache.x = 0;
        }

    }

    private void OnEnable()
    {

        UpdateTransform();
        pop = 0;
        focus = last = current;
        lastPinching = LeapPull.pinching;
        lastPinchDelta = LeapPull.GetPinchDelta();
        bg.color = bgColor;
        RenderSettings.skybox = bg;
    }
    private LeapPull.PinchPhase lastPinching;
    private Vector3 lastPinchDelta;

    void Update()
    {
        if (animation != null) return;
        if (LeapPull.pinching != LeapPull.PinchPhase.None)
        {
            if (LeapPull.pinching == LeapPull.PinchPhase.MoveX)
            {
                if (lastPinching == LeapPull.PinchPhase.None)
                {
                }
                else
                {
                    focus = Mathf.Clamp(focus + (LeapPull.GetPinchDelta() - lastPinchDelta).x * main.pinchMultiply, -0.2f, options.Length - 1 + 0.2f);
                    current = Mathf.RoundToInt(Mathf.Clamp(focus, 0, options.Length - 1));
                }
            }
            else
            {
                if (options[current].interactive)
                {

                }
                else
                {
                    if (lastPinching == LeapPull.PinchPhase.Move)
                    {
                        OnCancel();
                    }
                }

            }
        }
        else
        {

            focus = Mathf.Lerp(focus, current, main.moveDamping);
        }
        lastPinching = LeapPull.pinching;
        lastPinchDelta = LeapPull.GetPinchDelta();
        pop = Mathf.MoveTowards(pop, 1, Time.deltaTime / 0.3f);
        if (last != current) pop = 0;
        last = current;
        UpdateTransform();
        if (options[current].interactive)
        {
            var q = LeapPull.handRotation.normalized;
            q.Set(q.x- rotationOffset, 0,0,q.w- rotationOffset);
            options[current].display.transform.rotation = q;
        }

    }
    void UpdateTransform()
    {
        for (int i = 0; i < options.Length; i++)
        {
            Option option = options[i];
            option.display.transform.position = main.transform.TransformPoint(option.displayCache + new Vector3((i - focus) * main.insideGridWidth * main.pinchMultiply, main.watchHeight, 0));

        }
    }


    public void OnCancel()
    {
        focus = last = current = 0;
        if (lastPage)
        {
            lastPage.gameObject.SetActive(true);
            this.gameObject.SetActive(false);

        }
    }

   

}
