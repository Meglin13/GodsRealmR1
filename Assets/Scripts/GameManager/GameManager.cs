using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(InventoryScript))]
[RequireComponent(typeof(PartyManager))]
[RequireComponent(typeof(MiniMapManager))]
[RequireComponent(typeof(ColorManager))]
[RequireComponent(typeof(RunManager))]
public class GameManager : MonoBehaviour, IManager
{
    public static GameManager Instance { get; private set; }

    public static GameManager GetInstance()
    {
        if (Instance == null)
            Instance = new GameManager();
        return Instance;
    }

    [Range(0f, 2f)]
    public float TimeScale = 1f;

    [HideInInspector]
    public InventoryScript inventory;
    [HideInInspector]
    public PartyManager partyManager;
    [HideInInspector]
    public MiniMapManager miniMapManager;
    [HideInInspector]
    public ColorManager colorManager;
    [HideInInspector]
    public RunManager runManager;

    [Header("Default Prefabs")]
    public GameObject DamagePopUp;
    public GameObject HitVFX;
    public GameObject HealthBar;
    public GameObject InventoryCellPrefab;

    public UnityEngine.InputSystem.InputActionAsset playerInput;

    [Header("Misc")]
    public List<CharacterScript> characters;

    public float CaptionLifeTime = 5f;

    [Header("Parry")]
    public float ParryTime = 0.8f;

    public Modifier ParryMod = new Modifier(StatType.CritChance, 100F);

    [Header("Items")]
    public List<Item> ItemsList;

    private void Awake()
    {
        Instance = this;

        Initialize();
    }

    public void Initialize()
    {
        Time.timeScale = 1;

        //DontDestroyOnLoad(this);

        GetItems();

        inventory = GetComponent<InventoryScript>();
        partyManager = GetComponent<PartyManager>();
        miniMapManager = GetComponent<MiniMapManager>();
        colorManager = GetComponent<ColorManager>();

        miniMapManager.Initialize();

        MainCameraInitialize(Camera.main);
    }

    private void Start()
    {
        partyManager.Initialize(characters);

        //TODO: Решить чо делать в баффами команды
        //partyManager.ApplyTeamBuff();
        inventory.Initialize();
    }

    private void Update()
    {
        //Time.timeScale = TimeScale;
    }

    public void GetItems()
    {
        ItemsList = Resources.LoadAll("ScriptableObjects/Items", typeof(Item)).Cast<Item>().ToList();
    }

    void MainCameraInitialize(Camera camera)
    {
        camera.orthographic = true;
        camera.orthographicSize = 5.5f;

        camera.farClipPlane = 1000;
        camera.nearClipPlane = -1000;

        camera.depth = -1;
        camera.cullingMask = ~(1 << AIUtilities.MiniMapLayer);

        camera.transform.rotation = Quaternion.Euler(40, -45, 0);
        camera.transform.position = new Vector3(0, 1, 0);
    }
}