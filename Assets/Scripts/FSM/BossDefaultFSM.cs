using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDefaultFSM : MonoBehaviour
{
    public GameObject[] waypointList;
    protected Vector3 destPos; // Next destination position of the NPC
    private UnityEngine.AI.NavMeshAgent nav;
    public enum FSMState
    {
        None,
        Patrol,
        Chase,
        Attack,
        Dead,
    }

    // Current state that the NPC is reaching
	public FSMState curState;

	protected Transform playerTransform;// Player Transform

    public float moveSpeed = 6.0f; // Speed of the enemy
    public float moveMultiply = 9.0f; //faster moveSpeed for attack
    public float rotSpeed = 20.0f; // Enemy Rotation Speed

	// Attack shooting rate
	public float shootRate = 1.5f;
	protected float elapsedTime;

    // Whether the NPC is destroyed or not
    protected bool bDead;
    public int health = 200;

	// Ranges for chase and attack
	public float chaseRange = 20.0f;
	public float attackRange = 5.0f;
	public float attackRangeStop = 0.0f;

    //damage the enemy can give out
    public int damage = 50;

    private Vector3 newPos;
    public float speed = 20.0f;


    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        curState = FSMState.Patrol;

        bDead = false;
        elapsedTime = 0.0f;

         // Get the list of patrol points
        waypointList = GameObject.FindGameObjectsWithTag("BossDefaultPatrolPoint");
        FindNextPoint();  // Set a random destination point first

        // Get the target enemy(Player)
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        if(!playerTransform)
            print("Player doesn't exist.. Please add one with Tag named 'Player'");
    }

    // Update is called once per frame
    void Update()
    {
        switch (curState) {
            case FSMState.Patrol: UpdatePatrolState(); break;
            case FSMState.Chase: UpdateChaseState(); break;
            case FSMState.Attack: UpdateAttackState(); break;
            case FSMState.Dead: UpdateDeadState(); break;
        }

        // Update the time
        elapsedTime += Time.deltaTime;

        // Go to dead state if no health left
        if (health <= 0)
            curState = FSMState.Dead;
    }

    protected void UpdateChaseState()
    {
        destPos = playerTransform.position;

        // Rotate to the target point
        Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        GetComponent<Rigidbody>().MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed));

        // Go Forward
        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.forward * Time.deltaTime * moveSpeed);

        //Go to Patrol State if further than 35 units of player
        if (Vector3.Distance(transform.position, playerTransform.position) > chaseRange)
        {
            curState = FSMState.Patrol;
        }

        //Go to Attack State if within than 20 units of player
        if (Vector3.Distance(transform.position, playerTransform.position) < attackRange)
        {
            curState = FSMState.Attack;
        }
    }

    protected void UpdateAttackState() 
    {
        if (elapsedTime >= shootRate)
		{
			//Reset the time
			elapsedTime = 0.0f;
		}

        // Update the time
		elapsedTime += Time.deltaTime;

        // Rotate to the target point
        Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        GetComponent<Rigidbody>().MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed));

        // Go Forward if more than 10 units
        if (Vector3.Distance(transform.position, playerTransform.position) > 0.0f)
        {
            GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.forward * Time.deltaTime * moveMultiply);
        }

        //Go to Chase State if further than 20 units of player
        if (Vector3.Distance(transform.position, playerTransform.position) > attackRange)
        {
            curState = FSMState.Chase;
        }
    }

    /*
     * Patrol state
     */
    protected void UpdatePatrolState()
    {
        // Find another random patrol point if the current point is reached
        if (Vector3.Distance(transform.position, destPos) <= 5.0f)
        {
            FindNextPoint();
        }

        // Rotate to the target point
        Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        GetComponent<Rigidbody>().MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed));

        // Go Forward
        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.forward * Time.deltaTime * moveSpeed/2);

        // Go to chase state if within 35 units of player
        if (Vector3.Distance(transform.position, playerTransform.position) <= chaseRange)
        {
            curState = FSMState.Chase;
        } 
    }

    /*
     * Dead state
     */
    protected void UpdateDeadState()
    {
        // Destorys the enemy and updates score
        if (!bDead)
        {
            bDead = true;
            Destroy(gameObject, 0.5f);
            playerTransform.gameObject.SendMessage("UpdateScore", (int) 200 );
        }
    }

    // Find the next semi-random patrol point
    protected void FindNextPoint()
    {
        int rndIndex = Random.Range(0, waypointList.Length);
        destPos = waypointList[rndIndex].transform.position;
    }


    

    public void ApplyDamage(int dmg)
    {
        health -= dmg;
    }

    void OnDrawGizmos() {
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f);
	    Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = new Color(1.0f, 1.0f, 0.0f);
	    Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = new Color(0.0f, 1.0f, 1.0f);
	    Gizmos.DrawWireSphere(transform.position, attackRangeStop);
    }

}
