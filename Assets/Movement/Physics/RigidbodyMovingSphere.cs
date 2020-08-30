﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyMovingSphere : MonoBehaviour {
    [SerializeField, Range(0f, 100f)]
    private float maxSpeed = 10f;

    [SerializeField, Range(0f, 100f)]
    private float maxAcceleration = 10f, maxAirAcceleration = 1f;

    [SerializeField, Range(0f, 10f)]
    float jumpHeight = 2f;

    [SerializeField, Range(0, 5)]
    private int maxAirJumps = 0;

    private Vector3 velocity, desiredVelocity;

    private bool desiredJump;
    private int jumpPhase;

    private bool onGround;

    private Rigidbody body;

    // Start is called before the first frame update
    void Start() {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);
        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

        desiredJump |= Input.GetButtonDown("Jump");
    }

    private void FixedUpdate() {
        UpdateState();
        float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        if (desiredJump) {
            desiredJump = false;
            Jump();
        }

        body.velocity = velocity;
        onGround = false;
    }

    private void UpdateState() {
        velocity = body.velocity;
        if (onGround) {
            jumpPhase = 0;
        }
    }

    private void Jump() {
        if (onGround || jumpPhase < maxAirJumps) {
            jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            if (velocity.y > 0f) {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            velocity.y += jumpSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay(Collision collision) {
        EvaluateCollision(collision);
    }

    private void EvaluateCollision(Collision collision) {
        for (int i = 0; i < collision.contactCount; i++) {
            Vector3 normal = collision.GetContact(i).normal;
            onGround |= normal.y >= 0.9f;
        }
    }
}