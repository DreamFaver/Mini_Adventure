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

    //----------public/private----------

    bool isAlive = true;
    bool isGrabbing = false;

    Vector2 moveDirection;
    GameObject nearbyMoveableObject = null;
    #endregion

    private void Update()
    {
        if (!isAlive) return;

        moveDirection = moveAction.action.ReadValue<Vector2>(); // Read move input

        if (interactAction.action.triggered) // triggered is true when the action state changes to being pressed
            TryToGrabObject(); // Finds the nearest object that is moveable
        if (isGrabbing && nearbyMoveableObject)
        {
            Vector3 newPosition = transform.position;
            nearbyMoveableObject.transform.position = newPosition;
        }
    }
    
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
        FlipSprite();
    }

    //----------Functions-----------------------------------------

    private void FlipSprite()
    {
        if (moveDirection.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveDirection.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    #region Interaction with moveable objects
    void TryToGrabObject()
    {
        if (!isGrabbing)
        {
            FindNearestObject(); // Puts the selected object in "nearbyMoveableObject"
            if (nearbyMoveableObject)
                GrabObject();
        }
        else if (isGrabbing)
            MoveGrabbedObject();
    }

    void GrabObject()
    {
        isGrabbing = true;
    }
    void ReleaseObject()
    {
        isGrabbing = false;
        nearbyMoveableObject = null;
    }

    void MoveGrabbedObject()
    {

    }

    void FindNearestObject() // Finds the nearest object that is moveable
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
    #endregion
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}