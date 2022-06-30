using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public UnityEvent TriggerA;
    public UnityEvent TriggerB;
    public UnityEvent TriggerC;
    public UnityEvent TriggerD;
    
    
    public void TriggerA_Event()
    {
        TriggerA?.Invoke();
    }
    public void TriggerB_Event()
    {
        TriggerB?.Invoke();
    }
    public void TriggerC_Event()
    {
        TriggerC?.Invoke();
    }
    public void TriggerD_Event()
    {
        TriggerD?.Invoke();
    }
}
