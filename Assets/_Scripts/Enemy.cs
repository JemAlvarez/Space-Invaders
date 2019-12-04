using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header ("VFX")]
    [SerializeField] GameObject explosionParticles;
    [SerializeField] float durationExplosion = 1f;

    [Header("Sprites")]
    [SerializeField] Sprite originalSprite;
    [SerializeField] Sprite movingSprite;
    [SerializeField] float spriteRateOfChange = 1f;

    [Header("Bullet")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] float bulletSpeed = 5f;

    [Header("Life/Death")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreOnDeath = 1;

    [Header("SFX")]
    [SerializeField] AudioClip enemyDeath;
    [SerializeField] AudioClip enemyShoot;
    [SerializeField]  [Range(0,1)] float deathVolume = 0.7f;
    [SerializeField] [Range(0, 1)] float shootVolume = 0.7f;

    SpriteRenderer spriteRenderer;
    //AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // sprite effects
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(MoveSprite());

        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bulletSpeed);
        AudioSource.PlayClipAtPoint(enemyShoot, Camera.main.transform.position, shootVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer dmgDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!dmgDealer) { return; }
        ProcessHit(dmgDealer);
    }

    private void ProcessHit(DamageDealer dmgDealer)
    {
        health -= dmgDealer.GetDmg();
        dmgDealer.Hit();

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        var explosion = Instantiate(explosionParticles, transform.position, Quaternion.identity);
        Destroy(explosion, durationExplosion);
        AudioSource.PlayClipAtPoint(enemyDeath, Camera.main.transform.position, deathVolume);
        FindObjectOfType<GameSession>().AddToScore(scoreOnDeath);
    }


    // Sprite effect
    private IEnumerator MoveSprite()
    {
        while (true)
        {
            if (spriteRenderer.sprite == originalSprite)
            {
                spriteRenderer.sprite = movingSprite;
            }
            else if (spriteRenderer.sprite == movingSprite)
            {
                spriteRenderer.sprite = originalSprite;
            }

            yield return new WaitForSeconds(spriteRateOfChange);
        }
    }
}
