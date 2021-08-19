using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public bool isAttacking = false;
    public bool hitboxIsActive = false;
    public bool isRecovering = false;
    public float attackRange;
    private AudioSource _audioSource;

    [SerializeField] private float attackPower = 1f;
    [SerializeField]private float hitboxDuration = 0.5f;
    [SerializeField] private float recoveryDuration = 1f;
    [SerializeField] private float attackStartup = 0.2f;
    [SerializeField] private Vector3 hitboxSize = new Vector3(0.5f,0.5f,0.5f);
    [SerializeField] private Vector3 attackOffset = new Vector3(1.58f, 0.4f, 0f);
    [SerializeField] private Vector3 verticalAttackOffset = new Vector3(0f, 2f, 0f);
    [SerializeField] private LayerMask hitableLayer;

    private AttackHorizontalDirection horizontalAttackDirection;
    private AttackVerticalDirection verticalAttackDirection;
    /// <summary>
    /// The instance of the active coroutine.
    /// </summary>
    // If AttackController changes to use multiple simutaleous coroutines
    // this needs to turn into a list of active coroutines
    private Coroutine activeCoroutine;
    [SerializeField] AudioClip attackAudioClip;

    private void Start()
    {
        //initialize the attackRange value, depending on the attack's hitbox length
        attackRange = hitboxSize.x + attackOffset.x+1;
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (hitboxIsActive)
        {
            HitDetection();
        }
    }

    /// <summary>
    /// Detects all collided targets with the hitbox, and informs them that they've been hit
    /// </summary>
    private void HitDetection()
    {
        Collider[] colliders = Physics.OverlapBox(GetHitboxPosition(), hitboxSize, transform.rotation, hitableLayer);

        foreach (var collider in colliders)
        {
            //if the collider had a fighter component, inform them that they were hit
            if (collider.TryGetComponent(out Fighter hitFighter))
            {
                hitFighter.GotHit(transform.position, attackPower);
            }
        }
    }


    /// <summary>
    /// Creates a hitbox in front of the character for a specified duration. 
    /// Then the attack has a recovery duration during which you can't attack again
    /// </summary>
    public void Attack(AttackVerticalDirection verticalDirection, AttackHorizontalDirection horizontalDirection)
    {
        horizontalAttackDirection = horizontalDirection;
        verticalAttackDirection = verticalDirection;
        if (!isAttacking)
        {
            isAttacking = true;
            activeCoroutine= StartCoroutine(BeginAttack());
        }
    }

    /// <summary>
    /// If the attackController has any active attacks, they will be stopped, and the booleans reset
    /// </summary>
    public void InteruptAttack()
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }
        isAttacking = false;
        hitboxIsActive = false;
        isRecovering = false;
    }

    /// <summary>
    /// Begins counting the attack's startup, and after it finishes, start's the hitbox creationg of the attack
    /// </summary>
    /// <returns></returns>
    private IEnumerator BeginAttack()
    {
        _audioSource.clip = attackAudioClip;
        _audioSource.Play();
        yield return new WaitForSeconds(attackStartup);
        activeCoroutine = StartCoroutine(CreateHitbox());
    }

    /// <summary>
    /// Creates an active hitbox for the attack. After a few seconds starts the attack's recovery
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreateHitbox()
    {
        hitboxIsActive = true;
        yield return new WaitForSeconds(hitboxDuration);
        hitboxIsActive = false;
        activeCoroutine = StartCoroutine(RecoverFromAttack());
    }

    /// <summary>
    /// Keeps the character in recovery after the attack for a few seconds
    /// </summary>
    private IEnumerator RecoverFromAttack()
    {
        isRecovering = true;
        yield return new WaitForSeconds(recoveryDuration);
        isAttacking = false;
        isRecovering = false;
    }

    /// <summary>
    /// Draws the created hitbox on a gizmo
    /// </summary>
    private void OnDrawGizmos()
    {
        if (isAttacking)
        {
            if (hitboxIsActive)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }
            Gizmos.matrix = Matrix4x4.TRS(GetHitboxPosition(), transform.rotation, transform.localScale);
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(hitboxSize.x * 2, hitboxSize.y * 2, hitboxSize.z * 2));
        }

    }

    /// <summary>
    /// Returns the position in which the hitbox needs to be created
    /// </summary>
    /// <returns></returns>
    private Vector3 GetHitboxPosition()
    {
        Vector3 hitboxPosition = transform.position;
        //vertical attack
        if (verticalAttackDirection == AttackVerticalDirection.Upwards)
        {
            hitboxPosition += verticalAttackOffset * 2;
        }
        else if (verticalAttackDirection == AttackVerticalDirection.Downwards)
        {
            hitboxPosition -= verticalAttackOffset *1.5f;
        }
        if (horizontalAttackDirection == AttackHorizontalDirection.Center)
        {
            return hitboxPosition;
        }
        hitboxPosition += attackOffset;
        hitboxPosition.x = transform.position.x + transform.forward.x * attackOffset.x;
        return hitboxPosition;
    }

}
