using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAnimatorController
{
    //animator variables
    public bool jumped;
    public bool gotHurt;
    public bool attacked;
    public float horizontalSpeed;
    public float verticalSpeed;
    public Animator _animator;

    public FighterAnimatorController(Animator animator)
    {
        _animator = animator;
    }

    /// <summary>
    /// Passes the animator variables to the animator
    /// </summary>
    public void UpdateValues()
    {
        _animator.SetBool("Jumped_b", jumped);
        _animator.SetBool("Hurt_b", gotHurt);
        _animator.SetBool("Attacked_b", attacked);

        _animator.SetFloat("WalkingSpeed_f", horizontalSpeed);
        _animator.SetFloat("VerticalVelocity_f", verticalSpeed);
    }
}

