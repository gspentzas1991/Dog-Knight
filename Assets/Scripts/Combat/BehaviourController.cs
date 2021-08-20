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
    public InputStatus jumpInput = new InputStatus();
    public InputStatus attackInput = new InputStatus();

    public AttackHorizontalDirection attackHorizontalDirection;
    public AttackVerticalDirection attackVerticalDirection;

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        if (jumpInput.resetInput)
        {
            jumpInput.Reset();
        }
        if (attackInput.resetInput)
        {
            attackInput.Reset();
        }
    }

}
