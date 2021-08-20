using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : BehaviourController
{
    //inputs
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private KeyCode attackKey;
    [SerializeField] private string horizontalAxisName;
    [SerializeField] private string verticalAxisName;

    // Update is called once per frame
    protected override void Update()
    {
        DetectInput();
        base.Update();
    }

    private void DetectInput()
    {
        horizontalMovement = Input.GetAxis(horizontalAxisName);
        verticalMovement = Input.GetAxis(verticalAxisName);

        if (Input.GetKeyDown(jumpKey))
        {
            jumpInput.receivedInput = true;
        }

        if (Input.GetKeyDown(attackKey))
        {
            attackInput.receivedInput = true;
            CalculateAttackDirection();
        }   
    }

    private void CalculateAttackDirection()
    {
        attackHorizontalDirection = AttackHorizontalDirection.Front;
        attackVerticalDirection = AttackVerticalDirection.Center;
        if (verticalMovement != 0)
        {
            if (horizontalMovement == 0)
            {
                attackHorizontalDirection = AttackHorizontalDirection.Center;
            }
            if (verticalMovement > 0)
            {
                attackVerticalDirection = AttackVerticalDirection.Upwards;
            }
            else if (verticalMovement < 0)
            {
                attackVerticalDirection = AttackVerticalDirection.Downwards;
            }
        }
    }
}
