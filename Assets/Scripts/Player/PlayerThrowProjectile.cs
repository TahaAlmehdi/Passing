using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerThrowProjectile : MonoBehaviour {

    public GameObject projectile;
    public Transform throwPosition;
    public float throwForce;
    public float throwInterval;
    public int noOfProjectiles;

    public Text projText;

    private bool canThrow = true;
    private Vector2 direction = Vector2.zero;

    private void Update()
    {
        if(Input.GetButtonDown("Throw") && noOfProjectiles > 0)
        {
            GetComponent<Animator>().SetTrigger("Throw");
        }

        projText.text = " Shuriken : " + noOfProjectiles;

        Vector2 inputVector = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        inputVector.Normalize();
        if(inputVector != Vector2.zero)
        {
            direction = inputVector;
            
        }else{
            direction =  new Vector2(Mathf.Sign(transform.localScale.x), 0);
        }
    }

    public void Throw()
    {
        StartCoroutine(ThrowProjectile());
    }

    private IEnumerator ThrowProjectile()
    { 
        if(canThrow)
        {
            canThrow = false;
            GameObject t = Instantiate(projectile, throwPosition.position, throwPosition.rotation);
            t.transform.rotation = LookAtTarget();
            direction += new Vector2(Random.Range(-0.01f,0.01f),Random.Range(-0.01f,0.01f));
            t.GetComponent<PlayerProjectile>().source = GetComponent<AudioSource>();
            Rigidbody2D tBody = t.GetComponent<Rigidbody2D>();
            tBody.velocity = throwForce * direction;
            noOfProjectiles -= 1;
            yield return new WaitForSeconds(throwInterval);
            canThrow = true;
           
        }
    }

    private Quaternion LookAtTarget()
    {
        Vector2 dir = (Vector3)direction.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
       
    }

    

}
