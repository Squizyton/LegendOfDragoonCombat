using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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
    
    [OnValueChanged("OnValueChanged")]
    public AnimationClip animation;
    public string animationName;
    public float animationSpeed;
    public int spGain;
    
    private void OnValueChanged()
    {
        animationName = animation.name;
    }
}
