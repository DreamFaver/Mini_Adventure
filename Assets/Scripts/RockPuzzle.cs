using UnityEngine;

public class RockPuzzle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Moveable"))
        {
            collision.gameObject.layer = 0; // default layer
        }
    }
}