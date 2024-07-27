using UnityEngine;
using UnityEngine.UI;

public class CarNitrousSystem : MonoBehaviour
{
    public float maxNitrous = 100f;
    public float currentNitrous;
    public float nitrousConsumptionRate = 10f;
    public float boostMultiplier = 2f;
    public RectTransform nitrousBarFill;

    private Rigidbody carRigidbody;
    private float originalWidth;

    void Start()
    {
        GameObject canvasObject = GameObject.Find("Canvas");

        if (canvasObject != null)
        {
            Transform nitrousBarFillTransform = canvasObject.transform.Find("NitrousBarBackground/NitrousBarFill");

            if (nitrousBarFillTransform != null)
            {
                nitrousBarFill = nitrousBarFillTransform.GetComponent<RectTransform>();
            }
        }

        carRigidbody = GetComponent<Rigidbody>();
        currentNitrous = 0f;
        originalWidth = nitrousBarFill.sizeDelta.x;
    }

    void Update()
    {
        // Update the nitrous bar UI
        if (nitrousBarFill != null)
        {
            float fillWidth = (currentNitrous / maxNitrous) * originalWidth;
            nitrousBarFill.sizeDelta = new Vector2(fillWidth, nitrousBarFill.sizeDelta.y);
            UpdateNitrousBarColor();
        }

        // Check for boost input
        if (Input.GetKey(KeyCode.N) && currentNitrous > 0)
        {
            Boost();
        }
    }

    public void AddNitrous(float amount)
    {
        currentNitrous = Mathf.Clamp(currentNitrous + amount, 0, maxNitrous);
    }

    private void Boost()
    {
        carRigidbody.AddForce(transform.forward * boostMultiplier, ForceMode.Acceleration);
        currentNitrous -= nitrousConsumptionRate * Time.deltaTime;
        if (currentNitrous < 0)
        {
            currentNitrous = 0;
        }
    }

    private void UpdateNitrousBarColor()
    {
        // Change color based on the amount of nitrous
        Image nitrousBarImage = nitrousBarFill.GetComponent<Image>();
        if (currentNitrous / maxNitrous > 0.5f)
        {
            nitrousBarImage.color = Color.green;
        }
        else if (currentNitrous / maxNitrous > 0.25f)
        {
            nitrousBarImage.color = Color.yellow;
        }
        else
        {
            nitrousBarImage.color = Color.red;
        }
    }
}