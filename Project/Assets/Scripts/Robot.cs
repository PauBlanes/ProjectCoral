using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour {
    
    public enum States { walking, working, wander};
    public enum RobotType {Investigation, Repair, Rubbish, Net, Petrol } //cuidao que el nom del robot es el del tag de l'amenaça

    private Transform target;
    public float speed;

    public float workTime;
    private float waitCounter;

    public States state = States.walking;

    public float workPerSecond; //quants destrueixes per segon

    public RobotType rT;

    //Wander
    float wanderAngle = 0;
    private float minAngleChange = -30;
    private float maxAngleChange = 30;
    float circleDistance = 10;
    float circleRadius = 8;
    Vector3 wanderDirection = Vector3.up;
    Vector3 circleCenter;
    Vector3 circlePoint;

    // Use this for initialization
    void Start()
    {        
        waitCounter = workTime;

        //Cojemos el más cercano como target
        target = GetCloserTarget(GameObject.FindGameObjectsWithTag(rT.ToString()));
        
    }
	
	// Update is called once per frame
	void Update () {

        if (waitCounter > 0) //si encara te temps de vida
        {
            //restem temps de vida
            waitCounter -= Time.deltaTime;

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
                    target = GetCloserTarget(GameObject.FindGameObjectsWithTag(rT.ToString()));
                    if (target != null)
                    {
                        speed *= 1.5f;
                        state = States.walking;
                    }
                        
                }
                else
                    StartWander();
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
                    }
                    else
                    {                        
                        float step = speed * Time.deltaTime;                        
                        transform.position = Vector2.MoveTowards(transform.position, target.position, step);
                    }
                }
                else if (state == States.working) //l'ataquem
                {
                    //manternirse enganxat
                    transform.position = target.position; //per si encara estava moventse l'enemic quan l'atrapem que el continui seguint

                    //Quant triga a cargar-se l'enemic
                    float enemyHealth = Attack();
                    if (enemyHealth <= 0)
                    {
                        KillEnemy();
                    }
                }
            }                    
        }
        else //Acció que fer  mors
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
        speed /= 1.5f;
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

    void KillEnemy()
    {
        //Sumar al nivell del sistema
        Camera.main.GetComponent<EcosystemManager>().ecosystemEvolution += 10;

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
    }

    void Die ()
    {
        if (rT == RobotType.Investigation)
        {
            //show info
        }

        //destroy robot
        Destroy(gameObject);
    }
}
