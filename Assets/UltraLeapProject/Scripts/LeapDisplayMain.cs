using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapDisplayMain : MonoBehaviour
{
    //public LeapSpinPage spinPage; 
    public Color shadowColor = Color.grey;
    public Color invalidColor = new Color(0.8f, 0.2f, 0.2f, 1);

    public float gridWidth = 0.2f; // should support page custom width
    public float insideGridWidth = 0.2f;
    public float textGridWidth = 0.1f;
    public float textHeight = 0.1f;
    public float textDeep = 0.1f;
    public float selectedForward = 0.1f;
    [Range(0,1)]
    public float moveDamping = 0.6f;
    public float pinchMultiply = 1;
    public float transitionTime = 0.5f;
    public float watchHeight;
    public float transitionDistance = 0.15f;
    public AnimationCurve transitonCurve;
    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawLine(transform.TransformPoint(new Vector3(-gridWidth * 2, 0, 0)), transform.TransformPoint(new Vector3(gridWidth * 2, 0, 0)));
    //    for (int i = 0; i < 4; i++)
    //    {
    //        Gizmos.DrawWireSphere(transform.TransformPoint(new Vector3(gridWidth * (i-1), 0, 0)), 0.1f);
    //    }
    //    Gizmos.DrawWireSphere(transform.TransformPoint(new Vector3(0, 0, selectedForward)), 0.1f);
    //}
}
