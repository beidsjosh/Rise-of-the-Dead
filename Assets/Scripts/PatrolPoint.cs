using UnityEngine;
using System.Collections;

public class PatrolPoint : MonoBehaviour {

	void OnDrawGizmos () {
		Gizmos.color = new Color(1.0f, 0.0f, 0.0f);
		Gizmos.DrawSphere(transform.position, 1.0f);
	}
}
