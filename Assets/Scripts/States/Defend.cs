using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defend : IState
{
    public void Action()
    {
        CombatManager.instance.ReturnCurrentCharacter().Defend();
        CombatManager.instance.NextTurn();
    }
}
