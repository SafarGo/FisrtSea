using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
    public float ShipHP = 100f;

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

    [Header("Поля для элементов")]
    public TMP_Text timer_text;
    public TMP_Text _text;
    public Slider Slider;


    private void Awake()
    {
        instance = this;
    }

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
        GetFormattedTime();
        Slider.value = ShipHP;
    }

    void InitializeGame()
    {
        currentTime = gameDuration;
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
        if(disastersResolved == 2 && victimRescued)
        {
           _text.text = "УСПЕХ";
        }
        if (currentTime <= 0 && victimRescued && disastersResolved == 2)
        {
            CompleteGame("Успех", "Полный успех");
            return;
        }
        if (currentTime <= 0 && disastersResolved != 2 && !victimRescued)
        {
            CompleteGame("Неудача", "Не выполнены условия");
        }

        if(Slider.value ==0)
        {
            CompleteGame("Неудача", "Корабль потонул");
        }
    }

    void CheckLoseConditions()
    {
        if (!shipIntact)
        {
            CompleteGame("Неудача", "Неудача");
            return;
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

        _text.text = $"{resultMessage}";
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

    public void GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timer_text.text =  $"{minutes:00}:{seconds:00}";
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