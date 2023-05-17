using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriceratopsHealth : MonoBehaviour, IDamageable
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


    private void OnEnable()
    {
        if (maxHealth == 0) maxHealth = helper.Stats.Health;
        //currentHealth = maxHealth + helper.Manager.GetEnemyHealthMultiplier(maxHealth);
        hasDied = false;
        currentHealth = helper.Stats.Health;
        if (rd == null) rd = GetComponent<SpriteRenderer>();
        rd.material.SetFloat("_HitEffectBlend", 0);
    }


    public bool TakeDamage(float damage)
    {
        if (!hasDied)
        {
            if (damage >= 9999)
            {
                damage = Mathf.Clamp(damage, 0, 600);
            }
            float dmg = damage;
            string text = Mathf.RoundToInt(damage).ToString();
            
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
                Debug.Log("BITCH");
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
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    [SerializeField] AudioEvent deathSound;

    public void Die()
    {
        hasDied = true;
        helper.Manager.EnemiesKilled++;
        helper.Manager.PlayEnemyKilledVoiceLine();
        helper.Manager.AddToEnemiesKilled(gameObject);
        SFXManager.RequestSound(deathSound);
        bodyPool.GetBody(helper.Stats.Type, this.transform);

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
