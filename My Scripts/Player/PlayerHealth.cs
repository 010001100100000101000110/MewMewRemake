using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    PlayerHelper helper;
    
    [SerializeField] private GameEvent playerDeathEvent;
    [SerializeField] private AudioEvent playerDeathAudioEvent;
    [SerializeField] private AudioEvent playerHit;
    private ScoreManager scoreManager;
    private LeaderboardManager leaderboardManager;
    private Animator animator;
    float maxHealth;
    float currentHealth;
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;

    bool hasDied;

    public bool invulnerable;
    public bool Shield;    

    public delegate void OnHealthChange(float health);
    public OnHealthChange healthChange;

    public delegate void DamageTaken();
    public DamageTaken PlayerHit;

    //private PlayerStatHandler statHandler;
    [SerializeField] float tempShieldAfterDamageTaken = 0.5f;
    private bool dashInvulnerability;
    public void EnableDashInv() => dashInvulnerability = true;
    public void DisableDashInv() => dashInvulnerability = false;

    private void OnDisable()
    {
        invulnerable = false;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        helper = GetComponent<PlayerHelper>();
        maxHealth = helper.Stats.MaxHealth;
        currentHealth = maxHealth;
        hasDied = false;
        healthChange?.Invoke(currentHealth);
        egoSystem = GetComponent<EgoSystem>();
        scoreManager = FindObjectOfType<ScoreManager>();
        leaderboardManager = FindObjectOfType<LeaderboardManager>();
        fireEffect.SetActive(false);
    }

    void Update()
    {
        TakeFireDamage(fireDamageAmount);
    }
    public void Heal(float amount)
    {
        if (currentHealth + amount > maxHealth) currentHealth = maxHealth;
        else currentHealth += amount;
        healthChange?.Invoke(currentHealth);
        // Ääniefekti healille
        FloatingTextManager.InstantiateFloatingText(transform.position, amount.ToString(), FloatingTextManager.Instance.Heal);
    }

    public void TakeDamage(float damage, GameObject damageGiver)
    {
        if (invulnerable || dashInvulnerability || Shield || helper.TopGun.TopGunInvulnerability) 
        {
            FloatingTextManager.InstantiateFloatingText(transform.position, "Blocked", FloatingTextManager.Instance.Blocked);
            return;
        }



        currentHealth -= damage;
        switch (damage)
        {
            case (<= 5):
            {
                    EffectManager.instance.CameraShake(0.25f);
                    break;
            }
            case (<= 10):
                {
                    EffectManager.instance.CameraShake(0.5f);
                    break;
                }
            case (<= 15):
                {
                    EffectManager.instance.CameraShake(1f);
                    break;
                }

            default:
                {
                    EffectManager.instance.CameraShake(0.1f);
                    break;
                }
        }
        
        healthChange?.Invoke(currentHealth);
        //SFXManager.RequestSound(playerHit);
        PlayerVoicelineManager.PlayAudio(playerHit);
        StartCoroutine(TemporaryInvulnerability());
        if (currentHealth <= 0 && !hasDied)
        {
            animator.SetTrigger("Death");
            playerDeathEvent?.Raise();
            //SFXManager.RequestSound(playerHit);
            PlayerVoicelineManager.PlayAudio(playerHit);
            SFXManager.RequestSound(playerDeathAudioEvent);
            hasDied = true;
            return;
        }
        PlayerHit?.Invoke();
        egoSystem.ReduceEgo();
    }
    EgoSystem egoSystem;
    private IEnumerator TemporaryInvulnerability()
    {
        invulnerable = true;
        yield return new WaitForSeconds(tempShieldAfterDamageTaken);
        invulnerable = false;
    }

    public void SetScoreOnDeath()
    {
        leaderboardManager.UpdateScore(scoreManager.Score);
    }


    #region Fire Damage Things

    //kesken 

    bool takingFireDamage;
    float nextDamage;
    [SerializeField] float timeBetweenFireDamages;
    [SerializeField] float fireDamageDuration;
    [SerializeField] float fireDamageAmount;
    [SerializeField] GameObject fireEffect;
    public bool FDCoroutineOn { get; private set; }

    public IEnumerator StartFireDamage()
    {
        FDCoroutineOn = true;
        if (!takingFireDamage)
        {
            fireEffect.SetActive(true);
            takingFireDamage = true;
            yield return new WaitForSecondsRealtime(fireDamageDuration);
            takingFireDamage = false;
            fireEffect.SetActive(false);
        }
        FDCoroutineOn = false;
    }


    public void TakeFireDamage(float damage)
    {
        if (takingFireDamage)
        {
            if (Time.time < nextDamage) return;
            nextDamage = Time.time + timeBetweenFireDamages;
            TakeDamage(damage, null);
        }
    }
    #endregion
}
