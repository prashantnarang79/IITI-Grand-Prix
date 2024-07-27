using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;
using Photon.Pun;
using Photon.Realtime;

public class RaceManager : MonoBehaviourPunCallbacks
{
    public TMP_Text timerText;
    public TMP_Text countdownText;
    public GameObject finishLine;
    public Image fadeImage;
    public GameObject resultsPanel;
    public TMP_Text resultsText;

    public AudioClip countDownClip3;
    public AudioClip countDownClip2;
    public AudioClip countDownClip1;
    public AudioClip countDownClipGo;

    private AudioSource audioSource;
    private float startTime;
    private bool raceStarted = false;
    private bool raceFinished = false;
    private bool countdownActive = false;

    // ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    void Start()
    {

        // timerText = transform.Find("Canvas/TimerText").GetComponent<TMP_Text>();
        GameObject canvasObject = GameObject.Find("Canvas");

        if (canvasObject != null)
        {
            Transform timerTextTransform = canvasObject.transform.Find("TimerText");

            if (timerTextTransform != null)
            {
                timerText = timerTextTransform.GetComponent<TMP_Text>();
            }

            Transform countdownTextTransform = canvasObject.transform.Find("CountdownText");

            if (countdownTextTransform != null)
            {
                countdownText = countdownTextTransform.GetComponent<TMP_Text>();
            }

            Transform fadeImageTransform = canvasObject.transform.Find("FadeImage");

            if (fadeImageTransform != null)
            {
                fadeImage = fadeImageTransform.GetComponent<Image>();
            }

            Transform resultsPanelTransform = canvasObject.transform.Find("ResultsPanel");

            if (resultsPanelTransform != null)
            {
                resultsPanel = resultsPanelTransform.gameObject;
            }

            Transform resultsTextTransform = canvasObject.transform.Find("ResultsPanel/ResultText");

            if (resultsTextTransform != null)
            {
                resultsText = resultsTextTransform.GetComponent<TMP_Text>();
            }
        }

        finishLine = GameObject.Find("FinishLine");
        //finishLine = GameObject.Find("FinishLine").GetComponent<GameObject>();

        audioSource = GetComponent<AudioSource>();
        resultsPanel.SetActive(false);  // Hide results panel at the start
        StartCoroutine(StartRace());

    }

    void Update()
    {
        if (raceStarted && !raceFinished)
        {
            float t = Time.time - startTime;
            string minutes = Mathf.Floor(t / 60).ToString("00");
            string seconds = (t % 60).ToString("00.00");
            timerText.text = minutes + ":" + seconds;
        }
    }

    IEnumerator StartRace()
    {
        yield return new WaitForSeconds(1f); // Optional delay before starting countdown

        FadeOut(); // Fade out effect before countdown starts

        countdownActive = true;
        StartCoroutine(CountdownRoutine(3));
        yield return new WaitForSeconds(4f); // Wait for countdown to finish
        countdownActive = false;

        countdownText.gameObject.SetActive(false);
        FadeIn(); // Fade in effect after countdown finishes

        startTime = Time.time;
        raceStarted = true;
    }

    IEnumerator CountdownRoutine(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            countdownText.text = i.ToString();
            PlayCountdownSound(i);
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "GO!";
        PlayCountdownSound(0); // Play "GO!" sound
        yield return new WaitForSeconds(1f);
    }

    void PlayCountdownSound(int count)
    {
        if (!countdownActive) return; // Ensure sound only plays during countdown

        switch (count)
        {
            case 3:
                audioSource.PlayOneShot(countDownClip3);
                break;
            case 2:
                audioSource.PlayOneShot(countDownClip2);
                break;
            case 1:
                audioSource.PlayOneShot(countDownClip1);
                break;
            case 0:
                audioSource.PlayOneShot(countDownClipGo);
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == finishLine)
        {
            raceFinished = true;
            timerText.color = Color.green;
            float raceTime = Time.time - startTime;
            ExitGames.Client.Photon.Hashtable raceTimeProperty = new ExitGames.Client.Photon.Hashtable {
                { "RaceTime", raceTime },
                { "HasFinished", true }
            };
            Debug.Log("Setting RaceTime property: " + raceTime);

            PhotonNetwork.LocalPlayer.SetCustomProperties(raceTimeProperty);

            // Call CheckResults with a delay to ensure properties are updated
            StartCoroutine(CallCheckResultsWithDelay());

        }
    }
    private IEnumerator CallCheckResultsWithDelay()
    {
        // Wait for a short time to ensure properties are updated
        yield return new WaitForSeconds(0.5f);
        RaceResults.Instance.CheckResults();
    }

    void FadeOut()
    {
        if (fadeImage != null)
        {
            fadeImage.CrossFadeAlpha(1f, 0f, false); // Ensure image is fully visible initially
            fadeImage.CrossFadeAlpha(0f, 2f, false); // Fade out effect over 2 seconds
        }
    }

    void FadeIn()
    {
        if (fadeImage != null)
        {
            fadeImage.CrossFadeAlpha(1f, 2f, false); // Fade in effect over 2 seconds
        }
    }
}