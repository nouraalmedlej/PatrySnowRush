using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour
{
    public int value = 1;

    void Reset()
    {
        var c = GetComponent<Collider2D>();
        c.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<PlayerInput>(out var player)) return;

        if (GameManager.I == null)
        {
            Debug.LogError("Collectible: GameManager.I is null.");
            return;
        }

        GameManager.I.AddScore(player.playerIndex, value);
        Destroy(gameObject);
    }
}
