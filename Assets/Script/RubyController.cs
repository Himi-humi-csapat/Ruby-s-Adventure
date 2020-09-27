using TMPro;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    public float timeInvincible = 2.0f;
    public int maxHealth = 5;
    public int Health { get; private set; }
    public int maxLövedék = 5;
    public int Lövedék { get; private set; }
    public Transform kiindulásiPont;
    public GameObject projectilePrefab;
    public ParticleSystem sérülés;
    public AudioClip lövésHang;
    public AudioClip sérülésHang;

    bool IsInvincible;
    Rigidbody2D rigidbody2d;
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    AudioSource audioSource;
    float horizontal;
    float vertical;
    float invincibleTimer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        Health = maxHealth;
        Lövedék = maxLövedék;
        audioSource = GetComponent<AudioSource>();
        UIAmmoCount.Instance.SetAmmo(Lövedék, maxLövedék);
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (IsInvincible )
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                IsInvincible = false;
        }

        if (Input.GetButtonDown("Fire3"))
        {
            Launch();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.ShowDialog();
                }
            }
        }

        if (Input.GetButtonDown("Cancel"))
        {
            //PlaySound(lövésHang);
            Application.Quit();
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x += 3.0f
                      * horizontal
                      * Time.deltaTime;
        position.y += 3.0f
                      * vertical
                      * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount <0)
        {
            animator.SetTrigger("Hit");
            if (IsInvincible)
                return;

            IsInvincible = true;
            invincibleTimer = timeInvincible;
            PlaySound(sérülésHang);
            Instantiate(sérülés, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
        Health = Mathf.Clamp(Health + amount, 0, maxHealth);

        if (Health == 0)
            Ujrakezdés();

        UIHealthBar.Inctance.SetValue(Health / (float)maxHealth);
    }

    public void ChangeAmmo(int amount)
    {
        Lövedék = Mathf.Clamp(Lövedék + amount, 0, maxLövedék);
        UIAmmoCount.Instance.SetAmmo(Lövedék, maxLövedék);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    void Launch()
    {
        if (Lövedék <= 0)
            return;

        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        PlaySound(lövésHang);
        Lövedék -= 1;
        UIAmmoCount.Instance.SetAmmo(Lövedék, maxLövedék);
    }

    void Ujrakezdés()
    {
        ChangeHealth(maxHealth);
        transform.position = kiindulásiPont.position;
    }
}
