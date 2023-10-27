using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Float`s")]
    public float speed;
    public float smoothRotRime;
    public float turnsmoothvelocity;
    public float gravidade = 150;
    public float coliderradius;
    public float timer = 5f;
    
    [Header("Int`s")]
    public int dano = 15;
    public int life;
    
    [Header("Bool`s")]
    public bool iswalking;
    public bool waitfor;
    public bool ishiting;
    public bool isdead;
    
    [Header("Component`s")]
    public CharacterController Controler;
    public Transform cam;
    public Animator anim;
    public Vector3 moveDirection;
    
    [Header("List")]
    public List<Transform> enemylist = new List<Transform>();

    void Start()
    {
        anim = GetComponent<Animator>();
        Controler = GetComponent<CharacterController>();
        cam = Camera.main.transform;
    }
    
    
    void Update()
    {
        if (!isdead)
        {
            move();
            GetMoouseinput();
        }

        if (life <= 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SceneManager.LoadScene(0);
            }
        }

        if (life <= 0)
        {
            Destroy(gameObject.GetComponent<CharacterController>());
        }
    }

    void move()
    {
        if (Controler.isGrounded)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(x, 0f, y);

            if (direction.magnitude > 0)
            {

                if (!anim.GetBool("Atack"))
                {
                    float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

                    float smothAngle =
                        Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnsmoothvelocity, smoothRotRime);
                    transform.rotation = Quaternion.Euler(0f, smothAngle, 0f);

                    moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward * speed;
                
                    anim.SetInteger("Transition", 2);
                    iswalking = true;
                }
                else
                {
                    anim.SetBool("Walking", false);
                    moveDirection = Vector3.zero;
                }

                
            }
            else if (iswalking)
            {
                anim.SetBool("Walking", false);
                moveDirection = Vector3.zero;
                anim.SetInteger("Transition", 0);
                iswalking = false;
            }
            
        }

        moveDirection.y -= gravidade * Time.deltaTime;
        Controler.Move(moveDirection * Time.deltaTime);
    }
 

    void GetMoouseinput()
    {
        if (Controler.isGrounded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (anim.GetBool("Walking"))
                {
                    anim.SetBool("Walking", false);
                    anim.SetInteger("Transition", 0);
                }

                if (!anim.GetBool("Walking"))
                {
                    StartCoroutine("atack"); 
                }
            }
        }
    }

    IEnumerator atack()
    {
        if (!waitfor && !ishiting)
        {
            waitfor = true;
            anim.SetBool("Atack", true);
            anim.SetInteger("Transition", 1);
            yield return new WaitForSeconds(0.52f);

            GetEnemy();

            foreach (Transform enemys in enemylist)
            {
                Enemy e = enemys.GetComponent<Enemy>();
                if (e != null)
                {
                    e.getHit(dano);
                }
            }

            yield return new WaitForSeconds(1f);

            anim.SetInteger("Transition", 0);
            anim.SetBool("Atack", false);
            waitfor = false;
        }
    }

    void GetEnemy()
    {
        enemylist.Clear();
        foreach(Collider c in Physics.OverlapSphere((transform.position + transform.forward * coliderradius),coliderradius))
        {
            if (c.gameObject.CompareTag("Enemy"))
            {
                enemylist.Add(c.transform);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, coliderradius);
    }
    
    public void getHit(int dmg)
    {
        life -= dmg;
        if (life > 0)
        {
            StopCoroutine("Matack");
            anim.SetInteger("Transition", 3);
            ishiting = true;
            StartCoroutine("recovery");
        }
        else
        {
            isdead = true;
            anim.SetTrigger("dead");
        }
    }

    IEnumerator recovery()
    {
        yield return new WaitForSeconds(1f);
        anim.SetInteger("Transition", 0);
        ishiting = false;
        anim.SetBool("Atack",false);
    }

}
