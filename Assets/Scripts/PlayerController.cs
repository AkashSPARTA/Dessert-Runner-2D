using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2d;
    public float runSpeed;
    private int jumpCount = 0;
    private bool canJump = true;
    public Animator anim;
    public bool isGameOver = false;
    public GameObject GameOverPanel, ScoreText;
    public Text FinalScoreText, HighScoreText;
    public AudioSource BGAudio;
    public AudioSource GameOverAudio;
    public GameObject MainInGameButton, InGameJumpButton;

    // Start is called before the first frame update
    void Start()
    {
        GameOverAudio.Stop();
        BGAudio.Play();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        StartCoroutine("IncreaseGameSpeed");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            transform.position = Vector3.right * runSpeed * Time.deltaTime + transform.position;
        }

        if ( jumpCount == 2)
        {
            canJump = false;
        }

        if (Input.GetKeyDown("space") && canJump && !isGameOver)
        {
            rb2d.velocity = Vector3.up * 7.5f;
            anim.SetTrigger("Jump");
            jumpCount += 1;
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        anim.SetTrigger("Death");
        StopCoroutine("IncreaseGameSpeed");
        BGAudio.Stop();
        MainInGameButton.SetActive(false);
        InGameJumpButton.SetActive(false);

        StartCoroutine("ShowGameOverPanel");
        GameOverAudio.Play();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
            canJump = true;
        }
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            GameOver();
        }
        if (collision.gameObject.CompareTag("BottomDetector"))
        {
            GameOver();
        }
    }


    IEnumerator IncreaseGameSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            if (runSpeed < 8)
            {
                runSpeed += 0.2f;
            }

            if (GameObject.Find("GroundSpawner").GetComponent<ObstacleSpawner>().obstacleSpawnInterval > 1)
            {
                GameObject.Find("GroundSpawner").GetComponent<ObstacleSpawner>().obstacleSpawnInterval -= 0.1f;
            }
            
        }
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(2);
        GameOverPanel.SetActive(true);
        ScoreText.SetActive(false);

        FinalScoreText.text = "Score : " + GameObject.Find("ScoreDetector").GetComponent<ScoreSystem>().score;
        HighScoreText.text = "High Score : " + PlayerPrefs.GetInt("HighScore");
    }

    public void JumpButton()
    {
        if (canJump && !isGameOver)
        {
            rb2d.velocity = Vector3.up * 7.5f;
            anim.SetTrigger("Jump");
            jumpCount += 1;
        }
    }
}
