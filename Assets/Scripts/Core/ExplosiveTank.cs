using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
public class ExplosiveTank : MonoBehaviour
{
    [Header("Setting Ledakan")]
    [Tooltip("Tag objek yang dapat memicu ledakan (misalnya 'Player').")]
    public string triggerTag = "Player";

    [Tooltip("Waktu jeda (detik) setelah game mulai sebelum bom aktif dan bisa meledak.")]
    public float armingDelay = 0.2f;

    [Tooltip("Berapa lama GameObject tank akan dihancurkan setelah animasi ledakan dimulai.")]
    public float destroyDelay = 0.6f;

    private Animator animator;
    private Collider2D tankCollider;
    private bool hasExploded = false;
    private float timer = 0f;
    private bool isArmed = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        tankCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        // Berikan jeda waktu singkat agar posisi objek stabil dulu sebelum bom aktif
        if (!isArmed)
        {
            timer += Time.deltaTime;
            if (timer >= armingDelay)
            {
                isArmed = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Jika belum aktif (arming) atau sudah meledak, abaikan
        if (!isArmed || hasExploded) return;

        if (collision.gameObject.CompareTag(triggerTag))
        {
            TriggerExplosion();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isArmed || hasExploded) return;

        if (other.gameObject.CompareTag(triggerTag))
        {
            TriggerExplosion();
        }
    }

    private void TriggerExplosion()
    {
        hasExploded = true;

        if (tankCollider != null)
        {
            tankCollider.enabled = false;
        }

        if (animator != null)
        {
            animator.SetTrigger("Explode");
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }

        Destroy(gameObject, destroyDelay);
    }
}