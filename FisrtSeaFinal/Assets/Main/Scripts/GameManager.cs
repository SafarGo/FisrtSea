using System.Resources;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Настройки игры")]
    public float gameDuration = 300f;
    public float min_time = 30f;
    public float max_time = 60f;

    [Header("Текущее состояние")]
    public string currentGameState = "Playing";
    public float currentTime = 0f;
    public int disastersResolved = 0;
    public bool victimRescued = false;
    public bool shipIntact = true;
    public int totalDisasters = 0;

    [Header("Счетчики")]
    public int maxConcurrentDisasters = 2;
    public int activeDisastersCount = 0;
    public int crewMembersAvailable = 2;

    [Header("Статические события")]
    public static bool OnGameStart = false;
    public static bool OnGameWin = false;
    public static bool OnGameLose = false;
    public static bool OnDisasterResolved = false;
    public static bool OnVictimRescued = false;
    public static bool OnShipDamaged = false;

    void Start()
    {
        InitializeGame();
    }

    void Update()
    {
        if (currentGameState != "Playing") return;

        UpdateGameTimer();
        CheckWinConditions();
        CheckLoseConditions();
        ResetStaticEvents();
    }

    void InitializeGame()
    {
        currentTime = gameDuration; // Начинаем с максимального времени
        disastersResolved = 0;
        activeDisastersCount = 0;
        currentGameState = "Playing";

        OnGameStart = true;
    }

    void UpdateGameTimer()
    {
        currentTime -= Time.deltaTime;
        if (currentTime < 0) currentTime = 0;
    }

    void CheckWinConditions()
    {
        // Полный успех: время вышло + все условия выполнены
        if (currentTime <= 0 && shipIntact && victimRescued && activeDisastersCount == 0)
        {
            CompleteGame("Victory", "Полный успех");
            return;
        }

        // Базовый успех: время вышло, корабль цел
        if (currentTime <= 0 && shipIntact)
        {
            CompleteGame("Victory", "Безопасно пришвартован");
        }
    }

    void CheckLoseConditions()
    {
        if (!shipIntact)
        {
            CompleteGame("Failure", "Повреждён и потерпел крушение");
            return;
        }

        if (crewMembersAvailable <= 0)
        {
            CompleteGame("Failure", "Экипаж выбыл из строя");
        }
    }

    public void CompleteGame(string endState, string resultMessage)
    {
        currentGameState = endState;

        if (endState == "Victory")
        {
            OnGameWin = true;
        }
        else if (endState == "Failure")
        {
            OnGameLose = true;
        }

        Debug.Log($"Игра завершена: {resultMessage}");
    }

    public void ReportDisasterResolved()
    {
        disastersResolved++;
        activeDisastersCount--;
        OnDisasterResolved = true;
    }

    public void ReportNewDisaster()
    {
        totalDisasters++;
        activeDisastersCount++;
    }

    public void ReportVictimRescued()
    {
        victimRescued = true;
        OnVictimRescued = true;
    }

    public void ReportShipDamage()
    {
        shipIntact = false;
        OnShipDamaged = true;
    }

    public void ReportCrewMemberStatus(bool isAvailable)
    {
        crewMembersAvailable += isAvailable ? 1 : -1;
    }

    public float GetRemainingTime()
    {
        return Mathf.Max(0f, currentTime);
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    public float GetGameProgress()
    {
        return Mathf.Clamp01(1f - (currentTime / gameDuration));
    }

    public bool CanSpawnDisaster()
    {
        return activeDisastersCount < maxConcurrentDisasters && currentGameState == "Playing";
    }

    void ResetStaticEvents()
    {
        OnGameStart = false;
        OnGameWin = false;
        OnGameLose = false;
        OnDisasterResolved = false;
        OnVictimRescued = false;
        OnShipDamaged = false;
    }
}