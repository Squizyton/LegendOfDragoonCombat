using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Addition
{
    public string additionName;
    [Space(25)]
    public List<AnimationList> animationList = new List<AnimationList>();
}
[System.Serializable]
public class AnimationList
{
    public AnimationClip animation;
    public float animationSpeed;
    public int spGain;
}
