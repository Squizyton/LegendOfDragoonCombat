using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class Addition
{
    public string additionName;
    [Space(25)]
    public List<ComboMove> comboList = new List<ComboMove>();
}
[System.Serializable]
public class ComboMove
{
    
    [OnValueChanged("OnValueChanged")]
    public AnimationClip animation;
    [ReadOnly,InfoBox("This Will Be Updated Automatically")]public string animationName;
    public float animationSpeed;
    public int spGain;
    public float damageMultiplier;
    private void OnValueChanged()
    {
        animationName = animation.name;
    }
}
