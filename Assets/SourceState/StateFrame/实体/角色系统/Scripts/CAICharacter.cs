using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;

public class CAICharacter : CCharacter
{
    [HideInInspector] public NavMeshAgent agent;

    public string behaviour= "stroll";

    private Vector3 targetPosition;

    protected override void Init()
    {
        if (GetComponent<NavMeshAgent>() != null)
        {
            agent = GetComponent<NavMeshAgent>();
            //agent.destination = transform.position;
            //agent.isStopped = true;
            agent.destination = startPoint.position;
        }


        AIBehaviour();
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

            Stroll();
            await new WaitUntil(()=>behaviour!="stroll");

            //察觉到敌人时触发
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

    public async void Stroll()
    {
        while (behaviour == "stroll")
        {
            float rx = Random.Range(transform.position.x - 10, transform.position.x + 10);
            float rz = Random.Range(transform.position.z - 10, transform.position.z + 10);
            startPoint.position = new Vector3(rx, transform.position.y, rz);
            agent.destination = startPoint.position;

            await new WaitForSecondsRealtime(10);
        }
        //随机设置目标
        
    }

    protected override void FUpdate()
    {
        if (isDeath&&!agent.isStopped)
            agent.isStopped = true;


        float targetDistance = Vector3.Distance(transform.position, startPoint.position);

        if (targetDistance <= 1f)
        {
            anim.SetFloat("Speed", 0);
            agent.isStopped = true;
            body.velocity = Vector3.zero;
        }
        else if (targetDistance <= 4f)
        {
            anim.SetFloat("Speed", targetDistance / 8f + 0.2f);
            agent.speed = 0.1f;
        }
        else
        {
            anim.SetFloat("Speed", 1);
            agent.speed = 0.1f;
        }
    }

    protected override void NUpdate()
    {
        

    }
}
