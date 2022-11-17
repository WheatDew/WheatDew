using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class CCharacter : MonoBehaviour
{
    public bool useCharacterForward = false;
    public bool lockToCameraForward = false;
    public float turnSpeed = 10f;



    protected float turnSpeedMultiplier;
    protected float speed = 0f;
    protected float direction = 0f;
    protected bool isSprinting = false;
    [HideInInspector] public Animator anim;
    protected Vector3 targetDirection;

    protected Quaternion freeRotation;
    protected Camera mainCamera;
    protected float velocity;
    public static bool isMoving = true;


    //���������
    public int lmask = 0;
    public CWeapon weapon;
    [HideInInspector] public bool fighting = false;
    public bool speedCompensate = false;
    public bool isAI = false;
    [HideInInspector] public GameObject noticed;
    private Rigidbody body;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public bool isDeath = false;
    [HideInInspector] public bool isAIMove = true;

    [HideInInspector] public CBloodBar bloodBar;
    [HideInInspector] public float health = 1;
    [HideInInspector] public float energy = 1;

    public bool isGuard = false, isParry = false, isCollide;

    public Transform startPoint;

    public Transform weaponStart, weaponEnd;
    public List<Vector3> points = new List<Vector3>();
    public bool isWeaponDetecting = false;
    [HideInInspector] public bool isExecuting = false;
    [HideInInspector] public bool isExecuted = false;
    protected HashSet<CCharacter> executeObjects = new HashSet<CCharacter>();


    [HideInInspector] public float damage = 0.1f, energyDamage = 0.1f;
    [HideInInspector] public float executedDamage = 0.5f;
    [HideInInspector] public float energyRecover = 0.01f;
    // Use this for initialization
    void Start()
    {
        //��ʼ������
        anim = GetComponent<Animator>();
        mainCamera = Camera.main;

        body = GetComponent<Rigidbody>();
        if (GetComponent<NavMeshAgent>() != null)
        {
            agent = GetComponent<NavMeshAgent>();
            //agent.destination = transform.position;
            //agent.isStopped = true;
            agent.destination = startPoint.position;
        }


        if (isAI)
            AIBehaviour();


        Init();
    }

    virtual protected void Init()
    {
        
    }

    virtual protected void FUpdate()
    {

    }
    //AI�첽�ȴ�
    public async void AIBehaviour()
    {
        float guardTime = 0;
        float dodgeTime = 0;
        float strollTime = 0;

        while (!isDeath)
        {
            guardTime += Time.deltaTime;
            dodgeTime += Time.deltaTime;
            strollTime += Time.deltaTime;

            if (isExecuted)
            {
                await new WaitForUpdate();
                continue;
            }

            if (noticed != null)
            {
                if (Vector3.Distance(agent.destination, startPoint.position) < 1)
                {
                    agent.destination = noticed.transform.position;
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
                    Vector2 target_v2 = SMath.GetRandomValueOnCircle(3);
                    Vector3 target_v3 = new Vector3(target_v2.x, 0, target_v2.y);
                    //Debug.Log(target_v3);
                    agent.destination = noticed.transform.position + target_v3;
                }


                if (Vector3.Distance(noticed.transform.position, transform.position) <= 2f)
                {
                    agent.destination = noticed.transform.position;
                    transform.LookAt(agent.destination);
                    if (!noticed.CompareTag("Death"))
                    {
                        //Debug.LogFormat("Ŀ��λ��:{0};����λ��{1};����{2}", noticed.transform.position, transform.position, Vector3.Distance(noticed.transform.position, transform.position));
                        if (Vector3.Distance(noticed.transform.position, transform.position) < 1f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Low"))
                        {
                            float rv = Random.value;
                            if (rv > 0.95f && dodgeTime > Random.Range(1, 3))
                            {
                                dodgeTime = 0;
                                anim.ResetTrigger("Attack");
                                anim.ResetTrigger("Dodge");
                                anim.SetTrigger("Dodge");

                            }
                            else if (rv > 0.8f && guardTime > Random.Range(0.5f, 0.3f))
                            {
                                guardTime = 0;
                                anim.SetBool("Guard", true);
                                await new WaitForSeconds(Random.Range(1f, 5f));
                                anim.SetBool("Guard", false);
                            }
                            else
                            {
                                agent.destination = Vector3.Lerp(noticed.transform.position, transform.position, 0.2f);
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

                        }
                        else
                        {
                            agent.destination = Vector3.Lerp(noticed.transform.position, transform.position, 0.2f);
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

                    }
                    else if (fighting)
                    {
                        fighting = false;
                        anim.SetBool("Fighting", false);
                        agent.destination = startPoint.position;
                        noticed = null;
                    }

                }

                if (isAIMove && noticed != null)
                    AIMove(noticed.transform.position, 1, 0);
            }
            else
            {
                if (fighting)
                {
                    fighting = false;
                    anim.SetBool("Fighting", false);
                }
                if (agent != null)
                    AIMove(agent.destination, 0.2f, 0);
            }

            await new WaitForUpdate();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var currentInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (isExecuted && !isDeath && !currentInfo.IsName("Executed") && !anim.GetCurrentAnimatorStateInfo(0).IsName("ExecutedDeath"))
        {
            Debug.Log("Executed" + gameObject.name);
            anim.ResetTrigger("Attack");
            anim.ResetTrigger("Dodge");
            anim.ResetTrigger("Hit");
            WeaponBoxFreeze();
            if (executedDamage < health)
                anim.SetTrigger("Executed");
            else
                anim.SetTrigger("ExecutedDeath");
            return;
        }

        if (isExecuting && !currentInfo.IsName("Executing"))
        {
            Debug.Log("Executing" + gameObject.name);
            anim.ResetTrigger("Attack");
            anim.ResetTrigger("Dodge");
            anim.ResetTrigger("Hit");
            WeaponBoxFreeze();
            anim.SetTrigger("Executing");
            return;
        }

        if (!isDeath && currentInfo.IsName("Collide"))
        {
            anim.ResetTrigger("Attack");
            anim.ResetTrigger("Dodge");
            anim.ResetTrigger("Hit");
            energy = 0;
            WeaponBoxFreeze();
            anim.SetTrigger("Collide");
            return;
        }

        if (!isDeath && !isGuard)
        {
            energy += 0.05f * Time.fixedDeltaTime;
            if (energy > 1)
                energy = 1;
        }

        FUpdate();


        //����y���ٶ�
        if (body.velocity.y > 0.1f)
        {
            Vector3 temp = body.velocity;
            temp.y = 0.1f;
            body.velocity = temp;
        }

        
    }

    public void SetParry(int value)
    {
        isParry = !(value == 0);
    }


    //�����˺��ж�
    public void ExecutedHitDetection()
    {

        health -= executedDamage;
        if (health <= 0)
        {
            health = 0;
            isDeath = true;
            if (agent != null)
                agent.isStopped = true;
            tag = "Death";
            GetComponent<CapsuleCollider>().isTrigger = true;
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void SetDeath()
    {
        isDeath = false;
    }

    //�˺��ж�
    public async void HitDetection(float damage, CCharacter character)
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);


        if (isParry)
        {

            isParry = false;
            isGuard = false;
            anim.ResetTrigger("Attack");
            anim.ResetTrigger("Dodge");
            anim.ResetTrigger("Hit");
            anim.ResetTrigger("GuardBreak");
            anim.SetTrigger("Parry");

            character.anim.ResetTrigger("Attack");
            character.anim.ResetTrigger("Dodge");
            character.anim.ResetTrigger("Hit");
            character.anim.ResetTrigger("GuardBreak");
            character.anim.SetTrigger("Collide");
            executeObjects.Add(character);

            await new WaitForSeconds(1.2f);
            if (executeObjects.Contains(character))
                executeObjects.Remove(character);

        }
        else if (isGuard)
        {

            anim.SetTrigger("GuardHit");
            health -= damage * 0.2f;
            energy -= energyDamage;

        }
        else if (!stateInfo.IsName("Hit"))
        {
            anim.SetTrigger("Hit");
            health -= damage;
        }


        if (health <= 0)
        {
            health = 0;
            isDeath = true;
            if (agent != null)
                agent.isStopped = true;
            tag = "Death";
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
                anim.SetTrigger("Death");

            GetComponent<CapsuleCollider>().isTrigger = true;
            GetComponent<Rigidbody>().isKinematic = true;
        }
        else if (energy <= 0.4f)
        {
            isGuard = false;
            anim.ResetTrigger("Attack");
            anim.ResetTrigger("Dodge");
            anim.ResetTrigger("Hit");
            anim.SetTrigger("GuardBreak");
            character.executeObjects.Add(this);

            Debug.LogFormat("{0},{1}", character.name, this.name);
            await new WaitForSeconds(1.2f);
            if (character.executeObjects.Contains(this))
                character.executeObjects.Remove(this);
        }
    }

    //���߼���ж�
    public async void WeaponDetection()
    {
        int length = 5;
        HashSet<GameObject> hitObjects = new HashSet<GameObject>();
        Vector3[] last = new Vector3[length];
        Vector3[] current = new Vector3[length];
        RaycastHit result;
        
        while (isWeaponDetecting)
        {
            for (int i = 0; i < length; i++)
            {
                current[i] = Vector3.Lerp(weaponStart.position, weaponEnd.position, (float)i / (length - 1));
            }

            for (int i = 0; i < length; i++)
            {
                if (last[i] != Vector3.zero && Physics.Raycast(last[i], current[i] - last[i], out result, Vector3.Distance(current[i], last[i]), lmask))
                {
                    Debug.DrawLine(last[i], current[i], Color.red, 4);
                    if (result.collider.tag == "Player" && !hitObjects.Contains(result.collider.gameObject) && result.collider.gameObject != gameObject)
                    {
                        result.collider.GetComponent<CCharacter>().HitDetection(damage, this);
                        hitObjects.Add(result.collider.gameObject);
                    }

                }
            }

            for (int i = 0; i < length; i++)
            {
                last[i] = current[i];
            }
            await new WaitForFixedUpdate();
        }

    }



    IEnumerator BackMove()
    {
        //�ȴ�ĳ��Э��ִ����Ϻ���ִ�к�������
        yield return new WaitForSeconds(0.5f);
        agent.destination = transform.position + (noticed.transform.position - transform.position).normalized * 5;
    }

    #region �ƶ�

    public void AIMove(Vector3 target, float maxSpeed, float minSpeed)
    {
        speed = Vector3.Distance(target, transform.position) * 0.1f;
        speed = Mathf.Clamp(speed, 0f, 1f);
        speed = Mathf.SmoothDamp(anim.GetFloat("Speed"), speed, ref velocity, 0.1f);
        if (speed > maxSpeed)
            speed = maxSpeed;
        if (speed < minSpeed)
            speed = 0;

        anim.SetFloat("Speed", speed);
        //transform.LookAt(agent.destination);
    }

    #endregion

    #region ������
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
        //weapon.box.enabled = true;
        isWeaponDetecting = true;
        WeaponDetection();
    }

    public void WeaponBoxFreeze()
    {
        //weapon.tag = "FreezeWeapon";
        //weapon.box.enabled = false;
        isWeaponDetecting = false;
    }

    public void SetGuard(int value)
    {
        if (value == 0)
            isGuard = false;
        else
            isGuard = true;
    }

    public void SetExecuting(int value)
    {
        if (value == 0)
            isExecuting = false;
        else
            isExecuting = true;
    }

    public void SetExecuted(int value)
    {
        if (value == 0)
            isExecuted = false;
        else
            isExecuted = true;
    }

    #endregion



}
