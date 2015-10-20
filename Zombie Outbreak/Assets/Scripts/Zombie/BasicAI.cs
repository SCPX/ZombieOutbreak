using UnityEngine;
using System.Collections;

public class BasicAI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    void Move()
    {
        //check for player
        //if no player get random direction 180 degrees
        //move speed in that direction
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "Environment")
        {
            Debug.Log("Bumped into something");
        }

    }
}
