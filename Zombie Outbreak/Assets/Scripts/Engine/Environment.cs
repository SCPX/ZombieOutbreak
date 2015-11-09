using UnityEngine;
using System.Collections;

public class Environment : MonoBehaviour {
    private Vector3 position = new Vector3(0,0,0);
    public GameObject _Zombie;
    private float timer = 0.0f;
    private const float MAXTIMER = 5.0f;//used to spawn zombies
    public float numExits = 4;

	// Use this for initialization
	void Start () {
        if (_Zombie == null)
        {
            _Zombie = Resources.Load("Prefabs/Zombie") as GameObject;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (timer <= 0f)
        {
            //this is the timed spawn for zombies
            if (PlayerControl.Instance != null)
            {
                float threshold = Random.Range(15.0f, 60.0f);//generates a random threshold
                Vector2 randompoint = Random.insideUnitCircle;// generates a random point

                position = PlayerControl.Instance.transform.position + (Vector3)(randompoint.normalized * threshold);//generates the spawn point of the zombie

                if ((position.x > -150 && position.x < 150) && (position.y > -150 && position.y < 150))//makes sure the spawnpoint is in the bounds
                {
                    if (!Physics2D.OverlapCircle(position, 1.3f))//makes sure there is nothing conflicting with the spawnpoint
                    {
                        GameObject zombie = GameObject.Instantiate(_Zombie, position, PlayerControl.Instance.transform.rotation) as GameObject;//generates the zombie
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;

                }
            }

            timer = MAXTIMER;
        }
        else
        {
            timer -= Time.deltaTime;
        }
	}
}
