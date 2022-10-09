using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFSM : MonoBehaviour
{
    public GameObject[] waypointList;
    protected Vector3 destPos; // Next destination position of the NPC Tank
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

    public float moveSpeed = 12.0f; // Speed of the enemy
    public float rotSpeed = 20.0f; // Enemy Rotation Speed
    
    public int curPoint = 0;

     // Bullet
	public GameObject bullet;
	public GameObject bulletSpawnPoint;
	// Bullet shooting rate
	public float shootRate = 3.0f;
	protected float elapsedTime;

    // Whether the NPC is destroyed or not
    protected bool bDead;
    public int health = 100;

	// Ranges for chase and attack
	public float chaseRange = 20.0f;
	public float attackRange = 10.0f;
	public float attackRangeStop = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        curState = FSMState.Patrol;

        bDead = false;
        elapsedTime = 0.0f;

         // Get the list of patrol points
        waypointList = GameObject.FindGameObjectsWithTag("PatrolPoint");
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
        Debug.Log(curState);
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
        if (Vector3.Distance(transform.position, playerTransform.position) > 35.0f)
        {
            //Debug.Log("player is far away");
            curState = FSMState.Patrol;
        }

        //Go to Attack State if within than 20 units of player
        if (Vector3.Distance(transform.position, playerTransform.position) < 20.0f)
        {
            //Debug.Log("player is far away");
            curState = FSMState.Attack;
        }
    }

    protected void UpdateAttackState() 
    {
        if (elapsedTime >= shootRate)
		{
            Debug.Log("Update Attack State");
			//Reset the time
			elapsedTime = 0.0f;

			//Also Instantiate over the PhotonNetwork
			if ((bulletSpawnPoint) & (bullet))
                ///Attack code here
				Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
                // AddForce(transform.forward * 32f, ForceMode.Impulse);
                // AddForce(transform.up * 8f, ForceMode.Impulse);
                ///End of attack code
		}
        // Update the time
		elapsedTime += Time.deltaTime;

        // Rotate to the target point
        Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        GetComponent<Rigidbody>().MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed));

        // Go Forward if more than 10 units
        if (Vector3.Distance(transform.position, playerTransform.position) > 10.0f)
        {
            GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.forward * Time.deltaTime * moveSpeed/2);
        }

        //Go to Chase State if further than 20 units of player
        if (Vector3.Distance(transform.position, playerTransform.position) > 20.0f)
        {
            //Debug.Log("player is far away");
            curState = FSMState.Chase;
        }
    }

    /*
     * Patrol state
     */ 
    protected void UpdatePatrolState()
    {
        Debug.Log("Update patrol State");
        Debug.Log(Vector3.Distance(transform.position, destPos));
        // Find next patrol point if the current point is reached
        if (Vector3.Distance(transform.position, destPos) <= 0.5f)
        {
            FindNextPoint();
        }

        // Rotate to the target point
        // Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        // GetComponent<Rigidbody>().MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed));

        // Go Forward
        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.forward * Time.deltaTime * moveSpeed);

        // Go to chase state if within 35 units of player
        if (Vector3.Distance(transform.position, playerTransform.position) <= 35.0f)
        {
            //Debug.Log("player is close");
            curState = FSMState.Chase;
        } 
    }

    /*
     * Dead state
     */
    protected void UpdateDeadState()
    {
        Debug.Log("Update Dead State");
        // Show the dead animation with some physics effects
        if (!bDead)
        {
            bDead = true;
            Destroy(gameObject, 1.5f);
        }
    }

    // Find the next patrol point
    protected void FindNextPoint()
    {
        Debug.Log("Find next point");
        Debug.Log(curPoint);
         if (curPoint == waypointList.Length)
        {
            curPoint = 0;
        }
        destPos = waypointList[curPoint].transform.position;
        Debug.Log(destPos);
        curPoint += 1;
    }


    

    void ApplyDamage(int dmg)
    {
        health -= dmg;
        Debug.Log("hit enemy");
    }

    void OnDrawGizmos() {
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f);
	    Gizmos.DrawWireSphere(transform.position, 35.0f);

        Gizmos.color = new Color(1.0f, 1.0f, 0.0f);
	    Gizmos.DrawWireSphere(transform.position, 20.0f);

        Gizmos.color = new Color(0.0f, 1.0f, 1.0f);
	    Gizmos.DrawWireSphere(transform.position, 10.0f);
    }

}
