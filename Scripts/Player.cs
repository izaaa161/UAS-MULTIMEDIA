// KEMBALI KE VERSI YANG STABIL DAN BENAR
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))] 
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 14f;
    public float fallMultiplier = 2.5f; 
    public float lowJumpMultiplier = 2f; 

    [Header("Ground Check")]
    public LayerMask groundLayer; 

    [Header("Animation Sprites")]
    public Sprite spriteIdle; 
    public Sprite[] spriteLompat; 
    public float frameRate = 0.1f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;
    private bool isGrounded;
    private int frameLompat;
    private float frameTimer;

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>(); 
    }

    void Update() 
    {
        isGrounded = IsPlayerGrounded();
        if (isGrounded && Input.GetButtonDown("Jump")) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        UpdateSpriteAnimation();
    }

    void FixedUpdate() 
    {
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        } 
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void UpdateSpriteAnimation()
    {
        if (isGrounded) {
            spriteRenderer.sprite = spriteIdle;
            frameLompat = 0;
            frameTimer = 0f;
        } else {
            frameTimer += Time.deltaTime;
            if (frameTimer >= frameRate) {
                frameTimer = 0f;
                frameLompat++;
                if (frameLompat >= spriteLompat.Length) {
                    frameLompat = 0;
                }
                spriteRenderer.sprite = spriteLompat[frameLompat];
            }
        }
    }

    private bool IsPlayerGrounded() 
    {
        float extraHeightText = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            playerCollider.bounds.center, 
            playerCollider.bounds.size, 
            0f, 
            Vector2.down, 
            extraHeightText, 
            groundLayer
        );
        return raycastHit.collider != null;
    }
}