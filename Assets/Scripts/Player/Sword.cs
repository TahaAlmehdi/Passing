using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

    public AudioClip heavyAttackHit;
    public float attackHitFreezeDuration;
    public LayerMask hitLayer, hurtLayer;
    private PlayerMovement _playerMovement;
    private HitFreeze _hitFreeze;

    private AudioSource _source;

    private void Awake()
    {
        _playerMovement = GetComponentInParent<PlayerMovement>();
        _source = GetComponentInParent<AudioSource>();
        _hitFreeze = GetComponentInParent<HitFreeze>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != null)
        {
           // Debug.Log(col.gameObject.tag);
        }
        if (col.gameObject.tag == "Wall")
        {
            _playerMovement.TriggerCameraShake(0.15f, 0.02f);
        }


        /*if (col.gameObject.tag == "EnemyHurtBox")
        {
            _playerMovement.TriggerCameraShake(0.01f, 0.02f);
            _hitFreeze.Freeze(attackHitFreezeDuration);
            _source.PlayOneShot(heavyAttackHit);
            if(col.gameObject.GetComponentInParent<BaseEnemy>() != null)
            {
                BaseEnemy e = col.gameObject.GetComponentInParent<BaseEnemy>();
                e.OnHit(true, e.damage);
               
            }
        }*/
    }

    private void OnTriggerStay2D(Collider2D col)
    {
       

        /*if (col.gameObject.tag == "Deflectable")
        {
            GameObject t = col.gameObject.GetComponent<DeflectableProjectile>().targetProjectile;
            col.gameObject.layer = Mathf.RoundToInt(Mathf.Log(hitLayer.value, 2));
            if (t == null|| col.gameObject.GetComponent<DeflectableProjectile>().isDeflected)
            {
                return;
            }
            else
            {
                col.gameObject.GetComponent<DeflectableProjectile>().isDeflected = true;
                //col.gameObject.GetComponent<PlayerProjectile>().source = _source;
            }
            DeflectProjectile(t);
            _hitFreeze.Freeze(0.15f);
            _playerMovement.TriggerCameraShake(0.1f, 0.02f);
        }*/
    }

    private bool IsAnimPlaying(string animName)
    {
        if (_playerMovement.IsPlayingAnimation(animName))
        {
            return true;
        }

        return false;
    }

    private void DeflectProjectile(GameObject projectile)
    {
        Vector2 velocity = projectile.GetComponent<Rigidbody2D>().velocity;
        velocity *= -1f;
        projectile.GetComponent<Rigidbody2D>().velocity = velocity;
        projectile.gameObject.tag = "PlayerProjectile";
        projectile.AddComponent<PlayerProjectile>();
        projectile.GetComponent<PlayerProjectile>().source = _source;
        projectile.GetComponent<PlayerProjectile>().hitClip = heavyAttackHit;
        _hitFreeze.Freeze(0.05f);
    }

   


}
