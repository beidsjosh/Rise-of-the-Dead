using UnityEngine;
using System.Collections;

public class BossBullet : MonoBehaviour
{
    //Explosion Effect
    //public GameObject Explosion;

    public float speed = 20.0f;
    public float lifeTime = 3.0f;
    public int damage = 50;

	private Vector3 newPos;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
		// future position if bullet doesn't hit any colliders
		newPos = transform.position + transform.forward * speed * Time.deltaTime;

		// see if bullet hits a collider
		RaycastHit hit;
		if (Physics.Linecast(transform.position, newPos, out hit))
		{
			if (hit.collider)
			{
				//destroy bullet
				transform.position = hit.point;
				Destroy(gameObject);

				// apply damage to object
				GameObject obj = hit.collider.gameObject;
				if (obj.tag == "Player")
				{
					playerScript tank = (playerScript) obj.GetComponent(typeof(playerScript));
					tank.ApplyDamage(damage);

				}
			}
		}
		else
		{
			// didn't hit - move to newPos
			transform.position = newPos;
		}     
    }

}