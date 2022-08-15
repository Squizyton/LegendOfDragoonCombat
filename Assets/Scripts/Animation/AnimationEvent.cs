using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public bool useForCombat;

    [ShowIf("@useForCombat"),EnableIf("@!hitPlayer")]
    public bool hitEnemy;
    [ShowIf("@useForCombat"),EnableIf("@!hitEnemy")]
    public bool hitPlayer;
    
    
    
    [ShowIf("@!useForCombat")]
    public UnityEvent TriggerA;
    [ShowIf("@!useForCombat")]
    public UnityEvent TriggerB;
    [ShowIf("@!useForCombat")]
    public UnityEvent TriggerC;
    [ShowIf("@!useForCombat")]
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

    public void CombatHit()
    {
        if(hitEnemy)
            CombatManager.instance.HitEnemy();
        else if (hitPlayer)CombatManager.instance.PlayerHitAnimation();
    }

}
