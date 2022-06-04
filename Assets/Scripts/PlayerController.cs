using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody playerRb;
    BoxCollider playerBoxCollider;
    Animator playerAnim;
    AudioSource playerAudioSource;
    AudioSource mainCameraAudioSource;

    [SerializeField] GameObject unstoppableRing;
    [SerializeField] Text starsText;

    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] ParticleSystem starPickupParticles;
    [SerializeField] ParticleSystem unstoppableModeExplosionParticles;

    [SerializeField] AudioClip starPickupSound;
    [SerializeField] AudioClip powerIconPickupSound;
    [SerializeField] AudioClip unstoppableModeBumpSound;

    [SerializeField] float speed = 6;
    //[SerializeField] float jumpForce = 7;
    [SerializeField] float horizontalBound = 7;

    bool isGrounded = true;
    bool unstoppable;
    int stars;

    void Start()
    {
        playerBoxCollider = GetComponent<BoxCollider>();
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudioSource = GetComponent<AudioSource>();
        mainCameraAudioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        //GameManager.GameAcceleration += ChangeAnimationSpeed;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (unstoppable)
            {
                unstoppable = false;
                unstoppableRing.SetActive(false);
            }
            else
            {
                unstoppable = true;
                unstoppableRing.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (playerBoxCollider.enabled)
            {
                playerBoxCollider.enabled = false;
                playerRb.isKinematic = true;
            }
            else
            {
                playerRb.isKinematic = false;
                playerBoxCollider.enabled = true;
            }
        }
#endif
        MoveSideways();
        Jump();
        ConstrainPlayerPosition();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (!unstoppable)
            {
                Rigidbody obstacleRb = collision.gameObject.GetComponent<Rigidbody>();
                obstacleRb.AddForce(Vector3.forward * GameManager.CharacterSpeed, ForceMode.Impulse);
                StopGame();
            }
            else
            {
                Destroy(collision.gameObject);
                unstoppableModeExplosionParticles.Play();
                playerAudioSource.PlayOneShot(unstoppableModeBumpSound, 0.3f);
                playerRb.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                unstoppable = false;
                unstoppableRing.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Star"))
        {
            stars++;
            starsText.text = "Stars :" + stars;
            starPickupParticles.Play();
            playerAudioSource.PlayOneShot(starPickupSound);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Power Icon"))
        {
            unstoppable = true;
            unstoppableRing.SetActive(true);
            Destroy(other.gameObject);
            playerAudioSource.PlayOneShot(powerIconPickupSound);
        }
    }

    void MoveSideways()
    {
        if (GameManager.IsGameActive)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            //playerRb.AddForce(Vector3.right * speed * horizontalInput);
            transform.Translate(horizontalInput * speed * Time.deltaTime * Vector3.right);
        }
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            //playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //playerAnim.SetTrigger("Jump_trig");
            isGrounded = false;

        }
    }

    void ConstrainPlayerPosition()
    {
        if (transform.position.x < -horizontalBound)
        {
            transform.position = new Vector3(-horizontalBound, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > horizontalBound)
        {
            transform.position = new Vector3(horizontalBound, transform.position.y, transform.position.z);
        }
    }

    public void ChangeAnimationSpeed()
    {
        if (playerAnim != null)
            playerAnim.speed *= GameManager.GameAccelerator;
    }

    void StopGame()
    {
        GameManager.IsGameActive = false;
        GameManager.Stars = stars;
        explosionParticles.Play();

        playerAnim.SetBool("Death_b", true);
        playerAnim.SetInteger("DeathType_int", 1);

        mainCameraAudioSource.Stop();
        playerAudioSource.Play();
    }

}


