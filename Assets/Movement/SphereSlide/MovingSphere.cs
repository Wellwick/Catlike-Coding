using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere : MonoBehaviour {
    [SerializeField, Range(0f, 100f)]
    private float maxSpeed = 10f;

    [SerializeField, Range(0f, 100f)]
    private float maxAcceleration = 10f;

    [SerializeField, Range(0f, 1f)]
    private float bounciness = 0.5f;

    [SerializeField]
    private Rect allowedArea = new Rect(-5f, -5f, 10f, 10f);

    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);
        Vector3 desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
        Vector3 newPos = transform.localPosition + velocity * Time.deltaTime;
        if (!allowedArea.Contains(new Vector2(newPos.x, newPos.z))) {
            if (newPos.x < allowedArea.xMin) {
                newPos.x = allowedArea.xMin;
                velocity.x *= -1f * bounciness;
            } else if (newPos.x > allowedArea.xMax) {
                newPos.x = allowedArea.xMax;
                velocity.x *= -1f * bounciness;
            }
            if (newPos.z < allowedArea.yMin) {
                newPos.z = allowedArea.yMin;
                velocity.z *= -1f * bounciness;
            } else if (newPos.z > allowedArea.yMax) {
                newPos.z = allowedArea.yMax;
                velocity.z *= -1f * bounciness;
            }
            newPos.x = Mathf.Clamp(newPos.x, allowedArea.xMin, allowedArea.xMax);
            newPos.z = Mathf.Clamp(newPos.z, allowedArea.yMin, allowedArea.yMax);
        }
        transform.localPosition = newPos;
    }
}
