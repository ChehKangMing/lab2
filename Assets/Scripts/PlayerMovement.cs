using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    // move
    public float maxSpeed = 20;
    public float speed = 10;
    private Rigidbody2D marioBody;
    // jump
    public float upSpeed = 10;
    private bool onGroundState = true;
    // flip mario
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    // button
    public TextMeshProUGUI scoreText;
    public GameObject enemies;

    public JumpOverGoomba jumpOverGoomba;
    public GameOver gameOver;

    // animation
    public Animator marioAnimator;

    // for audio
    public AudioSource marioAudio;
    public AudioClip marioDeath;

    [System.NonSerialized]
    public bool alive = true;

    // death
    public float deathImpulse;

    // camera
    public Transform gameCamera;

    // layer mask
    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);

    // question box and brick
    public QuestionBox questionBox;
    public Brick brick;



    // Start is called before the first frame update
    void Start()
    {
        // set to be 30 fps
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();

        // get sprite component
        marioSprite = GetComponent<SpriteRenderer>();

        // update animator state
        marioAnimator.SetBool("onGround", onGroundState);
    }

    // Update is called once per frame
    void Update()
    {
        // toggle state
        if (Input.GetKeyDown("a") && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;

            if (marioBody.velocity.x > 0.1f)
            {
                marioAnimator.SetTrigger("onSkid");
            }
        }

        if (Input.GetKeyDown("d") && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;

            if (marioBody.velocity.x < -0.1f)
            {
                marioAnimator.SetTrigger("onSkid");
            }
        }

        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
    }

    // FixedUpdate is called 50 times a second, used when has to do with physics engine
    void FixedUpdate()
    {
        if (alive)
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(moveHorizontal) > 0)
            {
                Vector2 movement = new Vector2(moveHorizontal, 0);
                // check if it does not go beyond max speed
                if (marioBody.velocity.magnitude < maxSpeed)
                {
                    marioBody.AddForce(movement * speed);
                }
            }

            // stop
            if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
            {
                // stop
                marioBody.velocity = Vector2.zero;
            }

            // jump
            if (Input.GetKeyDown("space") && onGroundState)
            {
                marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
                onGroundState = false;
                // update animator state
                marioAnimator.SetBool("onGround", onGroundState);
            }
        }
    }


    // if mario body hit/collide with the ground, set state flag to true
    void OnCollisionEnter2D(Collision2D col)
    {
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with Goomba!");
            //gameOver.Setup(jumpOverGoomba.score);
            //Time.timeScale = 0.0f;

            // play death animation
            marioAnimator.Play("Mario_die");
            marioAudio.PlayOneShot(marioDeath);
            alive = false;
        }
    }

    public void RestartButtonCallback(int input)
    {
        Debug.Log("Restart!");
        RestartGame();
        Time.timeScale = 1.0f;
    }

    public void RestartGame()
    {
        // reset position
        marioBody.transform.position = new Vector3(-11.45f, -7.78f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        scoreText.text = "Score: 0";
        // reset goomba
        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.transform.localPosition = eachChild.GetComponent<EnemyMovement>().startPosition;
        }

        jumpOverGoomba.score = 0;

        //reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset camera position
        gameCamera.position = new Vector3(0, 0, -10);
    }

    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }


    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    void GameOverScene()
    {
        // stop time
        Time.timeScale = 0.0f;
        // set game over scene
        gameOver.Setup(jumpOverGoomba.score);
    }
}