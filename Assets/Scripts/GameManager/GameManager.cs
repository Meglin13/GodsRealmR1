using ObjectPooling;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InventoryScript))]
[RequireComponent(typeof(PartyManager))]
[RequireComponent(typeof(MiniMapManager))]
[RequireComponent(typeof(ColorManager))]
[RequireComponent(typeof(RunManager))]
public class GameManager : MonoBehaviour, ISingle
{
    public static GameManager Instance { get; private set; }

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
    public DialogueSystem.DialogueManager dialogueManager;

    [Header("Default Prefabs")]
    public VFXScript DamagePopUp;
    public VFXScript HitVFX;
    public GameObject HealthBar;

    public InputActionAsset playerInput;

    [Header("Misc")]
    [SerializeField]
    private List<CharacterScript> characters;
    public static List<CharacterScript> Characters;

    [Header("Parry")]
    public float ParryTime = 0.8f;

    public Modifier ParryMod = new Modifier(StatType.CritChance, 100F);

    [Header("Items")]
    [HideInInspector]
    public List<EquipmentItem> EquipmentList;
    [HideInInspector]
    public List<PotionItem> PotionList;

    private void Awake()
    {
        Instance = this;

        Initialize();
    }

    public void Initialize()
    {
        Time.timeScale = 1;

        Instance = this;

        //DontDestroyOnLoad(this);

        GetItems();

        inventory = GetComponent<InventoryScript>();
        partyManager = GetComponent<PartyManager>();
        miniMapManager = GetComponent<MiniMapManager>();
        colorManager = GetComponent<ColorManager>();

        dialogueManager.Initialize();
        miniMapManager.Initialize();

        MainCameraInitialize(Camera.main);
    }

    private void Start()
    {
        if (characters != null & characters.Count > 0)
        {
            Debug.Log("Using preselected chars");
            partyManager.Initialize(characters);
        }
        else
        {
            Debug.Log("Using selected chars " + Characters.Count);
            partyManager.Initialize(Characters);
        }

        SetPoolers();

        //TODO: Решить чо делать в баффами команды
        //partyManager.ApplyTeamBuff();
        inventory.Initialize();
    }

    [SerializeField]
    PoolerScript<SpawningObject> spawnings;
    [SerializeField]
    PoolerScript<VFXScript> vfxs;
    [SerializeField]
    PoolerScript<EnemyScript> enemies;

    private void SetPoolers()
    {
        if (spawnings)
        {
            foreach (var item in partyManager.PartyMembers)
            {
                foreach (var skill in item.EntityStats.SkillSet.Values)
                {
                    if (skill.SpawningObject)
                        spawnings.objectsList.AddItem(skill.SpawningObject, 2);
                }
            }
        }
    }

    public void SetCharacters(List<CharacterScript> characters)
    {
        Characters = characters;
    }

    private void GetItems()
    {
        EquipmentList = Resources.LoadAll("ScriptableObjects/Items/Equipment", typeof(EquipmentItem)).Cast<EquipmentItem>().ToList();
        PotionList = Resources.LoadAll("ScriptableObjects/Items/Potions", typeof(PotionItem)).Cast<PotionItem>().ToList();
    }

    void MainCameraInitialize(Camera camera)
    {
        camera.orthographic = true;
        camera.orthographicSize = 5.5f;

        camera.depth = -1;
        camera.cullingMask = ~(1 << AIUtilities.MiniMapLayer);

        camera.transform.SetPositionAndRotation(new Vector3(0, 1, 0), Quaternion.Euler(40, -45, 0));
    }
}