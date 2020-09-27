using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float Speed = 3.0f;
    public float ChangeTime = 3.0f;
    public bool Vertical;
    public ParticleSystem smokeEffect;
    public ParticleSystem Javítás;
    public AudioClip találatHang;
    public AudioClip javításHang;
    private Rigidbody2D rigidbody2D;
    Animator animator;
    AudioSource audioSource;
    float timer;
    int direction = 1;
    bool broken = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = ChangeTime;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!broken)
        {
            return;
        }

        timer -= Time.deltaTime;
        if(timer <0)
        {
            direction = -direction;
            timer = ChangeTime;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }

        Vector2 position = rigidbody2D.position;
        if (Vertical )
        {
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
            position.y = position.y + Time.deltaTime * Speed * direction;
        }
        else
        {
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
            position.x = position.x + Time.deltaTime * Speed * direction;
        }

        rigidbody2D.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RubyController player = collision.gameObject.GetComponent<RubyController>();

        if(player !=null )
        {
            player.ChangeHealth(-1);
        }
    }

    public void Fix()
    {
        broken = false;
        rigidbody2D.simulated = false;
        animator.SetTrigger("Fixed");
        Instantiate(Javítás, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        smokeEffect.Stop();
        audioSource.Stop();
        audioSource.PlayOneShot(találatHang);
        audioSource.PlayOneShot(javításHang);
    }
}
