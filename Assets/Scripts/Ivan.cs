using UnityEngine;
using UnityEngine.InputSystem;

public class Ivan : MonoBehaviour
{
    public float moveSpeed = 0f;
    
    [Space(5)]
    public Rigidbody2D rb;
    public InputActionReference moveAction;
    public InputActionReference interactAction;

    //----------public-variables-upside-----private-variables-downside-------------
    bool isAlive = true;
    bool isInteracting = false;
    Vector2 moveDirection;

    private void Update()
    {
        moveDirection = moveAction.action.ReadValue<Vector2>();
        Debug.LogWarning(moveDirection);
    }
    
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);

        #region Flip sprite
        if (moveDirection.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveDirection.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        #endregion
    }
}