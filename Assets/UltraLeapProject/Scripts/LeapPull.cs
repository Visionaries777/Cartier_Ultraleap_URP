using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class LeapPull : MonoBehaviour
{
    public LeapServiceProvider leap;

    Dictionary<int, Vector3> pinch = new Dictionary<int, Vector3>();

    public static bool hovering {
        get; private set;
    }
    public static PinchPhase pinching {
        get; private set;
    }
    public static bool closeing {
        get; private set;
    }
    public static Vector3 wristPosition {
        get; private set;
    }
    public static Quaternion handRotation {
        get; private set;
    }
    public PinchPhase pinchTest;
    public float pullPushDistance = 0.5f;
    public float detectDistance = 2f;
    public float pinchStrengthPrecision = 0.75f;
    public float deltaVelocity = 0.7f;

    public enum HandDiretion
    {
        left,
        right,
        none
    }
    public HandDiretion handDiretion = HandDiretion.none;

    public enum PinchPhase
    {
        None,
        Start,
        Move,
        MoveX,
        Positive,
        Negative
    }
    static Vector3 pinchStartPosition;
    static float pinchStartTme;
    public static Vector3 pinchPosition {
        get; private set;
    }
    public static float pullProgress {
        get; private set;
    }
    private static List<float> clickList = new List<float>();
    [Range(0, 1)]
    public float xWeight = 0.7f;
    public float doubleClickTIme = 0.8f;


    public enum PullMode
    {
        Y,
        Z,
        YExtra,
        YAndDoubleClick
    }
    public PullMode pullMode;

    public static Vector3 GetPinchDelta()
    {
        if (pinching == PinchPhase.None) return Vector3.zero;
        return pinchStartPosition - pinchPosition;
    }

    float shake = 0;
    public float smallestVelocity = 1.45f;

    void Update()
    {
        bool currentPinch = false;
        hovering = false;
        closeing = false;
        if (leap.CurrentFrame != null)
        {
            foreach (Hand hand in leap.CurrentFrame.Hands)
            {
                wristPosition = hand.WristPosition.ToVector3();
                handRotation = hand.Rotation.ToQuaternion();

                Vector3 testPos = hand.PalmPosition.ToVector3();
                testPos.x = 0;
                if (testPos.magnitude > detectDistance)
                {
                    continue;
                }
                hovering = true;
                if (hand != null)
                {
                    pinchPosition = hand.PalmPosition.ToVector3();
                    if (hand.PinchStrength > pinchStrengthPrecision)
                    {
                        currentPinch = true;
                        //pinchPosition = hand.Fingers[0].TipPosition.ToVector3();
                        if (pinching == PinchPhase.None)
                        {
                            pinchStartPosition = pinchPosition;
                            pinchStartTme = Time.time;
                        }
                    }

                    if (IsMoveRight(hand))
                    {
                            handDiretion = HandDiretion.right;
                    }

                    if (IsMoveLeft(hand))
                    {
                            handDiretion = HandDiretion.left;
                    }
                    if (IsStationary(hand))
                    {
                        handDiretion = HandDiretion.none;
                    }
                }


            }
        }
        if (currentPinch)
        {
            if (pinching == PinchPhase.None)
            {
                pinching = PinchPhase.Start;
            }
        }
        else
        {
            if (pinching == PinchPhase.Start && pullMode == PullMode.YExtra)
            {
                Vector3 delta = GetPinchDelta();
                Vector3 decide = delta;
                decide.x = decide.x * xWeight;
                decide.y = decide.y * (1 - xWeight);
                decide.z = decide.z * (1 - xWeight);
                decide.Normalize();
                //print(decide);
                if (decide.y > .5f)
                {
                    pinching = PinchPhase.Negative;
                }
                else
                {
                    pinching = PinchPhase.None;

                }
            }
            else if (pinching == PinchPhase.Start && pullMode == PullMode.YAndDoubleClick)
            {
                pinching = PinchPhase.None;
                float time = Time.time - pinchStartTme;
                if (time < 1)
                {
                    if (time > 0.05f)
                    {
                        clickList.Add(Time.time);

                        if (clickList.Count > 2)
                        {
                            clickList.RemoveAt(0);
                        }
                        if (clickList.Count == 2 && clickList[1] - clickList[0] < doubleClickTIme)
                        {
                            clickList.Clear();
                            pinching = PinchPhase.Negative;
                        }
                    }
                }

            }
            else
            {
                pinching = PinchPhase.None;
            }
        }


        if (pinching == PinchPhase.Start)
        {
            Vector3 delta = GetPinchDelta();
            if (delta.magnitude > pullPushDistance)
            {
                Vector3 decide = delta;
                decide.x = Mathf.Abs(decide.x) * xWeight;
                decide.y = Mathf.Abs(decide.y) * xWeight;
                decide.z = Mathf.Abs(decide.z) * (1 - xWeight);
                if (decide.y > decide.x && decide.y > decide.z)
                {
                    pinching = PinchPhase.Move;
                }
                if (decide.x > decide.y && decide.x > decide.z)
                {
                    pinching = PinchPhase.MoveX;
                }
                //else if (pullMode == PullMode.Y || pullMode == PullMode.YExtra)
                //{
                //    if (delta.y > 0)
                //    {
                //        pinching = PinchPhase.Negative;
                //    }
                //    else
                //    {
                //        pinching = PinchPhase.Positive;
                //    }
                //}
                //else if (pullMode == PullMode.Z)
                //{
                //    if (delta.z > 0)
                //    {
                //        pinching = PinchPhase.Positive;
                //    }
                //    else
                //    {
                //        pinching = PinchPhase.Negative;
                //    }
                //}
                //else if (pullMode == PullMode.YAndDoubleClick)
                //{

                //    if (delta.y > 0)
                //    {
                //    }
                //    else
                //    {
                //        pinching = PinchPhase.Positive;
                //    }


                //}
                //Debug.Log(delta.normalized * 100+","+ decide.normalized*100+ ": "+pinching);
            }




        }

        {
            if (pinching == PinchPhase.Positive)
            {
                pullProgress = 2;
            }
            else if (pinching == PinchPhase.Start)
            {

                Vector3 delta = GetPinchDelta();
                float max = Mathf.Max(Mathf.Abs(delta.x), Mathf.Abs(delta.y), Mathf.Abs(delta.z));
                float pull = 0;
                if (max > 0)
                {
                    if (pullMode == PullMode.Y || pullMode == PullMode.YExtra || pullMode == PullMode.YAndDoubleClick)
                    {
                        pull = Mathf.Clamp01(-delta.y / max);
                    }
                    else if (pullMode == PullMode.Z)
                        pull = Mathf.Clamp01(-delta.z / max);


                    pullProgress = pull * (delta.magnitude / pullPushDistance);

                }
            }
            else if (pinching == PinchPhase.Move)
            {
                pullProgress = 0;
            }
            else
            {
                pullProgress = -1;
            }
        }



        pinchTest = pinching;
    }

    protected bool IsMoveLeft(Hand hand)
    {
        return hand.PalmVelocity.x < -deltaVelocity && !IsStationary(hand);
    }

    protected bool IsMoveRight(Hand hand)
    {
        return hand.PalmVelocity.x > deltaVelocity && !IsStationary(hand);
    }

    protected bool IsStationary(Hand hand)
    {
        return hand.PalmVelocity.Magnitude < smallestVelocity;
    }


    private void OnDrawGizmos()
    {
        if (pinching != PinchPhase.None)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(pinchPosition, Vector3.one * 0.1f);
        }
    }


}
