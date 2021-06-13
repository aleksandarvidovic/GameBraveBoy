using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] bool dino;
    [SerializeField] float dirX;
    [SerializeField] public float killX;
    [SerializeField] public float killY;
    
    Rigidbody2D rb;
    bool facingRight = true;
    Vector3 localScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
    }

    void Update()
    {
        if (transform.position.y < -15) 
            Die();
    }

    void Die()
    {
        if(dino)
            rb.transform.position = new Vector3(100.08f, 39.4f);
        else
            Destroy(gameObject);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX, rb.velocity.y);
    }

    void LateUpdate()
    {
        if (dirX > 0)
            facingRight = true;
        else if (dirX < 0)
            facingRight = false;

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Character character = other.gameObject.GetComponent<Character>();

        if (character != null)
        {
            if (other.contacts[0].normal.y < -0.5)
            {
                SoundManager.PlaySound("enemy");
                Die();
            }
        }

        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        Crates crates = other.gameObject.GetComponent<Crates>();
        WoodSpike woodSpike = other.gameObject.GetComponent<WoodSpike>();
        MetalSpike metalSpike = other.gameObject.GetComponent<MetalSpike>();
        
        if (enemy != null || crates != null || woodSpike != null || metalSpike != null)
            dirX *= -1;
    }
}
