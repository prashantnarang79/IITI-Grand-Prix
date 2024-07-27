using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Anim : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public static Anim instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TriggerTransition()
    {
        StartCoroutine(PlayTransition());
    }

    private IEnumerator PlayTransition()
    {
        // Play the start transition animation
        transition.SetTrigger("Start");

        // Wait for the transition to finish
        yield return new WaitForSeconds(transitionTime);

        // Play the end transition animation
        transition.SetTrigger("End");

        // Wait for the end transition to finish
        yield return new WaitForSeconds(transitionTime);
    }
}