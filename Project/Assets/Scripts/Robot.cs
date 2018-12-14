using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour {
    
    public enum States { walking, working, wander, investigating};
    public enum RobotType {Investigation, Repair, Rubbish, Net, Petrol } //cuidao que el nom del robot es el del tag de l'amenaça

    private Transform target;
    public float speed;

    public float workTime;
    private float waitCounter;

    public States state = States.walking;

    public float workPerSecond; //quants destrueixes per segon

    public RobotType rT;

    //Pel robot de investigació
    public static List<GameObject> investigableObjects = new List<GameObject>();
    private static List<GameObject> investigatedFish = new List<GameObject>();

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

    // Use this for initialization
    void Start()
    {        
        waitCounter = workTime;

        //Cojemos el más cercano como target
        if (rT != RobotType.Investigation && rT != RobotType.Repair)
            target = GetCloserTarget(GameObject.FindGameObjectsWithTag(rT.ToString()));
        else if (rT == RobotType.Investigation) //cojemos un random entre todo lo que podemos investigar
            ChooseInvestigationTarget();
        else //cojemos el coral blanqueado más cercano
        {
            target = GetCloserRepairTarget();
        }           

    }
	
	// Update is called once per frame
	void Update () {

        if (waitCounter > 0 || rT == RobotType.Investigation) //si encara te temps de vida. AL de investigacio li dona igual
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
                else if (state == States.working) 
                {                    
                    if (rT != RobotType.Investigation && rT != RobotType.Repair)
                    {
                        //manternirse enganxat
                        transform.position = target.position; //per si encara estava moventse l'enemic quan l'atrapem que el continui seguint

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
                            target.GetComponent<InvestigableObj>().ShowInfo();
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

    float Investigate()
    {
        workTime -= Time.deltaTime;
        return workTime;
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
        //destroy robot
        Destroy(gameObject);
    }
    
    void ChooseInvestigationTarget()
    {
        int fishOrCoral = Random.Range(0, 1);
        if (fishOrCoral == 0)
            target = investigableObjects[Random.Range(0, investigableObjects.Count)].transform;
        else
        {
            GameObject[] fishes = GameObject.FindGameObjectsWithTag("FISH");
            GameObject chosen;
            do
            {
                chosen = fishes[Random.Range(0, fishes.Length)];
            } while (chosen.transform.position.x > 8 && chosen.transform.position.x < -8 && !investigatedFish.Contains(chosen));
            target = chosen.transform;
            speed *= 2;
            workTime *= 0.5f;
            investigatedFish.Add(chosen);
        }
            
        
    }
        
}
