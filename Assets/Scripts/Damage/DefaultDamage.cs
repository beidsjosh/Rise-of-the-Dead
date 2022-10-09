using UnityEngine;
using System.Collections;

public class DefaultDamage : MonoBehaviour
{

    //damage the enemy can give out
    public int damage = 10;

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
        // other object is close
        if (Vector3.Distance(other.transform.position, this.transform.position) < maxDistance)
        {
            isTouching = true; // they are touching AND close
            GameObject obj = other.gameObject;
				if (obj.tag == "Player")
				{
                    //apply damage to player
					playerScript player = (playerScript) obj.GetComponent(typeof(playerScript));
					player.ApplyDamage(damage);

				}
        }
        else {
            isTouching = false;
        }
    }

}