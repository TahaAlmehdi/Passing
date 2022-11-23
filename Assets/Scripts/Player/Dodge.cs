using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge : MonoBehaviour {

    public float dodgeDistance = 1f;
    public float invisibleTime = 0.08f;
    public float smokeDestroyTime = 1.5f;
    public float currentStamina = 100f;
    public float staminaCost= 20f, regainSpeed = 2f;
    public float hurtBoxDeactiveTime=0.5f;
    public GameObject hurtBox;
    public GameObject dashEffect;
    public Transform dashEffectPosition;

    public Slider staminaBar;
    public Image fillImage;

    private SpriteRenderer spriteRenderer;
    private Color fillDefaultColor, fillLowColor = Color.red;
    private PlayerHurtBox playerHurtBox;
    private GameObject t_dashEffect;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fillDefaultColor = fillImage.color;
        playerHurtBox = hurtBox.GetComponent<PlayerHurtBox>();
    }

    private void Update()
    {
        staminaBar.value = currentStamina;

        if(Input.GetButtonDown("Dodge") && currentStamina >= staminaCost)
        {
            float direction = transform.localScale.x;
            StartCoroutine(DodgeFunction(direction));
            currentStamina -= staminaCost;
        }

        if(currentStamina < 100f)
        {
            currentStamina += regainSpeed * Time.deltaTime;
        }

        if(currentStamina < staminaCost)
        {
            fillImage.color = fillLowColor;
        }
        else
        {
            fillImage.color = fillDefaultColor;
        }
    }

    private IEnumerator DodgeFunction(float dodgeDirection)
    {
        StartCoroutine(DisableHurtBox());
        Vector2 dodgePosition = new Vector2(transform.position.x + (dodgeDistance * dodgeDirection), transform.position.y);

        if(GetComponent<PlayerMovement>().onGround || !GetComponent<PlayerMovement>().onGround)
        {
            if(t_dashEffect != null)
            {
                t_dashEffect.transform.position  = dashEffectPosition.position;
                t_dashEffect.GetComponent<Animator>().SetTrigger("Start");
                if(t_dashEffect.GetComponent<AudioSource>() != null)
                {
                    t_dashEffect.GetComponent<AudioSource>().Play();
                }
            }else{
                t_dashEffect = Instantiate(dashEffect, dashEffectPosition.position, Quaternion.identity);
                t_dashEffect.transform.position  = dashEffectPosition.position;
                t_dashEffect.GetComponent<Animator>().SetTrigger("Start");
            }
        }

        spriteRenderer.enabled = false;
        transform.position = dodgePosition;
        //can control player = false;
        yield return new WaitForSeconds(invisibleTime);
        spriteRenderer.enabled = true;
    }

    private IEnumerator DisableHurtBox()
    {
        hurtBox.SetActive(false);
        yield return new WaitForSeconds(hurtBoxDeactiveTime);
        hurtBox.SetActive(true);
        if(playerHurtBox.isHit)
        {
            playerHurtBox.isHit = false;
        }
        GetComponent<Animator>().SetBool("HitState",false);
    }
}
