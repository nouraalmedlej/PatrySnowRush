using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameState { Menu, Playing, GameOver, Win }
public enum GameMode { CrystalRush, KingOfTheHill }

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    public GameMode mode = GameMode.CrystalRush;
    public GameState state = GameState.Menu;

    [SerializeField] float matchDuration = 60f;
    [SerializeField] GameObject[] players;

    [Header("UI Panels")]
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject hudPanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject winPanel;

    [Header("UI Elements")]
    [SerializeField] TMP_Text timerText;
    [SerializeField] Transform scoresRoot;
    [SerializeField] TMP_Text scoreRowPrefab;

    [Header("Modes")]
    [SerializeField] GameObject crystalsRoot;
    [SerializeField] GameObject hillZone;

    float _timeLeft;
    readonly Dictionary<int, int> _scores = new();
    readonly Dictionary<int, TMP_Text> _scoreLabels = new();

    void Awake()
    {
        if (I == null) I = this;
        else { Destroy(gameObject); return; }
    }

    void Start()
    {
        ShowMenu();
    }

    void Update()
    {
        if (state != GameState.Playing) return;

        _timeLeft -= Time.deltaTime;
        if (timerText) timerText.text = Mathf.CeilToInt(_timeLeft).ToString();
        if (_timeLeft <= 0f) EndGame();

        if (mode == GameMode.CrystalRush && crystalsRoot && crystalsRoot.transform.childCount == 0)
            WinGame();
    }

    public void ShowMenu()
    {
        state = GameState.Menu;
        if (startPanel) startPanel.SetActive(true);
        if (hudPanel) hudPanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (winPanel) winPanel.SetActive(false);

        _scores.Clear();
        ClearScoresUI();

        foreach (var p in players)
            if (p) p.SetActive(false);

        if (crystalsRoot) crystalsRoot.SetActive(false);
        if (hillZone) hillZone.SetActive(false);
    }

    public void StartGame()
    {
        state = GameState.Playing;
        if (startPanel) startPanel.SetActive(false);
        if (hudPanel) hudPanel.SetActive(true);
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (winPanel) winPanel.SetActive(false);

        _timeLeft = matchDuration;

        _scores.Clear();
        ClearScoresUI();

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i])
            {
                players[i].SetActive(true);
                RegisterPlayer(i);
            }
        }

        if (crystalsRoot) crystalsRoot.SetActive(mode == GameMode.CrystalRush);
        if (hillZone) hillZone.SetActive(mode == GameMode.KingOfTheHill);
    }

    public void EndGame()
    {
        state = GameState.GameOver;
        if (hudPanel) hudPanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(true);
        if (winPanel) winPanel.SetActive(false);

        if (crystalsRoot) crystalsRoot.SetActive(false);
        if (hillZone) hillZone.SetActive(false);
    }

    void WinGame()
    {
        state = GameState.Win;
        if (hudPanel) hudPanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (winPanel) winPanel.SetActive(true);

        if (crystalsRoot) crystalsRoot.SetActive(false);
        if (hillZone) hillZone.SetActive(false);
    }

    public void RestartToMenu() => ShowMenu();

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    void RegisterPlayer(int playerIndex)
    {
        if (_scores.ContainsKey(playerIndex)) return;
        _scores[playerIndex] = 0;

        if (scoreRowPrefab && scoresRoot)
        {
            TMP_Text row = Instantiate(scoreRowPrefab, scoresRoot);
            row.text = $"P{playerIndex + 1}: 0";
            _scoreLabels[playerIndex] = row;
        }
    }

    public void AddScore(int playerIndex, int amount = 1)
    {
        if (!_scores.ContainsKey(playerIndex)) RegisterPlayer(playerIndex);
        _scores[playerIndex] += amount;

        if (_scoreLabels.TryGetValue(playerIndex, out var label))
            label.text = $"P{playerIndex + 1}: {_scores[playerIndex]}";
    }

    public void UI_StartCrystalRush()
    {
        mode = GameMode.CrystalRush;
        StartGame();
    }

    public void UI_StartHill()
    {
        mode = GameMode.KingOfTheHill;
        StartGame();
    }

    void ClearScoresUI()
    {
        if (!scoresRoot) return;
        for (int i = scoresRoot.childCount - 1; i >= 0; i--)
            Destroy(scoresRoot.GetChild(i).gameObject);
        _scoreLabels.Clear();
    }
}
