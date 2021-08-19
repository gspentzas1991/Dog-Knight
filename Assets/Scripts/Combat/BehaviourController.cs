using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains the base variables that determin a Gameobjects behaviour
/// Will be inherited from other scripts depending on the control type of the object
/// </summary>
public class BehaviourController : MonoBehaviour
{    
    //axisInputs
    public float horizontalMovement;
    public float verticalMovement;
    public bool receivedJumpInput;
    public bool receivedAttackInput;

    public AttackHorizontalDirection attackHorizontalDirection;
    public AttackVerticalDirection attackVerticalDirection;

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }
}
