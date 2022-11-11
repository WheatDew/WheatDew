using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

//[AddComponentMenu("")] // Don't display in add component menu
public class CharacterMovement : MonoBehaviour
{
    public bool useCharacterForward = false;
    public bool lockToCameraForward = false;
    public float turnSpeed = 10f;

    private KeyCode sprintButton = KeyCode.LeftShift;
    private KeyCode attackButton = KeyCode.Mouse0;
    private KeyCode guardButton = KeyCode.Mouse1;
    private KeyCode dodgeButton = KeyCode.Space;
    private KeyCode fightingButton = KeyCode.Q;

    private float turnSpeedMultiplier;
    private float speed = 0f;
    private float direction = 0f;
    private bool isSprinting = false;
    [HideInInspector] public Animator anim;
    private Vector3 targetDirection;
    private Vector2 input;
    private Quaternion freeRotation;
    private Camera mainCamera;
    private float velocity;
    public static bool isMoving=true;


    //额外的增量
    public CWeapon weapon;
    [HideInInspector] public bool fighting=false;
    public bool isPlayer = false;
    public bool speedCompensate=false;
    public bool isAI = false;
    [HideInInspector] public GameObject noticed;
    private Rigidbody body;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public bool isDeath=false;
    [HideInInspector] public bool isAIMove = true;

    [HideInInspector] public CBloodBar bloodBar;
    [HideInInspector] public float health = 1;
    [HideInInspector] public float energy = 1;

    [HideInInspector] public bool isGuard=false;

    public Transform startPoint;

    public Transform weaponStart, weaponEnd;
    public List<Vector3> points;
    public bool  isWeaponDetecting=false;

    // Use this for initialization
    void Start()
    {
        //初始化数据
        anim = GetComponent<Animator>();
        mainCamera = Camera.main;

        body = GetComponent<Rigidbody>();
        if (GetComponent<NavMeshAgent>() != null)
        {
            agent = GetComponent<NavMeshAgent>();
            //agent.destination = transform.position;
            //agent.isStopped = true;
            agent.destination=startPoint.position;
        }


        if (isAI)
            AIBehaviour();

    }

    //AI异步等待
    public async void AIBehaviour()
    {
        float guardTime=0;
        float dodgeTime = 0;
        float strollTime = 0;

        while(!isDeath)
        {
            guardTime += Time.deltaTime;
            dodgeTime += Time.deltaTime;
            strollTime += Time.deltaTime;

            if (noticed != null)
            {
                if (Vector3.Distance(agent.destination,startPoint.position)<1)
                {
                    agent.destination=noticed.transform.position;
                }

                if (!noticed.CompareTag("Death"))
                {
                    if (!fighting)
                    {
                        fighting = true;
                        anim.SetBool("Fighting", true);
                    }

                }

                if (!noticed.CompareTag("Death") && (strollTime > Random.Range(1, 3) || Vector3.Distance(agent.destination, transform.position) < 0.2f))
                {
                    strollTime = 0;
                    //agent.destination = noticed.transform.position;
                    Vector2 target_v2 = SMath.GetRandomValueOnCircle(4);
                    Vector3 target_v3 = new Vector3(target_v2.x, 0, target_v2.y);
                    //Debug.Log(target_v3);
                    agent.destination = noticed.transform.position + target_v3;
                }


                if (Vector3.Distance(noticed.transform.position, transform.position) <= 2f)
                {
                    agent.destination = noticed.transform.position;

                    if (!noticed.CompareTag("Death"))
                    {
                        //Debug.LogFormat("目标位置:{0};自身位置{1};距离{2}", noticed.transform.position, transform.position, Vector3.Distance(noticed.transform.position, transform.position));
                        if (Vector3.Distance(noticed.transform.position, transform.position) < 1f)
                        {
                            if (Random.value > 0.8f && dodgeTime > Random.Range(1, 3))
                            {
                                dodgeTime = 0;
                                anim.ResetTrigger("Attack");
                                anim.ResetTrigger("Dodge");
                                if (Random.value > 0.9f)
                                    anim.SetTrigger("Dodge");
                            }
                            else if (Random.value > 0.9f && guardTime > Random.Range(2, 5))
                            {
                                guardTime = 0;
                                anim.SetBool("Guard", true);
                                await new WaitForSeconds(Random.Range(0.5f, 2));
                                anim.SetBool("Guard", false);
                            }
                            else
                            {
                                agent.destination = Vector3.Lerp(noticed.transform.position, transform.position, 0.2f);
                                anim.SetTrigger("Attack");
                            }

                        }
                        
                    }
                    else if (fighting)
                    {
                        fighting = false;
                        anim.SetBool("Fighting", false);
                        agent.destination = startPoint.position;
                        noticed = null;
                    }

                }

                if (isAIMove&&noticed!=null)
                    AIMove(noticed.transform.position,1,0);
            }
            else
            {
                if (fighting)
                {
                    fighting = false;
                    anim.SetBool("Fighting", false);
                }
                AIMove(agent.destination,0.2f,0);
            }
            
            await new WaitForUpdate();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        energy += 0.05f * Time.fixedDeltaTime;
        if(energy > 1)
            energy = 1;

        if (isPlayer && !isDeath)
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


            //速度补偿
            //if (speedCompensate)
            //    body.velocity = transform.TransformDirection(speed * Vector3.forward * 3);

            //Attack
            if (fighting && Input.GetKeyDown(attackButton) && !EventSystem.current.IsPointerOverGameObject())
            {
                anim.SetTrigger("Attack");

            }


            if (Input.GetKeyDown(dodgeButton))
            {
                anim.SetTrigger("Dodge");
            }

            if (Input.GetKey(guardButton))
            {
                if (!isGuard)
                {
                    isGuard = true;
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

        


        //限制y轴速度
        if(body.velocity.y>0.1f)
        {
            Vector3 temp = body.velocity;
            temp.y = 0.1f;
            body.velocity = temp;
        }
    }


    //射线检测判定
    public async void WeaponDetection()
    {
        points = new List<Vector3>();
        while (isWeaponDetecting)
        {
            points.Add(weaponStart.position);
            points.Add(weaponEnd.position);
            await new WaitForFixedUpdate();
        }

    }

    IEnumerator BackMove()
    {
        //等待某个协程执行完毕后再执行后续代码
        yield return new WaitForSeconds(0.5f);
        agent.destination=transform.position+(noticed.transform.position-transform.position).normalized*5;
    }

    #region 移动

    public void AIMove(Vector3 target,float maxSpeed,float minSpeed)
    {
        speed = Vector3.Distance(target, transform.position)*0.1f;
        speed = Mathf.Clamp(speed, 0f, 1f);
        speed = Mathf.SmoothDamp(anim.GetFloat("Speed"), speed, ref velocity, 0.1f);
        if (speed > maxSpeed)
            speed = maxSpeed;
        if (speed < minSpeed)
            speed = 0;

        anim.SetFloat("Speed", speed);
        //transform.LookAt(agent.destination);
    }

    public void InputMove(float ix,float iy)
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

    #endregion

    #region 工具组
    public void DisplayWeapon()
    {
        weapon.gameObject.SetActive(true);

        //WeaponBoxActive();
    }

    public void HiddenWeapon()
    {

        weapon.gameObject.SetActive(false);

        //WeaponBoxFreeze();
    }

    public void WeaponBoxActive()
    {
        //weapon.tag = "Weapon";
        weapon.box.enabled = true;
        isWeaponDetecting = true;
        WeaponDetection();
    }

    public void WeaponBoxFreeze()
    {
        //weapon.tag = "FreezeWeapon";
        weapon.box.enabled = false;
        isWeaponDetecting = false;
    }

    public void SetGuard(int value)
    {
        if(value == 0)
            isGuard= false;
        else
            isGuard= true;
    }

    #endregion

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
