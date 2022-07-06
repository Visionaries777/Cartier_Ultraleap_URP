using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class HandPoseDetect : MonoBehaviour
{
    public HandModelBase rightHandModel;
    public float deltaVelocity = 0.7f;
    public Finger.FingerType[] arr = { Finger.FingerType.TYPE_INDEX, Finger.FingerType.TYPE_MIDDLE };
    public enum HandDiretion
    {
        up,
        down,
        left,
        right,
        none
    }
    public HandDiretion handDiretion = HandDiretion.none;
    public GameObject obj;
    public float x;
    public float t=0;
    private GameObject _obj;
    private bool s;


    // Update is called once per frame
    void Update()
    {

        if (rightHandModel.IsTracked)
        {
            Hand leftHand = rightHandModel.GetLeapHand();
            if (IsMoveLeft(leftHand))
            {
                handDiretion = HandDiretion.left;

            }
            else if (IsMoveRight(leftHand))
            {
                handDiretion = HandDiretion.right;
            }
            else if (IsMoveUp(leftHand))
            {
                handDiretion = HandDiretion.up;
            }
            else if (IsMoveDown(leftHand))
            {
                handDiretion = HandDiretion.down;
            }
            else
            {
                handDiretion = HandDiretion.none;
            }

            if (isCloseHand(leftHand))
            {
                t+=Time.deltaTime;
                if (t >= 3&&!s)
                {
                   _obj=Instantiate(obj,leftHand.WristPosition.ToVector3(),Quaternion.identity);
                    s=true;
                }
            }
            else
            {
                t=0;
            }

            if (s)
            {
                var pos = leftHand.WristPosition.ToVector3();
                _obj.transform.localPosition = pos;
                var rotation = leftHand.Rotation.ToQuaternion();
                _obj.transform.localRotation = rotation;
                
            }
        }


    }


    bool CheckFingerOpenToHand(Hand hand, Finger.FingerType[] fingerTypeArr, float deltaCloseFinger = 0.05f)
    {
        List<Finger> listOfFingers = hand.Fingers;
        float count = 0;
        for (int f = 0; f < listOfFingers.Count; f++)
        {
            Finger finger = listOfFingers[f];
            if ((finger.TipPosition - hand.PalmPosition).Magnitude < deltaCloseFinger)
            {
                if (fingerTypeArr.Length == 0)
                {
                    count++;
                    continue;
                }

                for (int i = 0; i < fingerTypeArr.Length; i++)
                {
                    if (finger.Type == fingerTypeArr[i])
                    {
                        return false;
                    }
                    else
                    {
                        count++;
                    }
                }
            }
        }

        if (fingerTypeArr.Length == 0)
        {
            return count == 5;
        }

        return (count / fingerTypeArr.Length == 5 - fingerTypeArr.Length);
    }

    bool isGrabHand(Hand hand)
    {
        return hand.GrabStrength > 0.7;
    }

    bool isCloseHand(Hand hand)
    {
        List<Finger> listOfFingers = hand.Fingers;
        int count = 0;
        for (int f = 0; f < listOfFingers.Count; f++)
        {
            Finger finger = listOfFingers[f];
            if ((finger.TipPosition - hand.PalmPosition).Magnitude < 0.05f)
            {
                count++;
            }
        }
        return (count == 4);
    }

    bool IsOpenFullHand(Hand hand)
    {
        return hand.GrabStrength == 0;
    }

    protected bool IsMoveLeft(Hand hand)
    {
        return hand.PalmVelocity.x < -deltaVelocity;
    }

    protected bool IsMoveRight(Hand hand)
    {
        return hand.PalmVelocity.x > deltaVelocity;
    }

    protected bool IsMoveUp(Hand hand)
    {
        return hand.PalmVelocity.y > deltaVelocity;
    }

    protected bool IsMoveDown(Hand hand)
    {
        return hand.PalmVelocity.y < -deltaVelocity;
    }

    bool CheckFingerCloseTohand(Hand hand)
    {
        List<Finger> listOfFingers = hand.Fingers;
        int count = 0;
        for (int f = 0; f < listOfFingers.Count; f++)
        {
            Finger finger = listOfFingers[f];
            if ((finger.TipPosition - hand.PalmPosition).Magnitude < 0.05f)
            {
                if (finger.Type == Finger.FingerType.TYPE_INDEX)
                {
                    return false;
                }
                else
                {
                    count++;
                }
            }
        }
        return (count == 4);
    }
}
