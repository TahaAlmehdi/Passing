using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {

    public float spinRate  = 1f;
    public float destroyTime = 5f;

    public AudioSource source;
    public AudioClip hitClip;

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddTorque(spinRate);
        StartCoroutine(Slowdown());
    }

  
    private void OnTriggerEnter2D(Collider2D col)
    { 
       /*if(col.gameObject.tag == "EnemyHurtBox" )
        {
            BaseEnemy e = col.gameObject.GetComponentInParent<BaseEnemy>();
            e.OnHit(true, e.damage);
            source.PlayOneShot(hitClip,0.5f);        
            Destroy(gameObject);
        }*/
    }

    private IEnumerator Slowdown()
    {
        yield return new WaitForSeconds(destroyTime);
        GetComponent<Rigidbody2D>().gravityScale = 0.1f;
        Destroy(gameObject, destroyTime);
    }
}
