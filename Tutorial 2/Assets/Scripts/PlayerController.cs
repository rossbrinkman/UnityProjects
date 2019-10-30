using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2d;

    public float speed;

    public Text livesText;
    public Text score;
    public Text winText;

    private int lives;
    public int scoreValue = 0;
    private bool gameOver = false;
    public bool hasSwitched;

     AudioSource audioSource;
    public AudioClip audioClip;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
        audioSource = GetComponent<AudioSource>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        score.text = "Score: " + scoreValue.ToString();
        winText.text = "";
        bool hasSwitched = false;
        SetLivesText();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rb2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (gameOver == true)
        {
            Restart();
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.gameObject);
            if (scoreValue == 6 && hasSwitched == false)
            {
                StartCoroutine(Wait());

            }
            if (scoreValue == 12 && hasSwitched == true)
            {
                winText.text = "You Win! Game created by Ross Brinkman!\n\nPress R to Restart.";
                audioSource.clip = audioClip;
                audioSource.Play();
                rb2d.velocity = Vector2.zero;
                rb2d.isKinematic = true;
                GetComponent<SpriteRenderer>().enabled = false;
                gameOver = true;
            }
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);
            lives = lives - 1;
            SetLivesText();
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            Jump();
        }
    }


    void SetLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();
        if (lives <= 0)
        {
            winText.text = "You Lose!\n\nPress R to Restart.";
            rb2d.velocity = Vector2.zero;
            rb2d.isKinematic = true;
            GetComponent<SpriteRenderer>().enabled = false;
            gameOver = true;
        }
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
        }
    }

    void Restart()
    {
        if (Input.GetKey("r"))
        {
            SceneManager.LoadScene("Main");
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        transform.position = new Vector3(0, 50f, 0);
        rb2d.velocity = Vector2.zero;
        lives = 3;
        SetLivesText();
        hasSwitched = true;
    }
}