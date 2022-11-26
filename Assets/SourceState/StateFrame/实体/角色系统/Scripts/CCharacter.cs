using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public enum Group { Player, Enemy };
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


    //额外的增量
    public int lmask = 0;
    public CWeapon weapon;
    [HideInInspector] public bool fighting = false;
    public bool speedCompensate = false;
    [HideInInspector] public GameObject noticed;
    private Rigidbody body;

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

    public Group group;

    #region 建筑

    [System.NonSerialized] public bool buildingPrepare,isbuilding;
    #endregion

    // Use this for initialization
    void Start()
    {
        if (group == Group.Player)
            lmask = LayerMask.GetMask("Enemy");
        else if (group == Group.Enemy)
            lmask = LayerMask.GetMask("Player");

        //初始化数据

        anim = GetComponent<Animator>();
        mainCamera = Camera.main;

        body = GetComponent<Rigidbody>();

        //初始化数据
        DBuilding.s.AddCharacterData(this);
        DBuilding.s.GetCharacterData(gameObject.GetInstanceID()).buildingPrepare = true;

        Init();


    }

    virtual protected void Init()
    {
        
    }

    virtual protected void FUpdate()
    {

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


        //限制y轴速度
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


    //处刑伤害判定
    public virtual void ExecutedHitDetection()
    {

        health -= executedDamage;
        if (health <= 0)
        {
            health = 0;
            isDeath = true;
            tag = "Death";
            GetComponent<CapsuleCollider>().isTrigger = true;
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void SetDeath()
    {
        isDeath = false;
    }

    //伤害判定
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

    //射线检测判定
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


    #region 移动

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
