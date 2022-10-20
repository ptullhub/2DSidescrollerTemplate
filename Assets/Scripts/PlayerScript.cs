using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;
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

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
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
        if(collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }
}
