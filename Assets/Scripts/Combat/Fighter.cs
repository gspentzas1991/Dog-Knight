using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public BehaviourController _behaviourController;
    private FighterAnimatorController _animatorController;
    private AudioSource _audioSource;
    private Rigidbody _rigidbody;
    public AttackController attackController;
    /// <summary>
    /// During invincibility a fighter can't be hit
    /// </summary>
    private bool isinvincible;
    private float invincibilityTimer = 0.3f;
    /// <summary>
    /// During hitstun a fighter can't control themselves
    /// </summary>
    private bool isHitStunned;
    private float hitstunTimer = 0.5f;
    //the hitstunTimer will keep increasing, so we need a reference to the 
    //initial hitstun value
    private float initialHitstun = 0.5f;

    /// <summary>
    /// The more a fighter is hurt, extra forces will be applied to them on hit, and their hitstun increases.
    /// At 100 hurtPercentage, all forces are doubled
    /// </summary>
    public float hurtPercentage = 0f;

    public string displayName;

    [SerializeField]
    private float speed=10f;
    [SerializeField]
    private float jumpForce = 15f;
    private bool hasDoubleJump = true;
    private bool isGrounded = true;

    [SerializeField] private ParticleSystem hitstunSmoke;
    [SerializeField] private GameObject deathExplosionParticles;
    [SerializeField] private AudioClip hurtAudioClip;
    [SerializeField] private AudioClip jumpAudioClip;



    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        attackController = GetComponent<AttackController>();
        _behaviourController = GetComponent<BehaviourController>();
        _audioSource = GetComponent<AudioSource>();
        initialHitstun = hitstunTimer;
        _animatorController = new FighterAnimatorController(GetComponent<Animator>());
    }

    private void Update()
    {
        //Set animator values
        _animatorController.gotHurt = isHitStunned;
        if (attackController != null)
        {
            _animatorController.attacked = attackController.isAttacking;
        }
        _animatorController.jumped = _behaviourController.jumpInput.receivedInput;
        _animatorController.horizontalSpeed = Mathf.Abs(_rigidbody.velocity.x);
        _animatorController.verticalSpeed = _rigidbody.velocity.y;
        _animatorController.UpdateValues();

        //rotate renderer
        if (_behaviourController.horizontalMovement > 0)
        {
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else if (_behaviourController.horizontalMovement < 0)
        {
            transform.eulerAngles = new Vector3(0, 270, 0);
        }

        //If we're in hitstun then we remove all buffered inputs
        if (isHitStunned)
        {
            _behaviourController.jumpInput.resetInput = true;
            _behaviourController.attackInput.resetInput = true;
        }
        else
        {
            //updateMovement
            Move();

            //Jump detection
            if (_behaviourController.jumpInput.receivedInput && (isGrounded || hasDoubleJump))
            {
                Jump();
            }

            //attack
            if (attackController != null && _behaviourController.attackInput.receivedInput)
            {
                attackController.Attack(_behaviourController.attackVerticalDirection, _behaviourController.attackHorizontalDirection);
                _behaviourController.attackInput.resetInput = true;
            }
        }
    }


    /// <summary>
    /// Changes horizontal velocity depending on input
    /// </summary>
    private void Move()
    {
        var newVelocity = _rigidbody.velocity;
        newVelocity.x = _behaviourController.horizontalMovement * speed;
        _rigidbody.velocity = newVelocity;
    }

    /// <summary>
    /// Applies vertical velocity and turns the jump input flag false
    /// </summary>
    private void Jump()
    {
        //Play the hurt audioclip
        _audioSource.clip = jumpAudioClip;
        _audioSource.pitch = 2;
        _audioSource.Play();
        _audioSource.pitch = 1;
        var newVelocity = _rigidbody.velocity;
        if (isGrounded)
        {
            isGrounded = false;
        }
        else if (hasDoubleJump)
        {
            hasDoubleJump = false;
        }
        newVelocity.y = jumpForce;
        _rigidbody.velocity = newVelocity;
        _behaviourController.jumpInput.resetInput = true;
    }

    /// <summary>
    /// Resets the isGrounded and hasDoubleJump booleans
    /// </summary>
    public void TouchedGround()
    {
        isGrounded = true;
        hasDoubleJump = true;
    }

    /// <summary>
    /// Applies force on the rigidbody, depending on the side of the attack and its attacking power
    /// </summary>
    public void GotHit(Vector3 attackerPosition, float attackPower)
    {
        if (!isinvincible)
        {
            //Play the hurt audioclip
            _audioSource.clip = hurtAudioClip;
            _audioSource.Play();
            //interupt any active attacks of the fighter
            attackController.InteruptAttack();
            //calculate the new hurtPercentage and hitstun
            hurtPercentage += attackPower / 5f;
            hitstunTimer = initialHitstun + initialHitstun * hurtPercentage / 100;
            //Stop existing coroutines (this is fine for now, but it might need a new method in the future)
            //to stop only the GotHit coroutines
            StopAllCoroutines();
            //apply hitstun and invincibility
            StartCoroutine(InvcibilityTimer(invincibilityTimer));
            StartCoroutine(HitstunTimer(hitstunTimer));
            //We reset the velocity before applying the next hit
            _rigidbody.velocity = Vector3.zero;
            //We apply an offset on the attackerPosition, so the target flies a bit upwards
            var forceDirectionOffset = new Vector3(0f, -5f, 0f);
            var forceDirection = this.transform.position - (attackerPosition + forceDirectionOffset);
            //Apply the force of the attack to the character
            _rigidbody.AddForce(forceDirection * (attackPower + attackPower*hurtPercentage/100) ,ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Makes the Fighter invincible for the specified seconds
    /// </summary>
    IEnumerator InvcibilityTimer(float timer)
    {
        isinvincible = true;
        yield return new WaitForSeconds(timer);
        isinvincible = false;
    }

    /// <summary>
    /// Makes the Fighter hitstunned for the specified seconds
    /// </summary>
    IEnumerator HitstunTimer(float timer)
    {
        isHitStunned = true;
        hitstunSmoke.Play();
        yield return new WaitForSeconds(timer);
        hitstunSmoke.Stop();
        isHitStunned = false;
    }

    public void Destroy()
    {
        Instantiate(deathExplosionParticles, transform.position, deathExplosionParticles.transform.rotation);
        Destroy(gameObject);
    }
}
