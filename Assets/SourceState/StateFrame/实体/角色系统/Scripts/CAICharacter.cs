using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;
using UnityEngine.Events;

public class CAICharacter : CCharacter
{
    [HideInInspector] public NavMeshAgent agent;

    public List<string> currentBehaviours;
    
    public Transform targetPoint;
    //行为列表
    public Dictionary<string,Behaviour> behaviours=new Dictionary<string,Behaviour>();


    private bool behaviourEnable = true;
    private bool jumpEnable = false;
    private bool strollEnable=false;
    private float maxSpeed = 1;
    public float linkSpeed = 0.4f;
    public float stoppingDistence = 0.5f;

    public float stopTime = 0;
    public float attackProbability = 0.5f, attackTime = 0.5f;



    protected override void Init()
    {
        if (GetComponent<NavMeshAgent>() != null)
        {
            agent = GetComponent<NavMeshAgent>();
            //agent.destination = transform.position;
            //agent.isStopped = true;
            agent.destination = targetPoint.position;
        }

        AddBehaviour("移动到目标点", MoveToTarget);
        AddBehaviour("察觉敌人", NoticeEnemy);
        AddBehaviour("向敌人移动", MoveToEnemy);
        AddBehaviour("近距离时攻击", AttackAtCloseRange);
        AddBehaviour("近距离时后退", DodgeAtCloseRange);
        AddBehaviour("近距离时观察", ObserveAtCloseRange);
        AddBehaviour("闲逛", Stroll);
        AddBehaviour("丢失敌人", MissEnemy);
    }

    //添加行为到列表
    public void AddBehaviour(string name,UnityAction behaviour)
    {
        behaviours.Add(name,new Behaviour(behaviour));
    }

    public void AddBehaviour(string name,float probability,UnityAction behaviour)
    {
        behaviours.Add(name,new Behaviour(probability,behaviour));
    }

    public async void AIBehaviour()
    {
        float guardTime = 0;
        float dodgeTime = 0;
        float strollTime = 0;

        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);

        while (!isDeath)
        {
            animInfo = anim.GetCurrentAnimatorStateInfo(0);

            guardTime += Time.deltaTime;
            dodgeTime += Time.deltaTime;
            strollTime += Time.deltaTime;

            if (isExecuted)
            {
                await new WaitForUpdate();
                continue;
            }

            //建筑
            if (isbuilding && !animInfo.IsName("建筑中"))
            {
                anim.SetBool("Building", true);
                agent.destination = transform.position;
            }

            if (endbuilding && animInfo.IsName("建筑中"))
            {
                endbuilding = false;
                isbuilding = false;
                anim.SetBool("Building", false);
                
            }

            //察觉到敌人时触发
            if (noticed != null)
            {
                if (Vector3.Distance(agent.destination, targetPoint.position) < 1)
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

                if (Vector3.Distance(noticed.transform.position, transform.position) <= 2f)
                {
                    agent.destination = noticed.transform.position;
                    transform.LookAt(agent.destination);
                    if (!noticed.CompareTag("Death"))
                    {
                        //Debug.LogFormat("目标位置:{0};自身位置{1};距离{2}", noticed.transform.position, transform.position, Vector3.Distance(noticed.transform.position, transform.position));
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
                        agent.destination = targetPoint.position;
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



    /// <summary>
    /// 闲逛
    /// </summary>
    public void Stroll()
    {

        if (!strollEnable || Vector3.Distance(targetPoint.position, transform.position) < 0.7f)
        {
            Vector3 target = CharacterGroup.s.GetStrollPoint();
            if (target != Vector3.zero)
            {
                targetPoint.position = target;
                agent.destination = targetPoint.position;
                if (!strollEnable)
                {
                    maxSpeed = 0.2f;
                    strollEnable = true;
                }

                
            }
        }

    }

    /// <summary>
    /// 移动到目标点
    /// </summary>
    public void MoveToTarget()
    {
        if (!fighting)
        {
            agent.destination = targetPoint.position;

        }
        maxSpeed = 0.2f;

    }
    /// <summary>
    /// 察觉敌人
    /// </summary>
    public void NoticeEnemy()
    {
        if (!fighting && noticed != null)
        {

            fighting = true;
            anim.SetBool("Fighting", true);
        }
    }

    /// <summary>
    /// 丢失敌人
    /// </summary>
    public void MissEnemy()
    {
        if (fighting && (noticed == null||noticed.tag=="Death"))
        {
            fighting = false;
            anim.SetBool("Fighting", false);
            agent.destination = targetPoint.position;
        }
    }

    /// <summary>
    /// 向敌人移动
    /// </summary>
    public void MoveToEnemy()
    {
        if (noticed!=null && fighting)
        {
            agent.destination = noticed.transform.position;
            
            if(agent.isStopped)
            {
                agent.isStopped= false;
            }

        }
    }
    /// <summary>
    /// 近距离时攻击
    /// </summary>
    public void AttackAtCloseRange()
    {

        if (noticed != null&& Vector3.Distance(noticed.transform.position, transform.position) <= 2)
        {
            if (currentAnimatorStateInfo.IsTag("Low")    
                &&currentAnimatorStateInfo.normalizedTime>=0.8f  // 判断动画进度
                &&Random.value<=0.5f    //判断概率
                )
            {
                transform.LookAt(noticed.transform);
                anim.SetTrigger("Attack");
                if (!agent.isStopped)
                    agent.isStopped = true;
                jumpEnable = true;
            }
        }
    }

    /// <summary>
    /// 近距离时后退
    /// </summary>
    public void DodgeAtCloseRange()
    {

        if (noticed != null && Vector3.Distance(noticed.transform.position, transform.position) <= 2)
        {
            if (currentAnimatorStateInfo.IsTag("Low")
                && currentAnimatorStateInfo.normalizedTime >= 0.8f  // 判断动画进度
                && Random.value <= 1f    //判断概率
                )
            {
                transform.LookAt(noticed.transform);
                anim.SetTrigger("Dodge");
                if (!agent.isStopped)
                    agent.isStopped = true;
                jumpEnable = true;
            }

        }
    }

    /// <summary>
    /// 近距离时观察
    /// </summary>
    public void ObserveAtCloseRange()
    {
        if (noticed != null && Vector3.Distance(noticed.transform.position, transform.position) <= 5
            &&(Vector3.Distance(agent.destination,transform.position)<0.1f))
        {
            Vector2 target_v2 = SMath.GetRandomValueOnCircle(4);
            Vector3 target_v3 = new Vector3(target_v2.x, 0, target_v2.y);
            agent.destination = noticed.transform.position + target_v3;

        }
    }


    public void DodgeAction()
    {
        
    }

    /// <summary>
    /// 设定速度
    /// </summary>
    public void SetSpeed()
    {

        if (agent.path.corners.Length > 1)
        {
            if (agent.isOnOffMeshLink)
            {
                if (agent.speed == 0)
                    agent.speed = linkSpeed;
            }
            else if (agent.speed == linkSpeed)
            {
                agent.speed = 0;
            }

            //绘制路径
            for(int i = 1; i < agent.path.corners.Length; i++)
            {
                if (i == 1)
                {
                    Debug.DrawLine(agent.path.corners[1], agent.path.corners[0], Color.red, 0.02f);
                }
                else
                {
                    Debug.DrawLine(agent.path.corners[i], agent.path.corners[i-1], Color.blue, 0.02f);
                }
            }



            float distance = Vector3.Distance(agent.destination, transform.position);
            transform.LookAt(agent.path.corners[1]);

            if (distance > 3+stoppingDistence)
            {
                if (maxSpeed < 1)
                    anim.SetFloat("Speed", maxSpeed);
                else
                    anim.SetFloat("Speed", 1);
            }
            else if(distance>stoppingDistence)
            {
                if (maxSpeed < distance-stoppingDistence / 3)
                    anim.SetFloat("Speed", maxSpeed);
                else
                    anim.SetFloat("Speed", distance-stoppingDistence / 3);
            }
            else
            {
                anim.SetFloat("Speed", 0);
            }
        }

    }

    protected override void FUpdate()
    {
        SetSpeed();

    }

    protected override void NUpdate()
    {
        for (int i = 0; i < currentBehaviours.Count; i++)
        {
            if(jumpEnable)
            {
                jumpEnable = false;
                break;
            }
            //Debug.Log(currentBehaviours[i]+" "+behaviours[currentBehaviours[i]].probability.ToString());
            if (behaviourEnable && Random.value <= behaviours[currentBehaviours[i]].probability)
            {
                behaviours[currentBehaviours[i]].behaviour();
            }
        }

    }

    
}
