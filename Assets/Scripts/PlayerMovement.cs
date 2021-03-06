﻿using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Transform cam;
    public Animator anim;

    public AudioSource footSteps;
    public AudioSource punchSound;
    public AudioSource jumpSound;

    public GameEvent onCustomerCollision;
    public ParticleSystem runDust;
    public ParticleSystem collisionParticles;

    public float speed = 5f;
    public float turnSmoothTime = 0.1f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public float fallTreshold = -15f;

    CharacterController controller;
    Vector3 moveDir = Vector3.zero;
    Vector3 velocity = Vector3.zero;
    
    float turnSmoothVelocity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnMove(InputValue value)
    {
        var val = value.Get<Vector2>();
        moveDir = new Vector3(val.x, 0, val.y).normalized;
    }

    private void OnLook(InputValue value)
    {
        var lookDelta = value.Get<Vector2>();
    }

    private void OnJump ()
    {
        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            anim.SetBool("Jump", true);
            jumpSound.Play();
            footSteps.Stop();
            runDust.Stop();
        }
    }

    private void Update()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            anim.SetBool("Jump", false);
            velocity.y = -2f;
        }
        else if (!anim.GetBool("Jump") && velocity.y < fallTreshold)
        {
            anim.SetBool("Jump", true);
            footSteps.Stop();
            runDust.Stop();
        }

        Vector3 processedDir = Vector3.zero;

        if (moveDir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            processedDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            processedDir *= speed;

            anim.SetBool("Run", true);

            if (!footSteps.isPlaying && controller.isGrounded)
            {
                footSteps.Play();
                runDust.Play();
            }
        }
        else
        {
            anim.SetBool("Run", false);
            footSteps.Stop();
            runDust.Stop();
        }

        velocity.y += gravity * Time.deltaTime;

        if (anim.GetBool("Jump"))
        {
            footSteps.Stop();
            runDust.Stop();
        }

        controller.Move((velocity + processedDir) * Time.deltaTime);    
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Customer"))
        {
            Instantiate(collisionParticles, coll.transform.position, coll.transform.rotation);
            if (coll.GetComponent<CustomerController>().OnPlayerCollision(transform.position))
            {
                onCustomerCollision.Raise();
                punchSound.Play();
            }       
        }
    }
}
