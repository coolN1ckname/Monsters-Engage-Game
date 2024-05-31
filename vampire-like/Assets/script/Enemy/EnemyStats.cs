using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyStats : MonoBehaviour
{
     public EnemyScriptableObject enemyData;

     //текущая стата
     [HideInInspector]
     public float currentMoveSpeed;
     [HideInInspector]
     public float currentHealth;
     [HideInInspector]
     public float currentDamage;
     Transform player;

     [Header("Damage Feedback")]
     public Color damageColor = new Color(1,0,0,1);
     public float damageFlashDuration = 0.2f;
     public float deathFadeTime = 0.6f;
     Color originalColor;
     SpriteRenderer sr;
     EnemyMovement movement;

     void Awake()
     {
         currentMoveSpeed = enemyData.MoveSpeed;
         currentHealth = enemyData.MaxHealth;
         currentDamage = enemyData.Damage;
     }
  
     void Start()
     {
       player = FindObjectOfType<PlayerStats>().transform;
       sr = GetComponent<SpriteRenderer>();
       originalColor = sr.color;

       movement= GetComponent<EnemyMovement>();
     }

     public void TakeDamage(float dmg, Vector2 sourcePosition, float knockbackForce = 5f, float knockbackDuration = 0.2f)
     {
          currentHealth -=dmg;
          StartCoroutine(DamageFlash());

          if(knockbackForce > 0)
          {
               Vector2 dir = (Vector2)transform.position - sourcePosition;
               movement.Knockback(dir.normalized * knockbackForce, knockbackDuration);
          }

          if(currentHealth<=0)
              Kill();
     }

     IEnumerator DamageFlash()
     {
          sr.color=damageColor;
          yield return new WaitForSeconds(damageFlashDuration);
          sr.color=originalColor;
     }
     public void Kill()
     {
          StartCoroutine(KillFade());
     }

     IEnumerator KillFade()
     {
          WaitForEndOfFrame w = new WaitForEndOfFrame();
          float t = 0, origAlpha = sr.color.a;

          while(t<deathFadeTime)
          {
               yield return w;
               t+= Time.deltaTime;

               sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (1-t/deathFadeTime)*origAlpha);
          }
          Destroy(gameObject);
     }

     private void OnCollisionStay2D(Collision2D col) 
     {
          if(col.gameObject.CompareTag("Player"))
          {
              PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
              player.TakeDamage(currentDamage);
          }
     }

     private void OnDestroy()
     {
          EnemySpawner es = FindObjectOfType<EnemySpawner>();
          es.OnEnemyKilled();
     }
}
