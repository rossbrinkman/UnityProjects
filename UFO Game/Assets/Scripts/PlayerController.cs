using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;
    public Text winText;
    public Text livesText;

    private Rigidbody2D rb2d;
    private int count;
    private int lives;
    private bool hasSwitched;
    private bool gameOver = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        count = 0;
        lives = 3;
        winText.text = "";
        SetCountText();
        SetLivesText();
        bool hasSwitched = false;
    }

     void Update()
    {
        if (Input.GetKey("escape"))
            Application.Quit();

        if (gameOver == true)
        {
            Restart();
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb2d.AddForce(movement * speed);
    }

     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            lives = lives - 1;
            SetLivesText();
        }
    }

    void SetLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();
        if (lives <= 0)
        {
            winText.text = "You Lose! Game created by Ross Brinkman!\n\nPress R to Restart.";
            rb2d.velocity = Vector2.zero;
            rb2d.isKinematic = true;
            GetComponent<SpriteRenderer>().enabled = false;
            gameOver = true;
        }
    }

    void SetCountText()
        {
            countText.text = "Count: " + count.ToString();

        if (count >= 13 && hasSwitched == false)
        {
            StartCoroutine(Wait());
            
        }

            if (count >= 21)
        {
            winText.text = "You Win! Game created by Ross Brinkman!\n\nPress R to Restart.";
            rb2d.velocity = Vector2.zero;
            rb2d.isKinematic = true;
            GetComponent<SpriteRenderer>().enabled = false;
            gameOver = true;
            
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
        hasSwitched = true;
    }
}
