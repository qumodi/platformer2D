using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum Direction { none, left, right }
public enum State { idle, running, jumping, falling, hurt }


public class PlayerScript : MonoBehaviour
{
    [SerializeField] LayerMask ground;

    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D col;


    public float speed = 5;
    public float jumpPower = 8f;
    public float rotationSpeed = 5f;

    public State state = State.idle;
    public Direction rotationDir = Direction.none;
    public Vector3 showVelocity;

    [SerializeField] private Text collectableNumber;
    public int cherries;

    public int hurtPower = 10;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (state != State.hurt)
        {
            GetInput();
        }
        RotatePlayer();
        StateSwitch();

        showVelocity = rb.velocity;

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals("Collectable"))
        {
            cherries++;
            //other.gameObject.SetActive(false);
            Destroy(collider.gameObject);
            collectableNumber.text = cherries.ToString();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (state == State.falling)
            {
                Destroy(collision.gameObject);
                Jump();
            }
            else
            {
                state = State.hurt;
                Vector2 hurtVector = this.gameObject.transform.position - collision.transform.position;
                rb.velocity = hurtVector.normalized * hurtPower;
                print(hurtVector);
            }
        }
    }

    public void GetInput()
    {
        float hDirection = 0;

        hDirection = Input.GetAxis("Horizontal");

        //print(hDirection);
        //float vDirection = Input.GetAxis("Vertical");

        if (hDirection > 0)//Input.GetKey(KeyCode.D)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            rotationDir = Direction.right;
            //animator.SetBool("state");
        }
        else if (hDirection < 0)//Input.GetKey(KeyCode.A)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            rotationDir = Direction.left;

            //animator.SetBool("running", true);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);

            //animator.SetBool("running", false);

        }


        //if (col.IsTouchingLayers(ground) && rotationDir == Direction.left) rb.velocity = new Vector2(rb.velocity.x, Mathf.Abs(rb.velocity.y));

        if (Input.GetButtonDown("Jump") && col.IsTouchingLayers(ground))
        {
            Jump();
        }
    }

    private void RotatePlayer()
    {
        switch (rotationDir)
        {
            case Direction.left:
                if (transform.localScale.x <= 0.1 && transform.localScale.x >=-0.1) { transform.localScale = new Vector2(-0.1f, 1f); }
                transform.localScale += new Vector3(-rotationSpeed * Time.deltaTime, 0, 0);

                if (transform.localScale.x <= -1)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    rotationDir = Direction.none;
                }
                break;
            case Direction.right:
                if (transform.localScale.x <= 0.1 && transform.localScale.x >=-0.1) { transform.localScale = new Vector2(0.1f, 1f); }
                transform.localScale += new Vector3(rotationSpeed * Time.deltaTime, 0, 0);

                if (transform.localScale.x >= 1)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    rotationDir = Direction.none;
                }
                break;
        }
    }

    private void Jump()
    {
        rb.velocity += new Vector2(0, jumpPower);
        state = State.jumping;
    }

    private void StateSwitch()
    {
        /*if (state == State.hurt && Mathf.Abs(rb.velocity.x) < .1f) { state = State.idle; }
        else if (rb.velocity.y > 0.2f) { state = State.jumping; }
        else if (rb.velocity.y < 0 && !col.IsTouchingLayers(ground)) { state = State.falling; }
        else if (Mathf.Abs(rb.velocity.x) > .1f) { state = State.running; }
        else { state = State.idle; }*/


        if (state == State.jumping)
        {
            if (rb.velocity.y < 0) { state = State.falling; }
        }
        else if (state == State.falling)
        {
            if (col.IsTouchingLayers(ground)) { state = State.idle; }
        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            state = State.running;
        }
        else
        {
            state = State.idle;
        }

        animator.SetInteger("state", (int)state);

    }
}
