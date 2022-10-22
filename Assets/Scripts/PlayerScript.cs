using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;
    private bool facingRight = true;
    public float speed;
    public Text score;
    private int scoreValue = 0;
    private int lives = 3;
    public Text livesText;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    public GameObject player;
    public AudioSource musicSource;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        livesText.text = "Lives: " + lives.ToString();
        score.text = scoreValue.ToString();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;
    }
    void ScoreCheck()
    {
        if (scoreValue == 4)
        {
            transform.position = new Vector2(0.0f, 50f);
            lives = 3;
            livesText.text = "Lives: " + lives.ToString();
            
        }
        if (scoreValue == 8)
        {
            winTextObject.SetActive(true);
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            musicSource.loop = false;
            Destroy(rd2d);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    void KillPlayer()
    {
        livesText.text = "Lives: " + lives.ToString();
        if (lives == 0)
        {
            player.SetActive(false);
            loseTextObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }
        if (isOnGround == false)
        {
            anim.SetInteger("State", 3);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue++;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);

            ScoreCheck();
        }
        else if (collision.collider.tag == "Enemy")
        {
            lives--;
            livesText.text = "Lives: " + lives.ToString();
            Destroy(collision.collider.gameObject);

            KillPlayer();
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }

}
