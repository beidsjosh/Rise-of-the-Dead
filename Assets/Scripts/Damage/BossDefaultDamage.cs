using UnityEngine;
using System.Collections;

public class BossDefaultDamage : MonoBehaviour
{

    public int damage = 20;

    public bool isTouching = false;
    public float maxDistance = 1;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    
    void OnCollisionEnter(Collision other)
    {
        //Debug.Log("Triggered");
        // other object is close
        if (Vector3.Distance(other.transform.position, this.transform.position) < maxDistance)
        {
            isTouching = true; // they are touching AND close
            //Debug.Log(isTouching);
            GameObject obj = other.gameObject;
				if (obj.tag == "Player")
				{
                    //Debug.Log("Im hitting the player");
					playerScript tank = (playerScript) obj.GetComponent(typeof(playerScript));
					tank.ApplyDamage(damage);

				}
        }
        else {
            isTouching = false;
            //Debug.Log(isTouching);
        }
    }

}