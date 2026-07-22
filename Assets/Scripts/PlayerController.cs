using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce, gravityModifier;
    [SerializeField] private ParticleSystem explosionParticle, dirtParticle;
    [SerializeField] private AudioClip jumpSound, crashSound;
    private Animator playerAnimator;
    private AudioSource playerAudio;
    private Rigidbody playerRb;
    private bool isOnGround;
    private bool gameStarted;
    private float animationSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        isOnGround = true;
        Physics.gravity *= gravityModifier;
        animationSpeed = playerAnimator.GetFloat("Speed_f");
        playerAnimator.SetFloat("Speed_f", 0);
    }

    void Update()
    {
        if(!GameManager.Instance.gameOver && !gameStarted)
        {
            StartGame();
        }
        else if(GameManager.Instance.gameOver)
        {
            playerAnimator.SetFloat("Speed_f", 0);
            dirtParticle.Stop();
        }
    }

    private void StartGame()
    {
        gameStarted = true;
        playerAnimator.SetFloat("Speed_f", animationSpeed);
        isOnGround = true;
        dirtParticle.Play();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isOnGround && !GameManager.Instance.gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnimator.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ground" && !GameManager.Instance.gameOver)
        {
            isOnGround = true;
            dirtParticle.Play();
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            playerAnimator.SetBool("Death_b", true);
            dirtParticle.Stop();
            explosionParticle.Play();
            playerAudio.PlayOneShot(crashSound, 1.0f);
            UIManager.Instance.GameOver();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Scoreable"))
        {
            Debug.Log("Received a Score");
            UIManager.Instance.UpdateScore(5);
        }
    }
}
