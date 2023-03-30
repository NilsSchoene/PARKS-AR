using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.VFX;

public class ARInteractable : MonoBehaviour
{
    // AudioScource Reference (AR Session Origin)
    [SerializeField]
    [Tooltip("AudioScource Reference (AR Session Origin)")]
    private AudioSource audioSource;

    // Audio Clip to play.
    [SerializeField]
    [Tooltip("Audio Clip to play.")]
    private AudioClip audioToPlay;

    // Success Clip, when Station found
    [SerializeField]
    [Tooltip("Success Sound to play, when station found")]
    private AudioClip successSound;

    // Max Scaling of the speaker 3D object
    [SerializeField]
    [Tooltip("Max Scaling of the speaker 3D object")]
    private float maxScaling = 10;

    [SerializeField]
    private TMP_Text infoText;

    // Option to skip Audio (for testing only)
    private bool skipAudio = false;

    // How fast the speaker dissolves after being clicked
    [SerializeField]
    private float dissolveRate = 0.00125f;
    private float counter;
    private bool dissolvingEnabled = false;

    // ScalingSpeed of the AR Interactable
    private float scalingFactor;
    private bool scaling = false;

    // Duration of the audioclip
    private float duration;

    private bool blockInteraction = false;

    private MeshRenderer mesh;
    private Material material;

    private UIManager uiManager;
    private ConfettiManager confettiManager;

    /// <summary>
    /// Sets all relevant parameters on awake.
    /// </summary>
    void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        confettiManager = FindObjectOfType<ConfettiManager>();
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        audioSource.GetComponent<AudioSource>();
        mesh = gameObject.GetComponent<MeshRenderer>();
        duration = audioToPlay.length;
        mesh.enabled = true;
        blockInteraction = false;
        material = mesh.material;
        scalingFactor = maxScaling / 200;
    }

    /// <summary>
    /// Animations in runtime
    /// </summary>
    void Update()
    {
        transform.Rotate(0.0f, 2.0f, 0.0f, Space.Self);
        if (scaling == true && transform.localScale.x < maxScaling)
        {
            transform.localScale += new Vector3(scalingFactor, scalingFactor, scalingFactor);
        }

        if ((material.GetFloat("_DissolveAmount") < 1) && dissolvingEnabled == true)
        {
            confettiManager.ThrowConfetti();
            counter += dissolveRate;

            material.SetFloat("_DissolveAmount", counter);
        }
    }

    /// <summary>
    /// Starts Coroutine WaitForAudio on interaction.
    /// </summary>
    public void OnInteraction()
    {
        if(blockInteraction == false)
        {
            blockInteraction = true;
            StartCoroutine(WaitForAudio());
        }
    }

    /// <summary>
    /// starts scaling on first detection.
    /// </summary>
    public void OnDetection()
    {
        scaling = true;
        uiManager.OnStationDetected();
        infoText.text = "Station gefunden! Suche den Lautsprecher!";
        audioSource.PlayOneShot(successSound, 0.5f);
    }

    /// <summary>
    /// Coroutine plays AudioClip and waits for its duration and let's speaker dissolve, then calls OnStationComplete on UIManager.
    /// </summary>
    private IEnumerator WaitForAudio()
    {
        audioSource.clip = audioToPlay;
        dissolvingEnabled = true;

        if (skipAudio == false)
        {
            audioSource.Play();
            infoText.text = "Audio wird abgespielt.";
            yield return new WaitForSeconds(duration);
        }
        else if (skipAudio == true)
        {
            infoText.text = "Audio skipped";
            yield return new WaitForSeconds(5);
        }
        infoText.text = "Klicke auf Weiter, um fortzufahren.";
        blockInteraction = false;
        dissolvingEnabled = false;
        scaling = false;
        material.SetFloat("_DissolveAmount", 0);
        counter = 0;
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        uiManager.OnStationComplete();
        audioSource.PlayOneShot(successSound, 0.5f);
        gameObject.SetActive(false);
    }
}
