using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text label;   

    int playerIndex;

   
    public void Init(int index, string playerName)
    {
        playerIndex = index;
        label.text = $"{playerName}: 0";
    }

   
    public void UpdateScore(int score)
    {
        label.text = $"P{playerIndex + 1}: {score}";
    }
}
