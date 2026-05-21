using UnityEngine;

public class Waepon : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigidbody2D;

    public int rotationSpeed = 200;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (rigidbody2D == null)
            rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       // rigidbody2D.MoveRotation(rigidbody2D.rotation - rotationSpeed * Time.fixedDeltaTime);


    }

}
