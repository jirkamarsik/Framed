using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEater : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.collider.gameObject;

        if (other.CompareTag("PaintBall"))
        {
            Destroy(other);
        }
    }
}
