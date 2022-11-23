using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtBox : MonoBehaviour {

	public PlayerHealthManager playerHealthManager;

	public AudioClip hitClip;

	public bool isHit = false;
	public float hitStayTime;//Time to stay in hitanimation after getting hit
    

   
	[Header("HitReactionValues")]
	public float projectileHitReaction;
	public float meleeHitReaction;

	private BoxCollider2D boxCol;
	private AudioSource audioSource;
	private Animator anim;
	private PlayerMovement playerMovement;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
		anim = playerHealthManager.GetComponent<Animator>();
		audioSource = playerHealthManager.GetComponent<AudioSource>();
		playerMovement = playerHealthManager.GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Projectile")
        {
			HitReaction(projectileHitReaction, CollisionPosition(col.gameObject));
			float damage = 1f;//col.GetComponent<EnemyAttackDamage>().attackDamage;
            AddDamage(damage);
			TriggerCameraShake(0.08f,0.1f);
			Destroy(col.gameObject);
        }

		if(col.gameObject.tag == "EnemyHitBox")
		{
			HitReaction(meleeHitReaction, CollisionPosition(col.gameObject));
			float damage = 1f;//col.GetComponent<EnemyAttackDamage>().attackDamage;
			AddDamage(damage);
			TriggerCameraShake(0.1f,0.1f);

		}
    }

    private void OnTriggerExit2D(Collider2D col)
    {

    }

    private void OnTriggerStay2D(Collider2D col)
    {

    }

    private void AddDamage(float damage)
    {
		playerHealthManager.AddDamage(damage);
		if(!isHit){
			StartCoroutine(Hit());
		}
    }

	private IEnumerator Hit()
	{
		isHit = true;
		anim.SetBool("HitState",true);
		audioSource.PlayOneShot(hitClip);
		yield return new WaitForSeconds(hitStayTime);
		isHit = false;
		anim.SetBool("HitState",false);
	}

	public void TriggerCameraShake(float duration, float magnitude)
    {
        //Camera.main.GetComponent<CameraMovement>().ShakeScreen(duration, magnitude);
    }

	private void HitReaction(float reactionValue, float direction)
	{
		playerHealthManager.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * reactionValue, 0f), ForceMode2D.Force);
	}

	private float CollisionPosition(GameObject col)//-1 means attack was recieved from right and 1 means attack was recieved from left
	{
		if(transform.position.x - col.GetComponent<Collider2D>().transform.position.x < 0)
		{
			return -1;
		}else{
			return 1;
		}
		
	}


}


