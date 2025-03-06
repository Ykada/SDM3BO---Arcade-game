using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class DemoPlayerControls : MonoBehaviour
{

    public Transform[] shotPos;
    public Transform top, bottom, schootanm;
    public Rigidbody bulletPrefab, grenadePrefab;
    public GameObject footsteps, hitSound, muzFlashPrefab, bloodPrefab, bloodPoolPrefab, gameOver, gunobject, loadingmap;

    [HideInInspector] public bool _isHit, _pickedUpHealth, _hasMaxHealth;

    public float moveSpeed = 10f,
                  fireNext = 0.0f,
                  fireRate = 0.2f,
                  fireForce = 1500f,
                  fireSpread = 6f,
                  grenadeNext = 0.0f,
                  grenadeRate = 2.0f,
                  healMaxTimer = 0.0f,
                  healTime = 0.0f,
                  healRate = 0.5f,
                  startHealth = 100,
                  maxHealth = 300,
                  currentHealth,
                  Health;

    private AudioSource audioFootsteps, audioHitSound;

    private Animator animBottom, animTop, anim;
    private string animRun = "Run";
    private bool _stoppedMoving;

    // Recoil variables
    public float recoilAmount = 1f;        // How much recoil happens
    public float recoilRecoverySpeed = 5f; // How quickly the gun recovers
    private Vector3 originalGunPosition;   // The starting position of the gun
    private Quaternion originalGunRotation; // The starting rotation of the gun

    void Start()
    {
        audioFootsteps = footsteps.GetComponent<AudioSource>();
        audioHitSound = hitSound.GetComponent<AudioSource>();
        animBottom = bottom.GetComponent<Animator>();
        animTop = top.GetComponent<Animator>();
        Health = 100;
        currentHealth = startHealth;
        healTime = healRate;

        // Save the original position and rotation of the gun
        originalGunPosition = gunobject.transform.localPosition;
        originalGunRotation = gunobject.transform.localRotation;
    }

    void Update()
    {
        HealthManager();
        // Smoothly recover the gun's position and rotation back to its original state
        gunobject.transform.localPosition = Vector3.Lerp(gunobject.transform.localPosition, originalGunPosition, Time.deltaTime * recoilRecoverySpeed);
        gunobject.transform.localRotation = Quaternion.Lerp(gunobject.transform.localRotation, originalGunRotation, Time.deltaTime * recoilRecoverySpeed);
    }

    void FixedUpdate()
    {
        WeaponsManager();
    }

    void LateUpdate()
    {
    }

    private void HealthManager()
    {
        if (_isHit)
        {
            healTime = 0.0f;
            Health -= 5;
            float rand = Random.value;
            if (rand <= 0.5f)
            {
                audioHitSound.PlayOneShot(audioHitSound.clip);
            }
            _isHit = false;
        }
        else
        {
            healTime += Time.deltaTime;
            if (healTime > healRate)
            {
                if (Health <= currentHealth)
                {
                    Health++;
                }
            }
        }

        if (_pickedUpHealth)
        {
            currentHealth = maxHealth;
            Health = currentHealth;
            _hasMaxHealth = true;
            _pickedUpHealth = false;
        }
        if (_hasMaxHealth)
        {
            healMaxTimer += Time.deltaTime;
            if (healMaxTimer > 30)
            {
                currentHealth = startHealth;
                if (Health > currentHealth)
                {
                    Health = currentHealth;
                }
                _hasMaxHealth = false;
                healMaxTimer = 0.0f;
            }
        }

        if (Health <= 0f)
        {
            Instantiate(bloodPoolPrefab, transform.position, Quaternion.identity);
            loadingmap.SetActive(true);
            Instantiate(gameOver);
            Cursor.lockState = CursorLockMode.None;
            Destroy(gameObject);
        }
    }

    private void WeaponsManager()
    {
        if (Input.GetMouseButton(0) && Time.time > fireNext)
        {
            fireNext = Time.time + fireRate;
            StartCoroutine(Shoot());
        }
        if (Input.GetKey(KeyCode.F) && Time.time > grenadeNext)
        {
            grenadeNext = Time.time + grenadeRate;
            ShootProjectile(grenadePrefab, shotPos[1], 500, 0);
        }
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.0f);

        // Apply recoil to the gun
        ApplyRecoil();

        // Continue shooting the projectiles
        ShootProjectile(bulletPrefab, shotPos[0], fireForce, fireSpread);
        GameObject firstFlash = Instantiate(muzFlashPrefab, shotPos[0].position, shotPos[0].rotation) as GameObject;
        firstFlash.transform.parent = shotPos[0];
        Destroy(firstFlash, 0.5f);

        yield return new WaitForSeconds(0.1f);
    }

    public void Bleed(Quaternion rot)
    {
        GameObject blood = Instantiate(bloodPrefab, transform.position, rot) as GameObject;
        Destroy(blood, 3);
    }

    private void ShootProjectile(Rigidbody shotPrefab, Transform shotPosition, float shotForce, float shotSpread)
    {
        Quaternion shotRotation;
        shotRotation = shotPosition.rotation;
        if (shotSpread != 0)
        {
            float randUp = Random.Range(-shotSpread, shotSpread);
            shotRotation *= Quaternion.AngleAxis(randUp, shotPosition.up);
            float randRight = Random.Range(-shotSpread, shotSpread);
            shotRotation *= Quaternion.AngleAxis(randRight, shotPosition.right);
        }

        Rigidbody shot = Instantiate(shotPrefab, shotPosition.position, shotRotation) as Rigidbody;
        shot.AddForce(shot.transform.forward * shotForce);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.name.Contains("Spit"))
        {
            _isHit = true;
        }
    }

    void OnGUI()
    {
        ShowHealth();
    }

    private void ShowHealth()
    {
        Texture2D lifeTexture = new Texture2D(10, 10);
        int y = 0;
        while (y < lifeTexture.height)
        {
            int x = 0;
            while (x < lifeTexture.width)
            {
                if (Health >= currentHealth / 2)
                {
                    lifeTexture.SetPixel(x, y, Color.green);
                }
                else
                {
                    lifeTexture.SetPixel(x, y, Color.red);
                }
                x++;
            }
            y++;
        }
        lifeTexture.Apply();
        GUI.DrawTexture(new Rect(10, 10, Screen.width / 4 * Health / 200, Screen.height / 50), lifeTexture);
    }

    // Apply recoil effect to the gun in multiple directions (side-to-side, up-and-down, back)
    void ApplyRecoil()
    {
        // Apply random recoil in all directions: X (side-to-side), Y (up-and-down), Z (back)
        float recoilX = Random.Range(-recoilAmount, recoilAmount); // Side-to-side
        float recoilY = Random.Range(-recoilAmount, recoilAmount); // Up-and-down
        float recoilZ = recoilAmount; // Backward (Z direction)

        // Apply the recoil
        gunobject.transform.localPosition -= new Vector3(recoilX, recoilY, recoilZ);

        // Apply a small random rotation for added effect
        float rotationX = Random.Range(-recoilAmount, recoilAmount);
        float rotationY = Random.Range(-recoilAmount, recoilAmount);
        gunobject.transform.localRotation *= Quaternion.Euler(rotationX, rotationY, 0);
    }
}
