using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour,IState
{
    public void Action()
    {
        CombatManager.instance.currentState = CombatManager.CombatState.SelectingTarget;
        CombatUIManager.instance.TurnOnAttackUI();
        CombatUIManager.instance.UpdateEnemySelection(CombatManager.instance.GetEnemies()[0]);
    }
}
