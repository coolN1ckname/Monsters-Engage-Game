using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStats : MonoBehaviour
{
    public CharacterScriotableObject characterData;

    //статы
    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;

    #region Current Stats Properties
    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if(currentHealth != value)
                currentHealth= value;
                if(GameManager.instance != null)
                    GameManager.instance.currentHealthDisplay.text = "Здоровье: " + currentHealth;
        }
    }

    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {
            if(currentRecovery != value)
                currentRecovery= value;
                if(GameManager.instance != null)
                    GameManager.instance.currentRecoveryDisplay.text = "Восстановление: " + currentRecovery;
        }
    }

    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            if(currentMoveSpeed != value)
                currentMoveSpeed= value;
                if(GameManager.instance != null)
                    GameManager.instance.currentMoveSpeedDisplay.text = "Скорость Движения: " + currentMoveSpeed;
        }
    }
    public float CurrentMight
    {
        get { return currentMight; }
        set
        {
            if(currentMight != value)
                currentMight= value;
                if(GameManager.instance != null)
                    GameManager.instance.currentMightDisplay.text = "Сила: " + currentMight;
        }
    }
    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set
        {
            if(currentProjectileSpeed != value)
                currentProjectileSpeed= value;
                if(GameManager.instance != null)
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Скорость снаряда: " + currentProjectileSpeed;
        }
    }

    #endregion



    public int maxHealth;
    //опыт и уровни персонажа
    [Header("Experience/Level")]
    public int experiance = 0;
    public int level = 1;
    public int experianceCap;

    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experianceCapIncrease;
    }

    //I-frames
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    public List<LevelRange> levelRanges;

    InventoryManager inventory;
    public int passiveItemIndex;

    [Header("UI")]
    public Image healthBar;
    public Image expBar;
    public Text levelText;
    public int weaponIndex;


    public GameObject firstPassiveItemTest, secondPassiveItemTest, thirdPassiveItem;
    public GameObject weaponTest;

    void Awake()
    {
        inventory = GetComponent<InventoryManager>();

        CurrentHealth = characterData.MaxHealth;
        CurrentRecovery = characterData.Recovery;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMight = characterData.Might;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;

        SpawnPassiveItem(firstPassiveItemTest);
        SpawnPassiveItem(thirdPassiveItem);

    }

    void Start()
    {
        experianceCap = levelRanges[0].experianceCapIncrease;

        GameManager.instance.currentHealthDisplay.text = "Здоровье: " + currentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Восстановление: " + currentRecovery;
        GameManager.instance.currentMoveSpeedDisplay.text = "Скорость Движения: " + currentMoveSpeed;
        GameManager.instance.currentMightDisplay.text = "Сила: " + currentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Скорость снаряда: " + currentProjectileSpeed;
        UpdateHealthBar();
        UpdateExpBar();
        UpdateLevelText();
    }

    void Update()
    {
        if(invincibilityTimer >0)
            invincibilityTimer -= Time.deltaTime;
        else if (isInvincible)
            isInvincible = false;
        Recover();
        GameManager.instance.currentHealthDisplay.text = "Здоровье: " + currentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Восстановление: " + currentRecovery;
        GameManager.instance.currentMoveSpeedDisplay.text = "Скорость Движения: " + currentMoveSpeed;
        GameManager.instance.currentMightDisplay.text = "Сила: " + currentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Скорость снаряда: " + currentProjectileSpeed;
    }

    public void IncreaseExperience(int amount)
    {
        experiance += amount;
        LevelUpChecker();
        UpdateExpBar();
    }

    void LevelUpChecker()
    {
        if(experiance >= experianceCap)
        {
            level++;
            experiance -=experianceCap;
            int experianceCapIncrease = 0;
            foreach ( LevelRange range in levelRanges)
            {
                if(level >= range.startLevel && level <= range.endLevel)
                {
                    experianceCapIncrease = range.experianceCapIncrease;
                    break;
                }
            }
            experianceCap += experianceCapIncrease;

            currentRecovery += 0.2f ;    
            currentMight += 0.25f;
            currentHealth+= 10f;
            currentMoveSpeed+=0.1f;

            UpdateLevelText();
        }
    }

    void UpdateExpBar()
    {
        expBar.fillAmount = (float)experiance / experianceCap;
    }
    
    void UpdateLevelText()
    {
        levelText.text = "LVL " + level.ToString();
    }

    public void TakeDamage(float dmg)
    {
        if(!isInvincible)
        {
            CurrentHealth -= dmg;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if(CurrentHealth <= 0)
                Kill();

            UpdateHealthBar();
        } 
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / characterData.MaxHealth;
    }


    public void Kill()
    {
        if(!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignChosenPassiveItemsUI(inventory.passiveItemsUISlots);
            GameManager.instance.GameOver();
        }
    }

    public void RestoreHealth(float amount)
    {
        if(CurrentHealth < characterData.MaxHealth)
        {    
            CurrentHealth += amount;
            if (CurrentHealth > characterData.MaxHealth)
                CurrentHealth = characterData.MaxHealth;
            UpdateHealthBar();
        }
    }


    void Recover()
    {
        if(CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth+=CurrentRecovery*Time.deltaTime;
            if(CurrentHealth>characterData.MaxHealth)
                CurrentHealth=characterData.MaxHealth;
            UpdateHealthBar();
        }
    }

    public void SpawnPassiveItem(GameObject passiveItem)
    {
        if(passiveItemIndex >= inventory.passiveItemSlots.Count -1)
        {
            Debug.LogError("Инвентарь заполнен");
            return;
        }
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItems>());

        passiveItemIndex++;
    }

   
}
