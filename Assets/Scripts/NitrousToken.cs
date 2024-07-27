using UnityEngine;

public class NitrousToken : MonoBehaviour
{
    public AudioClip nitrousSound; // Assign the audio clip in the Inspector
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Find the car's nitrous system and add nitrous
            other.GetComponent<CarNitrousSystem>().AddNitrous(10f); // Add 10 units of nitrous

            // Play the nitrous sound
            audioSource.PlayOneShot(nitrousSound);

            // Destroy the token after a short delay to allow the sound to play
            Destroy(gameObject, nitrousSound.length); // Destroy after the audio clip length
        }
    }
}
