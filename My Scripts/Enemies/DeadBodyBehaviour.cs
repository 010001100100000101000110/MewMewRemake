using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBodyBehaviour : MonoBehaviour
{
    [SerializeField] EnemyType type;
    Animator anim;
    EnemyDeadBodyPool bodyPool;
    SpriteRenderer rd;
    float colorFlashDuration = 0.12f;
    void Awake()
    {
        anim = GetComponent<Animator>();
        bodyPool = FindObjectOfType<EnemyDeadBodyPool>();
        rd = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        StartCoroutine(HitEffect());
        StartCoroutine(AddBodyToPoolAfterSeconds());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator AddBodyToPoolAfterSeconds()
    {
        float time = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
        bodyPool.ReturnBody(this.gameObject, type);
    }
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
