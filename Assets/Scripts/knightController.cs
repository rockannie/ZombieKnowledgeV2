using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knightController : MonoBehaviour
{
    public float speed;

    private bool studying = false;

    [SerializeField] private Animator anim;
    private knightUIController UIscript;

    [SerializeField] private GameObject ZUIObj;
    private zombieUIController zombUICont;
    
    void Start()
    {
        UIscript = GetComponentInChildren<knightUIController>();
        zombUICont = ZUIObj.GetComponentInChildren<zombieUIController>();
    }
    
    void Update()
    {
        moveKnight();
    }

    //moves the knight character and sets animations
    private void moveKnight()
    {
        Vector3 dir = Vector3.zero;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            dir = Vector3.left;
            //dir.x = -1;
            anim.SetBool("isMoving", true);
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            dir = Vector3.up;
            //dir.y = 1;
            anim.SetBool("isMoving", true);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            dir = Vector3.right;
            //dir.x = 1;
            anim.SetBool("isMoving", true);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            dir = Vector3.down;
            //dir.y = -1;
            anim.SetBool("isMoving", true);
        }
        
        dir.Normalize();
        transform.position += dir * Time.deltaTime * speed;
        
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.D) ||
            Input.GetKeyUp(KeyCode.S) ||
            Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.UpArrow) ||
            Input.GetKeyUp(KeyCode.RightArrow) ||
            Input.GetKeyUp(KeyCode.DownArrow))
        {
            anim.SetBool("isMoving", false);
        }
        
    }

    //function to attack zombies
    private void Attack()
    {
        anim.SetBool("isAttacking", true);
        zombUICont.setHealth(-50);
    }

    //function to give brains to zombie to recruit
    private void giveBrain()
    {
        UIscript.setBrain(-25);
        zombUICont.setBrain(25);
    }
    //detecting zombie collisions and assigning what happens
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Zombie")
        {
            if (Input.GetKey(KeyCode.F))
            {
                Attack();
            }
            else if (Input.GetKey(KeyCode.G))
            {
                giveBrain();
            }
            //UIscript.setHealth(-10);
        }

        /*if (other.gameObject.tag == "Library")
        {
            StartCoroutine(Studying());
            studying = true;
        }
        else
        {
            studying = false;
        }*/
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Library")
        {
            studying = true;
            StartCoroutine(Studying());
        }
        else
        {
            studying = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Library")
        {
            studying = false;
        }
    }

    IEnumerator Studying()
    {
        while (studying)
        {
            UIscript.setBrain(1);
            yield return new WaitForSeconds(1f);
        }
    }
}
