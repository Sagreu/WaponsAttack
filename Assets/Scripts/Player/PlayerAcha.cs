using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerAcha : MonoBehaviour
{
    public PlayerInput playerInput;
    private Vector2 input;
    public Rigidbody2D rigidbody;
    public float fuerzaMovimiento = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        input = playerInput.actions["Move"].ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rigidbody.linearVelocity = new Vector2(input.x * fuerzaMovimiento, input.y * fuerzaMovimiento);
    }
    
}
