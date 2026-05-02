using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ivan : MonoBehaviour
{
    float moveSpeed = 3f;
    bool isAlive = true;
    Vector2 zart;

    public Rigidbody2D rb;
    public InputActionReference moveAction;

    private void Start()
    { }

    void Update()
    {
        zart = moveAction.action.ReadValue<Vector2>();
    }
    
    private void OnEnable()
    {
        moveAction.action.Enable();
    }
    private void OnDisable()
    {
        moveAction.action.Disable();
    }
}
