using UnityEngine;
using System.Collections;

public class ZombieControl : MonoBehaviour
{
    private float health = 1;
    public float damage = 1;
    public float resistance = 0;
    public float speed = 2;
    private Vector3 moveDirection = Vector3.zero;
    bool stun = false;
    private float timer = 0;
    private const float MAXTIMER = 2;
    private const float THRESHOLD = 15;

    public float Health
    {
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
            //insert ai here
            if (PlayerControl.Instance != null)
            {
                if (Vector3.Distance(PlayerControl.Instance.transform.position, this.transform.position) < THRESHOLD)
                {
                    moveDirection = (PlayerControl.Instance.transform.position - this.transform.position).normalized;
                }
                else
                {
                    moveDirection = Random.insideUnitCircle.normalized;
                }
            }
            //if(no player)
            
            timer = MAXTIMER;
        }
        else
        {
            timer -= Time.deltaTime;
        }
        
        stun = false;
    }

    void FixedUpdate()
    {

        // Fixed updates to use Physics
       
        UpdatePosition();
    }

    void UpdatePosition()
    {

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
        Destroy(this.gameObject);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;

    }
    public void ReceiveKnockback(Vector3 knockback)
    {
        float diff =  knockback.magnitude - resistance;
        Debug.Log(diff);
        if (diff > 0f)
        {
            moveDirection = knockback.normalized * diff;
            stun = true;
            timer = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == Constants.Strings.PLAYER_TAG)
        {
            col.gameObject.SendMessage("TakeDamage", damage);
        }else if(col.gameObject.tag == Constants.Strings.ENVIRONMENT_TAG)
        {
            Debug.Log("Bumped into something");
        }
    }
}