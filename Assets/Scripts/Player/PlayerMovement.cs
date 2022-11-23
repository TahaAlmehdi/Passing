using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

    [Header("Movement")]
    public float runAcceleration = 5f, maxRunSpeed = 3f, runStopSpeed = 5f;
    public float airMoveSpeed,maxAirMoveSpeed;
    private Vector2 lastVelocity;//Velocity right before InputX becomes 0 or the horizontal button is not pressed
    public float attackPushAmount = 50f;
    public AudioSource footStepAudioSource;

    [Header("Jump")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float jumpForce = 1f;
    public float maxJumpDistance, jumpXReducingSpeed, jumpFactor = 1f;
    private bool jumpButtonPressed = false;
    private Vector2 lgVelocity = Vector2.zero;
    public PlatformToggle groundActivator;
    //public int jumpCount = 0;

    [Header("GroundDetect")]
    public Transform legPos;
    public float groundDetectRadius = 0.05f;
    public LayerMask groundLayer;
    public bool onGround = true;

    [Header("AudioClips")]
    public AudioClip jumpClip;
    public AudioClip lightAttackClip;

    private Animator anim;
    private Rigidbody2D body;
    public bool isFacingRight = false;
    private AudioSource audioSource;
    private Vector2 inputVector = Vector2.zero;
    public bool isPlayerAttacking = false;//Set in animation
    public PlayerHurtBox playerHurtBox;
    private float moveAcceleration;

    private bool isHit = false;
    //private bool onRoofBox = false;
    private GameObject roofObject;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

    }

    private void FixedUpdate()
    {
        IsPlayerAttacking();
        isHit = playerHurtBox.isHit;

        if (isHit)
        {
            return;
        }

        if (onGround)
        {
            if(!isPlayerAttacking)
            {
                MoveOnGround();
            }
        }
        else
        {
            MoveInAir();
        }



        //Jump

        if (jumpButtonPressed && onGround && !isPlayerAttacking)
        {
            lgVelocity = body.velocity;
            Jump();
        }

        if (body.velocity.y < 0 && inputVector.x == 0f)
        {
            body.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if(body.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            body.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

        if (System.Math.Abs(inputVector.x) > Mathf.Epsilon)
        {
            anim.SetFloat("XVelocity", Mathf.Abs(inputVector.x) * 10);
        }
        else
        {
            anim.SetFloat("XVelocity", 0);
        }
    }

    private void Update()
    {

        inputVector = new Vector2(Input.GetAxisRaw("Horizontal") * Time.fixedDeltaTime, Input.GetAxis("Vertical"));
        onGround = Physics2D.OverlapCircle(legPos.position, groundDetectRadius, groundLayer);

        if(isFacingRight && inputVector.x < 0)
        {
            flip();
        }else if(!isFacingRight && inputVector.x > 0)
        {
            flip();
        }
        // if (isFacingRight && lastVelocity.x < 0)
        // {
        //     flip();
        // }
        // else if (!isFacingRight && lastVelocity.x > 0)
        // {
        //     flip();
        // }

        if ((inputVector.x == 0f && onGround && !jumpButtonPressed) || isPlayerAttacking)
        {
            //body.velocity = Vector2.zero;
            StopGroundMovement();
            footStepAudioSource.mute = true;
        }else
        {
                lastVelocity = body.velocity;
        }

        if (onGround)
        {
            body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -maxRunSpeed, maxRunSpeed), Mathf.Clamp(body.velocity.y, -jumpForce -fallMultiplier, jumpForce + fallMultiplier));
        }
        else
        {
            body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -maxAirMoveSpeed, maxAirMoveSpeed), Mathf.Clamp(body.velocity.y, -jumpForce - fallMultiplier, jumpForce + fallMultiplier));
        }

        if (isHit)//
            return;

        if(Input.GetButtonDown("Jump"))
        {
            jumpButtonPressed = true;
        }

        if(Input.GetButtonUp("Jump"))
        {
            jumpButtonPressed = false;
        }

        anim.SetFloat("YVelocity", body.velocity.y);
        anim.SetBool("OnGround", onGround);


        //Light Attack
        if (Input.GetButton("Attack"))
        {
            LightAttack();
            StopGroundMovement();
        }

        if(isPlayerAttacking&&onGround)
        {
            if (!isPlayerAttacking)
                isPlayerAttacking = true;
            StopGroundMovement();
        }
    }

    private void StopGroundMovement()
    {
        //body.velocity = Vector2.Lerp(body.velocity, new Vector2(0f, body.velocity.y), runStopSpeed * Time.fixedDeltaTime);
        body.velocity = new Vector2(0f, body.velocity.y);
    }

    private void Jump()
    {
        if (isPlayerAttacking)
            return;

        jumpButtonPressed = false;
        Vector2 jumpVector = Vector2.zero;
        jumpVector.x = Mathf.Clamp(lgVelocity.x * jumpFactor, -maxJumpDistance, maxJumpDistance);
        jumpVector.y = jumpForce;
        body.velocity = jumpVector;
        audioSource.PlayOneShot(jumpClip);
    }

    private void MoveOnGround()
    {
        if (!CanPlayerMove()||isPlayerAttacking)
            return;

        Vector2 finalVelocity= body.velocity;
        float sign = Mathf.Sign(inputVector.x);

        //newVelocity.x += inputVector.x * runAcceleration;
        //newVelocity.y = body.velocity.y;
        if(sign < 0 && body.velocity.x > 0)//if moving right but left key pressed
        {
            finalVelocity.x = 0f;
        }
        else if(sign > 0 && body.velocity.x < 0)//if moving left and right key pressed
        {
            finalVelocity.x = 0f;
        }

        finalVelocity.x += inputVector.x * runAcceleration;
        body.velocity = finalVelocity;
      
    }

    private void MoveInAir()
    {
        if (Mathf.Approximately(inputVector.x, 0f))
        {
            body.velocity = Vector2.Lerp(body.velocity, new Vector2(0f, body.velocity.y), jumpXReducingSpeed * Time.fixedDeltaTime);
        }

        Vector2 newVelocity = body.velocity;
        newVelocity.x += inputVector.x * airMoveSpeed;
        body.velocity = newVelocity;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground" || col.gameObject.tag == "Wall")
        {
            if (col.relativeVelocity.magnitude > 4f)
            {
               // TriggerCameraShake(0.1f, 0.02f);
            }
            //jumpCount = 0;
        }

        if(col.gameObject.tag == "Bamboo")
        {
            anim.SetLayerWeight(1, 1f);
        }

        if (col.gameObject.tag == "Ground")
        {
            if (col.gameObject.GetComponent<PlatformToggle>() != null)
            {
                //onRoofBox = true;
                roofObject = col.gameObject;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bamboo")
        {
            anim.SetLayerWeight(1, 1f);
        }

        if (col.gameObject.tag == "Ground")
        {
            if (col.gameObject.GetComponent<PlatformToggle>() != null)
            {
               // onRoofBox = true;
                roofObject = col.gameObject;
            }
        }


    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.tag == "Bamboo")
        {
            anim.SetLayerWeight(1,0f);
        }

        if (col.gameObject.tag == "Ground")
        {
            if (col.gameObject.GetComponent<PlatformToggle>() != null)
            {
                //onRoofBox = false;
                roofObject = null;
            }
        }
    }

    public void flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void TriggerCameraShake(float duration, float magnitude)
    {
       // Camera.main.GetComponent<CameraMovement>().ShakeScreen(duration, magnitude);

    }

    private void LightAttack()
    {
        anim.SetTrigger("LightAttack");
        //audioSource.PlayOneShot(lightAttackClip);
    }

    private void HeavyAttack()
    {
        anim.SetTrigger("HeavyAttack");
        TriggerCameraShake(0.2f, 0.05f);
    }

    public bool IsPlayingAnimation(string animName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(animName))
        {
            return true;
        }
        return false;
    }
    public void HeavyAttackShake()
    {
        TriggerCameraShake(0.2f, 0.03f);
    }

    private bool CanPlayerMove()
    {
        if (IsPlayingAnimation("Ninja_HeavyAttack") || IsPlayingAnimation("Ninja_LightAttack"))
        {
            return false;
        }

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(legPos.position, groundDetectRadius);
    }

    public void PlayAudio(AudioClip clip)
    {
        audioSource.PlayOneShot(clip,0.3f);
    }


    private void IsPlayerAttacking()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            isPlayerAttacking = true;
        }
        else
        {
            isPlayerAttacking = false;
        }
    }

    public void PlayerAttackPush()
    {
        if (inputVector.x != 0.0f)
        {
            float x = Mathf.Sign(transform.localScale.x);
            //body.AddForce(new Vector2(attackPushAmount * x, 0f), ForceMode2D.Force);
        }

    }

    public void PlayFootStepSound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip,0.3f);
    }

  

}
