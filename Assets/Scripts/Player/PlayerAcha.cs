using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerAcha : MonoBehaviour
{
    public PlayerInput playerInput;
    private Vector2 input;
    public Rigidbody2D rigidbody;
    public float fuerzaMovimiento = 1f;
    [Header("Animation")]
    public Animator animator;
    const string movimiento = "isMoving";
    [SerializeField] private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody2D>();

        if(animator == null)
            animator = GetComponent<Animator>();

        if(spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        input = playerInput.actions["Move"].ReadValue<Vector2>();
        animator.SetBool(movimiento, input != Vector2.zero);
        if(input.x > 0.1f)
            spriteRenderer.flipX = false;
        else if(input.x < -0.1f)
            spriteRenderer.flipX = true;
    }

    private void FixedUpdate()
    {
        rigidbody.linearVelocity = new Vector2(input.x * fuerzaMovimiento, input.y * fuerzaMovimiento);
    }
    
}
