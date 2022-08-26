using UnityEngine;

public class Text : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource audioSourceSneky;
    public AudioClip deathSound;

    public static bool paused = false;
    private bool playedDeathSound = false;

    public GameObject pauseMenuUI;
    public GameObject deathMenuUI;

    public GameObject scoreText;
    public GameObject speedText;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Sneky.dead)
        {
            if (paused) {ResumeGame();}
            else {PauseGame();}
        }

        if (Input.GetKeyDown(KeyCode.Space) && Sneky.dead) {NewGame();}
        if (Input.GetKeyDown(KeyCode.Backspace)) {QuitGame(); }

        if (Sneky.dead)
        {
            DeathScreen();
            if (Input.GetKeyDown(KeyCode.Escape)) {QuitGame();}
        }

        scoreText.GetComponent<UnityEngine.UI.Text>().text = "Score: " + Sneky.score;
    }

    public void ResumeGame()
    {
        audioSource.Play();
        audioSourceSneky.Play();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        paused = false;
    }

    public void NewGame()
    {
        deathMenuUI.SetActive(false);
        Time.timeScale = 1;
        playedDeathSound = false;
        FindObjectOfType<Sneky>().ResetState();
    }

    void PauseGame()
    {
        audioSource.Play();
        audioSourceSneky.Pause();
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        paused = true;
    }

    void DeathScreen()
    {
        audioSourceSneky.Stop();
        if (!playedDeathSound)
        {
            audioSource.PlayOneShot(deathSound);
            playedDeathSound = true;
        }
        deathMenuUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void QuitGame() {Application.Quit();}
}