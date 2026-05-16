using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform Destination;
    public Animator Anim;
    public float TeleportDelay = 2f;

    private IEnumerator Teleport(Transform _Player)
    {
        yield return new WaitForSeconds(TeleportDelay);
        _Player.transform.position = Destination.position;
        Destroy(gameObject);
        yield return null;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Anim.Play("Open");
            StartCoroutine(Teleport(collision.transform));
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Anim.Play("Close");
            StopAllCoroutines();
        }
    }
}
