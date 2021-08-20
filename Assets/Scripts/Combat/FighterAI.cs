using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAI : BehaviourController
{
    private CombatState combatState;
    [SerializeField] private string targetTag;
    private Fighter targetFighter;
    /// <summary>
    /// The maximum distance from the target, during which the enemy is cautious.
    /// In a cautious state, the enemy will flee from the target's attacks
    /// </summary>
    private float cautionRange = 5f;

    /// <summary>
    /// If the attackController is null, then the AI will never attack
    /// </summary>
    private AttackController attackController;
    /// <summary>
    /// How much higher the target must be, for the AI to attempt a jump
    /// </summary>
    private float jumpTreshold = 2f;

    /// <summary>
    /// How much further than the target the AI will travel
    /// </summary>
    // This was put in place to fix a bug where the AI rotated constantly at edge cases
    private float moveTreshold = 1f;
    /// <summary>
    /// The frequency during which the ai decides on a behaviour. Used to adjust the difficulty of the fighter
    /// </summary>
    [SerializeField] public float behaviourDescisionFrequency = 0.5f;

    protected override void Start()
    {
        base.Start();
        attackController = GetComponent<AttackController>();
        targetFighter = GameObject.FindGameObjectWithTag(targetTag)?.GetComponent<Fighter>();
        StartCoroutine(DecideBehaviour());
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (targetFighter==null)
        {
            targetFighter = GameObject.FindGameObjectWithTag(targetTag)?.GetComponent<Fighter>();
        }
    }

    /// <summary>
    /// Decides which action the AI will do, according to  Finite State Machine logic, every set seconds.
    /// Then it repeats itself
    /// </summary>
    private IEnumerator DecideBehaviour()
    {
        if (targetFighter != null)
        {
            switch (combatState)
            {
                case CombatState.Approaching:
                    ApproachTarget();
                    break;
                case CombatState.BackingAway:
                    BackAway();
                    break;
                case CombatState.Attacking:
                    Attack();
                    break;
            }
            yield return new WaitForSeconds(behaviourDescisionFrequency);
            StartCoroutine(DecideBehaviour());
        }

    }

    /// <summary>
    /// Approaches the target, until it reaches the attack range, or the target attacks when within caution range
    /// </summary>
    private void ApproachTarget()
    {
        if (targetFighter.transform.position.x > transform.position.x+ moveTreshold)
        {
            horizontalMovement = 1;
        }
        else if (targetFighter.transform.position.x < transform.position.x - moveTreshold)
        {
            horizontalMovement = -1;
        }

        if (targetFighter.transform.position.y > transform.position.y + jumpTreshold)
        {
            jumpInput.receivedInput = true;
        }
        else
        {
            //We reset the jump input to false, so that the enemy doesn't jump due to buffering
            jumpInput.resetInput = true;
        }
        var distanceToTarget = Vector3.Distance(targetFighter.transform.position, transform.position);
        //If we are within caution range and the player is attackig, then back away
        if (distanceToTarget <= cautionRange && targetFighter.attackController.isAttacking)
        {
            combatState = CombatState.BackingAway;
        }
        if (attackController!= null && distanceToTarget <= attackController.attackRange)
        {
            combatState = CombatState.Attacking;
        }
    }

    /// <summary>
    /// Runs away from the target. If out of danger, then starts aproaching them again
    /// </summary>
    private void BackAway()
    {
        if (targetFighter.transform.position.x > transform.position.x + moveTreshold)
        {
            horizontalMovement = -1;
        }
        else if (targetFighter.transform.position.x < transform.position.x - moveTreshold)
        {
            horizontalMovement = 1;
        }

        var distanceToTarget = Vector3.Distance(targetFighter.transform.position, transform.position);
        if (!targetFighter.attackController.isAttacking || distanceToTarget > cautionRange+moveTreshold)
        {
            combatState = CombatState.Approaching;
        }
    }

    /// <summary>
    /// Atacks the player. If he leaves the attacking range, start aproaching them again
    /// </summary>
    private void Attack()
    {
        //Calculate the distance to target
        var distanceToTarget = Vector3.Distance(targetFighter.transform.position, transform.position);
        //If the target is too far away, change combat state to Aproaching
        if (distanceToTarget > attackController.attackRange)
        {
            combatState = CombatState.Approaching;
        }
        //If the target is within range, start attacking
        else
        {
            attackInput.receivedInput = true;
            //Calculate the direction of the attack
            attackHorizontalDirection = AttackHorizontalDirection.Center;
            attackVerticalDirection = AttackVerticalDirection.Center;
            if (Mathf.Abs(targetFighter.transform.position.x - transform.position.x) > 0.5f)
            {
                attackHorizontalDirection = AttackHorizontalDirection.Front;
            }
            if (targetFighter.transform.position.y > transform.position.y + 1f)
            {
                attackVerticalDirection = AttackVerticalDirection.Upwards;
            }
            else if (targetFighter.transform.position.y < transform.position.y - 1f)
            {
                attackVerticalDirection = AttackVerticalDirection.Downwards;
            }
        }
    }
}
