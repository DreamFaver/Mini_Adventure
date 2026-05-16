using UnityEngine;
using UnityEngine.InputSystem;

public class Ivan : MonoBehaviour
{
    #region Variables
    public float normalMoveSpeed = 0f;
    public float grabMoveSpeed = 0f;
    public float interactionRange = 0f;

    [Space(5)]
    public Rigidbody2D rb;
    public InputActionReference moveAction;
    public InputActionReference interactAction;

    [Header("Animation")]
    [SerializeField] private Animator Anim;
    [SerializeField] private PlayerState AnimState;

    public bool isAlive = true;

    //----------public/private----------

    private bool isGrabbing = false;
    private float currentMoveSpeed;

    private enum PlayerState : byte
    {
        Idle,
        Jog,
        GrabIdle,
        Grab,
        None
    }

    private Vector2 moveDirection;
    private Vector3 offset;
    private GameObject grabbedObject;
    #endregion

    private void Update()
    {
        if (!isAlive) return;
        currentMoveSpeed = isGrabbing ? grabMoveSpeed : normalMoveSpeed;

        moveDirection = moveAction.action.ReadValue<Vector2>(); // Read move input

        if (interactAction.action.IsPressed()) // triggered is true when the action state changes to being pressed
            TryToGrabObject();
        else if (isGrabbing) // release the object if the button is not being held
        {
            isGrabbing = false;
            grabbedObject = null;
        }
        SwitchState();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * currentMoveSpeed, rb.linearVelocity.y);
        FlipSprite();

        if (isGrabbing && grabbedObject) // Move the object if it's grabbed
        {
            Vector3 newPosition = transform.position + offset;
            grabbedObject.transform.position = newPosition;
        }
    }

    //----------Functions-----------------------------------------
    #region Animations
    private void SwitchState()
    {
        if (moveDirection.magnitude > 0 && !isGrabbing)
            AnimState = PlayerState.Jog;
        if (moveDirection.magnitude == 0 && !isGrabbing)
            AnimState = PlayerState.Idle;
        if (moveDirection.magnitude > 0 && isGrabbing)
            AnimState = PlayerState.Grab;
        if (moveDirection.magnitude == 0 && isGrabbing)
            AnimState = PlayerState.GrabIdle;

        ApplyAnimation(AnimState);
    }
    private void ApplyAnimation(PlayerState _State)
    {
        switch(_State)
        {
            case PlayerState.Idle: Anim.Play("Idle"); 
                break;
            case PlayerState.Jog:
                Anim.Play("Jog");
                break;
            case PlayerState.GrabIdle:
                Anim.Play("Grab Idle");
                break;
            case PlayerState.Grab:
                Anim.Play("Grab Walk");
                break;
            default: Anim.Play("Idle");
                break;
        }
    }
    #endregion
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
        }
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

        grabbedObject = closestObject;
        if (grabbedObject)
        {
            isGrabbing = true;
            offset = grabbedObject.transform.position - transform.position;
        }
    }
    #endregion

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}