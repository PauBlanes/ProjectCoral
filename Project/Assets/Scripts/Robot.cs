﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Robot : MonoBehaviour {
    
    public enum States { walking, working, wander, investigating};
    public enum RobotType {Investigation, Repair, Rubbish, Net, Petrol } //cuidao que el nom del robot es el del tag de l'amenaça

    private Transform target;
    public float speed;

    public float lifeTime;
    private float lifeCounter;

    public States state = States.walking;

    public float workPerSecond; //quants destrueixes per segon

    public RobotType rT;

    //Pel robot de investigació
    public static List<GameObject> investigableObjects = new List<GameObject>();    
    public static List<GameObject> alreadyInvestigated = new List<GameObject>();

    //Pel robot de reparar blanqueamiento
    public static List<GameObject> bleachedCorals = new List<GameObject>();

    //Wander
    float wanderAngle = 0;
    private float minAngleChange = -30;
    private float maxAngleChange = 30;
    float circleDistance = 10;
    float circleRadius = 8;
    Vector3 wanderDirection = Vector3.up;
    Vector3 circleCenter;
    Vector3 circlePoint;

    //UI
    public Image lifeBar;
    public Image workIcon;

    // Use this for initialization
    void Start()
    {        
        lifeCounter = lifeTime;

        //Cojemos el más cercano como target
        if (rT != RobotType.Investigation && rT != RobotType.Repair)
            target = GetCloserTarget(GameObject.FindGameObjectsWithTag(rT.ToString()));
        else if (rT == RobotType.Investigation) //cojemos un random entre todo lo que podemos investigar
            ChooseInvestigationTarget();
        else //cojemos el coral blanqueado más cercano
        {
            target = GetCloserRepairTarget();
        }

        workIcon.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (lifeCounter > 0 || rT == RobotType.Investigation) //si encara te temps de vida. AL de investigacio li dona igual
        {
            //restem temps de vida
            lifeCounter -= Time.deltaTime;
            if(rT != RobotType.Investigation)
                lifeBar.fillAmount = lifeCounter / lifeTime;

            //Si no tenim objectiu fem wander
            if (target == null)
            {
                if (state == States.wander)
                {
                    //Mirar al target de wander
                    FaceTarget(transform.position + wanderDirection.normalized * 5);

                    //comprovar si ens estem anant dels limits            
                    if (transform.position.x > 15 || transform.position.x < -15)
                        wanderDirection.x = -transform.position.x/15;
                    if (transform.position.y > 8 || transform.position.y < -5)
                        wanderDirection.y = -transform.position.y;

                    //Moure
                    transform.position += wanderDirection.normalized * speed * Time.deltaTime;

                    //Comprovem si tenim objectiu
                    if (rT != RobotType.Repair)
                        target = GetCloserTarget(GameObject.FindGameObjectsWithTag(rT.ToString()));
                    else
                        target = GetCloserRepairTarget();

                    if (target != null)
                    {
                        speed *= 2f;
                        state = States.walking;
                    }
                        
                }
                else
                {
                    if (rT == RobotType.Investigation)
                        ChooseInvestigationTarget();
                    StartWander();
                }
                    
            }            
            else //si tenim objectiu
            {
                if (state == States.walking) //el perseguim
                {
                    //Mirar al target
                    FaceTarget(target.transform.position);

                    //Si hem arrivat passem a treballar, sino ens continuem movent
                    if ((transform.position - target.position).magnitude < 0.1f)
                    {
                        state = States.working;
                        StartCoroutine(WorkingUI());
                    }
                    else
                    {                        
                        float step = speed * Time.deltaTime;                        
                        transform.position = Vector2.MoveTowards(transform.position, target.position, step);
                    }

                    if (rT == RobotType.Investigation && target == null)
                    {
                        ChooseInvestigationTarget();
                    }
                }
                else if (state == States.working) 
                {
                    //manternirse enganxat
                    transform.position = target.position; //per si encara estava moventse l'enemic quan l'atrapem que el continui seguint

                    if (rT != RobotType.Investigation && rT != RobotType.Repair)
                    {
                        //Restar vida a l'amenaça
                        float enemyHealth = Attack();
                        if (enemyHealth <= 0)
                        {
                            KillEnemy();
                        }
                    }
                    else if (rT == RobotType.Repair)
                    {
                        bool curedCoral = target.GetComponent<Bleaching>().Healing();
                        if(curedCoral)
                        {
                            //buscar nou coral
                            target = GetCloserRepairTarget();
                            if (target == null) //si ja no hi ha més pasem a wander
                            {
                                StartWander();
                            }
                            else //si hi ha un altre perseguim aquest
                                state = States.walking;
                        }
                    }
                    else
                    {
                        float timeLeft = Investigate();                        
                        if (timeLeft <= 0) //quan hem acabat d'investigar mostrem info i morim
                        {
                            investigableObjects.Remove(target.gameObject); //treiem l'objecte que ja hem investigat
                            alreadyInvestigated.Add(target.gameObject);
                            target.GetComponent<InvestigableObj>().ShowInfo();
                            Camera.main.GetComponent<EcosystemManager>().UpdateInvestigationUI();
                            Die();
                        }
                    }
                }
            }                    
        }
        else //Acció que fer quan s'acaba el temps de vida
        {
            Die();
        }
               
    }

    Transform GetCloserTarget(GameObject[] objs)
    {
        float maxDist = 0;
        Transform result = null;

        foreach (GameObject o in objs)
        {
            float distance = (o.transform.position - transform.position).magnitude;
            if (distance > maxDist)
                result = o.transform;
        }
        return result;
    }

    Transform GetCloserRepairTarget ()
    {
        float maxDist = 0;
        Transform result = null;

        foreach (GameObject o in bleachedCorals)
        {
            float distance = (o.transform.position - transform.position).magnitude;
            if (distance > maxDist)
                result = o.transform;
        }
        return result;
    }

    IEnumerator SetRandomDirection()
    {
        while(state == States.wander)
        {
            float angleChange = Random.Range(minAngleChange, maxAngleChange);
            wanderAngle += angleChange; 

            circleCenter = transform.position + wanderDirection.normalized * circleDistance;
            
            circlePoint.x = circleCenter.x + circleRadius * Mathf.Cos(wanderAngle*Mathf.Rad2Deg);            
            circlePoint.y = circleCenter.y + circleRadius * Mathf.Sin(wanderAngle*Mathf.Rad2Deg);                      
            
            wanderDirection = (circlePoint - transform.position).normalized;
            

            yield return new WaitForSeconds(1f);
        }
    }

    void StartWander ()
    {
        speed /= 2f;
        state = States.wander;
        StartCoroutine(SetRandomDirection());
    }

    void FaceTarget(Vector3 t)
    {
        Vector3 vectorToTarget = t - transform.position;
        float angle = Mathf.Atan2(-vectorToTarget.y, -vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime);
    }

    float Attack ()
    {

        Color col = target.GetComponent<SpriteRenderer>().color;
        col.a -= Time.deltaTime * workPerSecond;
        target.GetComponent<SpriteRenderer>().color = col;

        return col.a;
    }    

    float Investigate()
    {
        lifeTime -= Time.deltaTime*workPerSecond;
        return lifeTime;
    }

    void KillEnemy()
    {
        //Sumar al nivell del sistema per destruir petroli, reixa o oli
        Camera.main.GetComponent<EcosystemManager>().UpdateSystemHealth(10);

        //destruir enemic
        Destroy(target.gameObject);

        //buscar nou enemic
        target = GetCloserTarget(GameObject.FindGameObjectsWithTag(rT.ToString()));
        if (target == null) //si ja no hi ha més pasem a wander
        {
            StartWander();
        }
        else //si hi ha un altre perseguim aquest
            state = States.walking;

        Camera.main.GetComponent<RobotManager>().PlayRewardSound();
    }

    void Die ()
    {
        //destroy robot
        Destroy(gameObject);
    }
    
    void ChooseInvestigationTarget()
    {
        int fishOrCoral = Random.Range(0, 2);
        fishOrCoral = Mathf.Clamp(fishOrCoral, 0, 1);
        
        if (fishOrCoral == 0)
            target = investigableObjects[Random.Range(0, investigableObjects.Count)].transform;
        else
        {
            GameObject[] fishes = GameObject.FindGameObjectsWithTag("FISH");
            GameObject chosen;
            int saveNum = 0;
            do
            {
                saveNum++;
                chosen = fishes[Random.Range(0, fishes.Length)];
                if (saveNum > 100)
                {
                    target = investigableObjects[Random.Range(0, investigableObjects.Count)].transform;
                    return;
                }                    
            } while (chosen.transform.position.x > 6 && chosen.transform.position.x < -6 && !alreadyInvestigated.Contains(chosen)); //si està al centre de la pantalla i no l'haviem investigat

            target = chosen.transform;
            speed *= 1.5f;
            lifeTime *= 0.5f;            
        }
            
        
    }
    IEnumerator WorkingUI()
    {
        while(state == States.working)
        {
            workIcon.enabled = true;
            yield return new WaitForSeconds(0.2f);
            workIcon.enabled = false;
            yield return new WaitForSeconds(0.2f);
        }

        workIcon.enabled = false;
    }
        
}
