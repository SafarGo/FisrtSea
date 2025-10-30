using UnityEngine;

public class Disasters : MonoBehaviour
{
    public static Disasters instance;
    [Header("Префабы")]
    public GameObject Fire_prefab;
    public GameObject Black_prefab;

    [Header("Позиции")]
    public Transform PosFire;
    public Transform PosBlack;

    [Header("Границы тайминга")]
    public float Min_T;
    public float Max_T;

    private float _spawnTime1;
    private float _spawnTime2;
    public bool IsSpawned1 = false;
    public bool IsSpawned2 = false;

    private void Start()
    {
        _spawnTime1 = Random.Range(Min_T, Max_T);
        _spawnTime2 = Random.Range(Min_T, Max_T);
    }

    private void Update()
    {
        if ((GameManager.instance.gameDuration - GameManager.instance.currentTime) >=  _spawnTime1 && !IsSpawned1)
        {
            Instantiate(Fire_prefab, PosFire.position, Fire_prefab.transform.rotation);
            IsSpawned1 = true;
        }
        if ((GameManager.instance.gameDuration - GameManager.instance.currentTime) >= _spawnTime2 && !IsSpawned2)
        {
            Instantiate(Black_prefab, PosBlack.position, Black_prefab.transform.rotation);
            IsSpawned2 = true;
        }

        
    }
}
