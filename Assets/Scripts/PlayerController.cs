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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        isOnGround = true;
        Physics.gravity *= gravityModifier;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isOnGround)// && !GameManager.gameOver)
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
        if (collision.gameObject.name == "Ground")// && !GameManager.gameOver)
        {
            isOnGround = true;
            dirtParticle.Play();
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            dirtParticle.Stop();
            explosionParticle.Play();
            playerAnimator.SetBool("Death_b", true);
            playerAudio.PlayOneShot(crashSound, 1.0f);
            GameManager.gameOver = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Scoreable"))
        {
            GameManager.ChangeScore(5);
        }
    }
}
