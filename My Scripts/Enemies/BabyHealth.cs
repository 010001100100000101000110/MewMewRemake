                                  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyHealth : MonoBehaviour, IDamageable
{
    private AudioSource audioSource;
    private float currentHealth;
    private float maxHealth;
    bool hasDied;
    public delegate void DamageEnemy();
    public DamageEnemy damageEnemy;
    EnemyHelper helper;
    [SerializeField] AudioEvent hitSFX;
    EnemyDeadBodyPool bodyPool;

    PowerupSpawner powerupSpawner;
    Instakill instakill;

    private void OnEnable()
    {
        Enabled();
    }
    void Enabled()
    {
        hasDied = false;
        currentHealth = helper.Stats.Health;
        if (rd == null) rd = GetComponent<SpriteRenderer>();
        rd.material.SetFloat("_HitEffectBlend", 0);
    }

    public void Instakill()
    {
        instakill.InstantiateInstakillPrefab(this.transform.position);
    }

    public bool TakeDamage(float damage)
    {
        if (!hasDied)
        {
            float dmg = damage;
            string text = instakill.IsActive ? "INSTAKILL" : Mathf.RoundToInt(damage).ToString();
            if (instakill.IsActive)
            {
                dmg = currentHealth;
                Instakill();
            }
            FloatingTextManager.InstantiateFloatingText(transform.position, text, FloatingTextManager.Instance.DamageColor);

            currentHealth -= dmg;
            if (currentHealth <= 0)
            {
                if (!hasDied)
                {
                    FloatingTextManager.InstantiateFloatingText(transform.position, text, FloatingTextManager.Instance.DamageColor);
                    Die();                 
                    return true;
                }
            }
            helper.Manager.NotifyHealthChange(transform, currentHealth, maxHealth);

            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(HitEffect());
                if (hitSFX == null) Debug.LogWarning("Hit sfx puuttuu");
                SFXManager.RequestSound(hitSFX, audioSource);
                damageEnemy?.Invoke();
            }

            return false;
        }

        else return false;

    }


    private void Awake()
    {
        helper = GetComponent<EnemyHelper>();
        rd = GetComponent<SpriteRenderer>();
        bodyPool = FindObjectOfType<EnemyDeadBodyPool>();
        powerupSpawner = FindObjectOfType<PowerupSpawner>();
        instakill = FindObjectOfType<Instakill>();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    [SerializeField] AudioEvent deathSound;


    public void Die()
    {
        gameObject.SetActive(false);
        hasDied = true;
        //helper.Manager.EnemiesKilled++;
        helper.Manager.AddToEnemiesKilled(gameObject, false);
        SFXManager.RequestSound(deathSound);
        powerupSpawner.SpawnPowerupIfAllowed(transform.position);
        bodyPool.GetBody(helper.Stats.Type, this.transform);

        if (helper.Manager.EnemiesKilled % 20 == 0) helper.Manager.PlayEnemyKilledVoiceLine();
    }

    SpriteRenderer rd;
    float colorFlashDuration = 0.12f;

    private IEnumerator HitEffect()
    {
        Material m = rd.material;
        float timer = 0;
        float dur = colorFlashDuration;
        float val = 0;

        while (timer <= dur)
        {
            timer += Time.deltaTime;
            if (timer < dur * 0.5f)
            {
                val = Mathf.Lerp(val, 1, timer / dur * 0.5f);
            }

            else
            {
                val = Mathf.Lerp(val, 0, timer - (dur * 0.5f) / dur);
            }
            m.SetFloat("_HitEffectBlend", val);
            yield return null;
        }

        m.SetFloat("_HitEffectBlend", 0);
    }

}
