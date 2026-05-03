using UnityEngine;
using UnityEngine.InputSystem;

public class Ivan : MonoBehaviour
{
    
    float moveSpeed = 3f;
    bool isAlive = true;

    Vector2 move;
    bool jump = false;
    [Space(5)]
    public Rigidbody2D rb;
    public InputActionReference moveAction;
    public InputActionReference interactAction;
    public InputActionReference sprintAction;

}
