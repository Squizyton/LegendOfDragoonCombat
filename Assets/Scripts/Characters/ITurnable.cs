using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The only purpose of this class is to let me put enemies/characters into the same queue
/// </summary>
public interface ITurnable
{
    void TakeTurn();

    void EndTurn();

    int ReturnSpeed();
}
