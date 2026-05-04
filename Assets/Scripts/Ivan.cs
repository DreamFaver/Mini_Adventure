using UnityEngine;
using UnityEngine.InputSystem;

public class Ivan : MonoBehaviour
{
    #region Variables
    public float moveSpeed = 0f;
    public float interactionRange = 0f;
    [Space(5)]
    public Rigidbody2D rb;
    public InputActionReference moveAction;
    public InputActionReference interactAction;

    //----------public-variables-upside-----private-variables-downside-----------

    bool isAlive = true;
    Vector2 moveDirection;

    GameObject nearbyMoveableObject = null;
    #endregion

    private void Update()
    {
        if (!isAlive) return;

        moveDirection = moveAction.action.ReadValue<Vector2>(); // Read move input

        if (interactAction.action.triggered) // triggered is true when the action state changes to being pressed
            FindNearestObject(); // Finds the nearest object that is moveable
    }
    
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
        FlipSprite();
    }

    //-------------Functions---------------------------------------------------
    
    private void FlipSprite()
    {
        if (moveDirection.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveDirection.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void FindNearestObject()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactionRange);

        GameObject closestObject = null;
        float minDistance = interactionRange;

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Moveable"))
            {
                Vector3 closestPoint = hitCollider.ClosestPoint(transform.position);
                float distance = Vector2.Distance(transform.position, closestPoint);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestObject = hitCollider.gameObject;
                }
            }
        }

        nearbyMoveableObject = closestObject;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}