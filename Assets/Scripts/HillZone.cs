using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HillZone : MonoBehaviour
{
    public float tickEvery = 1f;
    float t;
    HashSet<int> inside = new HashSet<int>();

    void Update()
    {
        if (inside.Count == 1)
        {
            t += Time.deltaTime;
            if (t >= tickEvery)
            {
                t = 0f;
                foreach (var idx in inside)
                {
                    GameManager.I.AddScore(idx, 1);
                    break;
                }
            }
        }
        else
        {
            t = 0f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var pi = other.GetComponentInParent<PlayerInput>();
        if (pi != null) inside.Add(pi.playerIndex);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var pi = other.GetComponentInParent<PlayerInput>();
        if (pi != null) inside.Remove(pi.playerIndex);
    }
}
