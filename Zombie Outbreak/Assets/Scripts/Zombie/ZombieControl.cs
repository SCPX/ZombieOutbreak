using UnityEngine;
using System.Collections;

public class ZombieControl : MonoBehaviour
{
    private float health = 1;
    public float damage = 1;
    public float resistance = 0;
    public float speed = 2;
    private Vector3 moveDirection = Vector3.zero; 
    bool stun = false; //used in knockback
    private float timer = 0; // used in AI to wander around
    private float rot_z = 0; //used to rotate sprite
    private const float MAXTIMER = 2; //used in AI to wander around
    private const float THRESHOLD = 20;// used in AI to hunt player

    public float Health
    {
        //acts as a variable with error checks built in
        get { return health; }
        set
        {
            health = Mathf.Clamp(value, 0f, 1.0f);
            if (health <= 0f)
            {
                // Kill zombie
                KillZombie();
            }
        }
    }

    private Rigidbody2D _rigidbody;

    void Start()
    {
        //initiates rigid body and sets stun to false
        _rigidbody = GetComponent<Rigidbody2D>() as Rigidbody2D;
        if (_rigidbody == null)
        {
            _rigidbody = gameObject.AddComponent<Rigidbody2D>() as Rigidbody2D;
        }//*/
        stun = false;
    }

    void Update()
    {
        if (!stun && timer <= 0f)
        {
            moveDirection = Vector3.zero;
            //this is the AI  it wanders until the player is found
            if (PlayerControl.Instance != null)
            {
                if (Vector3.Distance(PlayerControl.Instance.transform.position, this.transform.position) < THRESHOLD)
                {
                    moveDirection = (PlayerControl.Instance.transform.position - this.transform.position).normalized;
                    rot_z = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;//this rotates the sprite to face its movement direction
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
                }
                else
                {
                    moveDirection = Random.insideUnitCircle.normalized;
                    rot_z = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;//this rotates the sprite to face its movement direction
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
                }
            }
            
            timer = MAXTIMER;
        }
        else
        {
            timer -= Time.deltaTime;
        }
        
        //stun = false;
    }

    void FixedUpdate()
    {

        // Fixed updates to use Physics
       
        UpdatePosition();
    }

    void UpdatePosition()
    {
		if(stun){ 
			stun = false;
			return;
		}
        //this is movement if it has a direction it moves in it
        //otherwise it stays still
        if (moveDirection != Vector3.zero)
        {
            Vector3 newLocation = transform.position + (moveDirection * speed);
            _rigidbody.MovePosition(newLocation);
            
        }
        else
        {
            _rigidbody.MovePosition(transform.position);
        }
    }

    void KillZombie()
    {
        Destroy(this.gameObject);//kills the game object
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;//taking damage

    }
    public void ReceiveKnockback(Vector3 knockback)
    {
        float diff =  knockback.magnitude - resistance;//calculates knockback and sets the movement direction to match
        if (diff > 0f)
        {
            //moveDirection = knockback.normalized * diff;
			Vector2 force = new Vector2(knockback.x, knockback.y);
			force = force.normalized * diff; // adjust magnitude to account for resistance.
			Vector3 oldV = _rigidbody.velocity;
			_rigidbody.AddForce(force, ForceMode2D.Impulse);
			stun = true;
            timer = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == Constants.Strings.PLAYER_TAG)
        {
            col.gameObject.SendMessage("TakeDamage", damage);//if it hits a player send a message for them to take damage
        }else if(col.gameObject.tag == Constants.Strings.ENVIRONMENT_TAG)
        {
            //Debug.Log("Bumped into something");//when it bumps into the environment change direction
            timer = 0;
        }
    }
}