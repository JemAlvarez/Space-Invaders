using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //config params
    [Header("Player Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 0.2f;                    // Move boundaires padding

    [Header("Bullet")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float bulletFiringPeriod = 0.1f;
    [SerializeField] float bulletOffset = 0.2f;

    [Header ("VFX")]
    [SerializeField] float durationExplosion = 1f;
    [SerializeField] GameObject explosionParticles;

    [Header ("Life/Death")]
    [SerializeField] int health = 1000;

    [Header ("SFX")]
    [SerializeField] AudioClip playerDeath;
    [SerializeField] AudioClip playerShoot;
    [SerializeField] [Range(0, 1)] float deathVolume = 0.7f;
    [SerializeField] [Range(0, 1)] float shootVolume = 0.7f;


    Level levelManager;
    Coroutine firingCoroutine;

    // Boundaries variables
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        //Get level manager
        levelManager = FindObjectOfType<Level>();

        SetUpMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer dmgDealer = collision.gameObject.GetComponent<DamageDealer>();
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
        AudioSource.PlayClipAtPoint(playerDeath, Camera.main.transform.position, deathVolume);
        levelManager.LoadGameOver();
    }

    // Player Shoot
    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    // Continuous Fire Coroutine
    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y + bulletOffset, transform.position.z), Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bulletSpeed);
            AudioSource.PlayClipAtPoint(playerShoot, Camera.main.transform.position, shootVolume);
            yield return new WaitForSeconds(bulletFiringPeriod);
        }
    }

    // Move character
    private void Move()
    {
        // Change in position
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var detalY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        // New postion
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + detalY, yMin, yMax);

        // Update position
        transform.position = new Vector2(newXPos, newYPos);
    }

    // Setup movement boundaries
    private void SetUpMoveBoundaries()
    {
        // Get camera object
        Camera gameCamera = Camera.main;

        // Setup x boundaries
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        // Setup y boundaries
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    public int GetHealth()
    {
        return health;
    }
}
