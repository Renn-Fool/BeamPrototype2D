using UnityEngine;

public class Antimatter : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            DestroyPlayer(collision.gameObject);
        }
    }

    private void DestroyPlayer(GameObject player)
    {

        Destroy(player);
    }
}
