using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CPlayerCharacter : CCharacter
{
    private KeyCode sprintButton = KeyCode.LeftShift;
    private KeyCode attackButton = KeyCode.Mouse0;
    private KeyCode guardButton = KeyCode.Mouse1;
    private KeyCode dodgeButton = KeyCode.Space;
    private KeyCode fightingButton = KeyCode.Q;

    private Vector2 input;

    protected override void Init()
    {
        lmask = LayerMask.GetMask("Enemy");
    }

    protected override void FUpdate()
    {
        if (!isDeath)
        {

            if (Input.GetKeyDown(fightingButton))
            {
                if (!fighting)
                {
                    fighting = true;
                    //weapon.gameObject.SetActive(true);
                    anim.SetBool("Fighting", true);
                    speedCompensate = true;
                }
                else
                {
                    fighting = false;
                    //weapon.gameObject.SetActive(false);
                    anim.SetBool("Fighting", false);
                    speedCompensate = false;
                }
            }

            InputMove(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            //Attack
            if (fighting && Input.GetKeyDown(attackButton) && !EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log(executeObjects.Count);
                if (executeObjects.Count != 0)
                {
                    isExecuting = true;
                    foreach (var item in executeObjects)
                    {
                        item.isExecuted = true;
                        transform.LookAt(item.transform);
                        item.transform.LookAt(transform);
                    }
                    executeObjects.Clear();
                }
                else
                    anim.SetTrigger("Attack");

            }


            if (Input.GetKeyDown(dodgeButton))
            {
                anim.SetTrigger("Dodge");
            }

            if (Input.GetKey(guardButton))
            {
                if (!isGuard && energy >= 0.4f)
                {
                    anim.SetBool("Guard", true);
                }

            }
            else
            {
                if (isGuard)
                {
                    isGuard = false;
                    anim.SetBool("Guard", false);
                }
            }
        }
    }


    public void InputMove(float ix, float iy)
    {
        if (isMoving)
        {
            input.x = ix;
            input.y = iy;

            // set speed to both vertical and horizontal inputs
            if (useCharacterForward)
                speed = Mathf.Abs(input.x) + input.y;
            else
                speed = Mathf.Abs(input.x) + Mathf.Abs(input.y);

            speed = Mathf.Clamp(speed, 0f, 1f);
            speed = Mathf.SmoothDamp(anim.GetFloat("Speed"), speed, ref velocity, 0.1f);
            anim.SetFloat("Speed", speed);

            if (input.y < 0f && useCharacterForward)
                direction = input.y;
            else
                direction = 0f;

            anim.SetFloat("Direction", direction);

            // set sprinting
            isSprinting = (Input.GetKey(sprintButton) && input != Vector2.zero && direction >= 0f);
            anim.SetBool("isSprinting", isSprinting);

            // Update target direction relative to the camera view (or not if the Keep Direction option is checked)
            UpdateTargetDirection();
            if (input != Vector2.zero && targetDirection.magnitude > 0.1f)
            {
                Vector3 lookDirection = targetDirection.normalized;
                freeRotation = Quaternion.LookRotation(lookDirection, transform.up);
                var diferenceRotation = freeRotation.eulerAngles.y - transform.eulerAngles.y;
                var eulerY = transform.eulerAngles.y;

                if (diferenceRotation < 0 || diferenceRotation > 0) eulerY = freeRotation.eulerAngles.y;
                var euler = new Vector3(0, eulerY, 0);

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler), turnSpeed * turnSpeedMultiplier * Time.deltaTime);
            }
        }
    }

    public virtual void UpdateTargetDirection()
    {
        if (!useCharacterForward)
        {
            turnSpeedMultiplier = 1f;
            var forward = mainCamera.transform.TransformDirection(Vector3.forward);
            forward.y = 0;

            //get the right-facing direction of the referenceTransform
            var right = mainCamera.transform.TransformDirection(Vector3.right);

            // determine the direction the player will face based on input and the referenceTransform's right and forward directions
            targetDirection = input.x * right + input.y * forward;
        }
        else
        {
            turnSpeedMultiplier = 0.2f;
            var forward = transform.TransformDirection(Vector3.forward);
            forward.y = 0;

            //get the right-facing direction of the referenceTransform
            var right = transform.TransformDirection(Vector3.right);
            targetDirection = input.x * right + Mathf.Abs(input.y) * forward;
        }
    }
}
