using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;
using UnityEngine.Events;

public class CAICharacter : CCharacter
{
    [HideInInspector] public NavMeshAgent agent;

    public string[] currentBehaviours;
    
    public Transform targetPoint;
    //行为列表
    public Dictionary<string,UnityAction> behaviours=new Dictionary<string,UnityAction>();

    private bool behaviourEnable = true;
    private bool jumpEnable = false;

    protected override void Init()
    {
        if (GetComponent<NavMeshAgent>() != null)
        {
            agent = GetComponent<NavMeshAgent>();
            //agent.destination = transform.position;
            //agent.isStopped = true;
            agent.destination = targetPoint.position;
        }

        behaviours.Add("到达目标点", MoveToTarget);
        behaviours.Add("向敌人移动", MoveToEnemy);
        behaviours.Add("近距离时攻击", AttackAtCloseRange);
        behaviours.Add("近距离时后退", DodgeAtCloseRange);
        behaviours.Add("近距离时观察", ObserveAtCloseRange);
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

    //public async void Stroll()

    //        float rx = Random.Range(transform.position.x - 10, transform.position.x + 10);
    //        float rz = Random.Range(transform.position.z - 10, transform.position.z + 10);
    //        startPoint.position = new Vector3(rx, transform.position.y, rz);
    //        agent.destination = startPoint.position;

    //        await new WaitForSecondsRealtime(10);

    //    //随机设置目标
        
    //}

    /// <summary>
    /// 移动到目标点
    /// </summary>
    public void MoveToTarget()
    {
        if (!fighting)
        {
            agent.destination = targetPoint.position;

        }

    }

    /// <summary>
    /// 向敌人移动
    /// </summary>
    public void MoveToEnemy()
    {
        if (noticed!=null && !isDeath&&Vector3.Distance(noticed.transform.position,transform.position)>5)
        {
            agent.destination = noticed.transform.position;
            if (!fighting)
            {
                fighting = true;
                anim.SetBool("Fighting", true);
            }
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
            if (!agent.isStopped)
                agent.isStopped = true;
            if ((currentAnimatorStateInfo.IsTag("Attack") || currentAnimatorStateInfo.IsName("AttackStatus"))&&currentAnimatorStateInfo.normalizedTime>=0.8f)
            {
                transform.LookAt(noticed.transform);
                anim.SetTrigger("Attack");
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
            if (!agent.isStopped)
                agent.isStopped = true;
            if ((currentAnimatorStateInfo.IsTag("Attack") || currentAnimatorStateInfo.IsName("AttackStatus")) && currentAnimatorStateInfo.normalizedTime >= 0.8f)
            {
                transform.LookAt(noticed.transform);
                anim.SetTrigger("Attack");
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

    protected override void FUpdate()
    {
        anim.SetFloat("Speed", agent.velocity.magnitude / agent.speed);
    }

    protected override void NUpdate()
    {
        for (int i = 0; i < currentBehaviours.Length; i++)
        {
            if(jumpEnable)
            {
                jumpEnable = false;
                break;
            }

            if (behaviourEnable)
            {
                behaviours[currentBehaviours[i]]();
            }
        }

    }

    
}
