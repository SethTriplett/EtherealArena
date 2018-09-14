using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    // This script handles player controls including movement, aiming, and using skills

    private const float speed = 6f;
    private float movementAngle = 0f;
    private bool facingRight = true;
    private float aimingAngle = 0f;
    private bool aimingRight = true;
    private bool aiming = false;
    private float energy = 0f;
    private Transform arm;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer armRenderer;
    private Transform hand;
    public ISkill[] equipedSkills = new ISkill[8];
    private ISkill activeSkill;
    private bool stopMovement = false;

    void Start() {
        arm = transform.Find("Arm");
        hand = arm.Find("Hand");
        spriteRenderer = arm.GetComponent<SpriteRenderer>();
        armRenderer = GetComponentInChildren<SpriteRenderer>();

        // Temporary
        equipedSkills[0] = GetComponent<UseDanmaku>();

        activeSkill = equipedSkills[0];
    }

    void Update () {
        if (!stopMovement) {
            MovementAndAiming();
        }
    }

    void MovementAndAiming() {
        // Movement
        // Get left-stick input
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        // Get angle of input
        float angleRad = 0f;
        if (xInput == 0) {
            if (yInput > 0) {
                angleRad = Mathf.PI / 2f;
            } else if (yInput < 0) {
                angleRad = Mathf.PI / -2f;
            }
        } else if (yInput == 0 && xInput < 0) {
            angleRad = Mathf.PI;
        } else {
            angleRad = Mathf.Atan2(yInput, xInput);
        }
        movementAngle = angleRad * 180 / Mathf.PI;
        // Get magnitude of input
        float magnitude = Mathf.Sqrt(xInput * xInput + yInput * yInput);
        // Move based on input
        gameObject.transform.position += Vector3.right * magnitude * Mathf.Cos(angleRad) * speed * Time.deltaTime;
        gameObject.transform.position += Vector3.up * magnitude * Mathf.Sin(angleRad) * speed * Time.deltaTime;


        //Aiming
        // Get inputs
        float xRInput = Input.GetAxis("Horizontal_R");
        float yRInput = Input.GetAxis("Vertical_R");
        // Find angle
        angleRad = 0f;
        if (xRInput == 0 && yRInput == 0) {
            aiming = false;
        } else {
            aiming = true;
            if (xRInput == 0) {
                if (yRInput > 0) {
                    angleRad = Mathf.PI / 2f;
                } else if (yRInput < 0) {
                    angleRad = Mathf.PI / -2f;
                }
            } else if (yRInput == 0 && xRInput < 0) {
                angleRad = Mathf.PI;
            } else {
                angleRad = Mathf.Atan2(yRInput, xRInput);
            }
        }
        aimingAngle = angleRad * 180 / Mathf.PI;
        // Determine which way they're aiming
        if (aimingAngle > -90 && aimingAngle < 90) {
            aimingRight = true;
        } else {
            aimingRight = false;
        }


        // Set left/right facing
        if (!aiming) {
            // moving right, no aiming
            if (movementAngle > -90f && movementAngle < 90f) {
                if (magnitude > 0) {
                    facingRight = true;
                }
            // moving left, no aiming
            } else {
                facingRight = false;
            }
        // determined by aiming angle
        } else {
            facingRight = aimingRight;
        }
        // flip sprites and set values accordingly
        if (!facingRight) {
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            spriteRenderer.flipY = true;
            armRenderer.flipY = true;
            if (arm.localPosition.y > 0) {
                arm.localPosition = new Vector3(arm.localPosition.x, -arm.localPosition.y, arm.localPosition.z);
            }
        } else {
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            spriteRenderer.flipY = false;
            armRenderer.flipY = false;
             if (arm.localPosition.y < 0) {
                arm.localPosition = new Vector3(arm.localPosition.x, -arm.localPosition.y, arm.localPosition.z);
            }
        }


        // Rotate arm after determining direction
        if (!facingRight && !aiming) {
            aimingAngle = 180f;
        }
        arm.rotation = Quaternion.Euler(0f, 0f, aimingAngle);

        // Auto Release with right stick
        if (aiming) {
            activeSkill.UseSkill(hand, true);
        }
    }

    public void gainEnergy(float energy) {
        if (energy > 0) {
            this.energy += energy;
        }
    }

    public void StopMovement() {
        this.stopMovement = true;
    }

    public void AllowMovement() {
        this.stopMovement = false;
    }

}
