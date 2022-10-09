using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour
{
    //Explosion Effect
    //public GameObject Explosion;

    public float speed = 20.0f;
    public float lifeTime = 3.0f;
    public int defaultDamage = 25;
	public int BossDamage = 50;
	public int playerDamage = 50;

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
				if (obj.tag == "RangedEnemy")
				{
					RangedEnemyFSM enemy = (RangedEnemyFSM) obj.GetComponent(typeof(RangedEnemyFSM));
					enemy.ApplyDamage(playerDamage);

				}
				else if (obj.tag == "BossRangedEnemy")
				{
					BossRangedFSM enemy = (BossRangedFSM) obj.GetComponent(typeof(BossRangedFSM));
					enemy.ApplyDamage(playerDamage);

				}
				else if (obj.tag == "DefaultEnemy")
				{
					DefaultEnemyFSM enemy = (DefaultEnemyFSM) obj.GetComponent(typeof(DefaultEnemyFSM));
					enemy.ApplyDamage(playerDamage);

				}
				else if (obj.tag == "BossEnemy")
				{
					BossDefaultFSM enemy = (BossDefaultFSM) obj.GetComponent(typeof(BossDefaultFSM));
					enemy.ApplyDamage(playerDamage);

				}
				else if (obj.tag == "NinjaEnemy")
				{
					NinjaFSM enemy = (NinjaFSM) obj.GetComponent(typeof(NinjaFSM));
					enemy.ApplyDamage(playerDamage);

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