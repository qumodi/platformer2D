using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogScript : MonoBehaviour
{

    [SerializeField] private Collider2D col;
    [SerializeField] private LayerMask ground;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [SerializeField] private float WaypointLeft;
    [SerializeField] private float WaypointRight;

    [SerializeField] private float jumpLenght;
    [SerializeField] private float jumpHeight;

    [SerializeField] private bool facingLeft = true;

    void Update()
    {
        if (animator.GetBool("Jumping"))
        {
            if (rb.velocity.y < 0)
            {
                animator.SetBool("Jumping", false);
                animator.SetBool("Falling", true);
            }

        }
        if (animator.GetBool("Falling") && col.IsTouchingLayers(ground))
        {
            animator.SetBool("Falling", false);
        }
    }

    private void Move()
    {
        //left direction
        if (facingLeft)
        {
            if (transform.localScale.x != 1)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            //перевірка меж
            if (this.transform.position.x > WaypointLeft)
            {
                if (col.IsTouchingLayers(ground))
                {   //jump
                    rb.velocity = new Vector2(-jumpLenght, jumpHeight);
                    animator.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        //right direction
        else
        {   //перевірка повороут
            if (transform.localScale.x != -1)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            if (this.transform.position.x < WaypointRight)
            {
                if (col.IsTouchingLayers(ground))
                {   //jump

                    rb.velocity = new Vector2(jumpLenght, jumpHeight);
                    animator.SetBool("Jumping", true);

                }
            }
            else
            {
                facingLeft = true;
            }
        }


    }
}
