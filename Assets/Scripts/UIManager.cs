using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Welcome Panel Reference
    [SerializeField]
    [Tooltip("Welcome Panel Reference")]
    private GameObject welcomePanel;

    // Hints Panel Reference
    [SerializeField]
    [Tooltip("Hints Panel Reference")]
    private GameObject hintsPanel;

    // Station Complete Panel Reference
    [SerializeField]
    [Tooltip("Station Complete Reference")]
    private GameObject stationCompletePanel;

    // All Complete Panel Reference
    [SerializeField]
    [Tooltip("All Complete Reference")]
    private GameObject allCompletePanel;

    // Current Hint Text Reference
    [SerializeField]
    [Tooltip("Current Hint Text Reference")]
    private GameObject currentHintText;

    // Start Button Reference
    [SerializeField]
    [Tooltip("Start Button Reference")]
    private GameObject startButton;

    // Image to download
    [SerializeField]
    [Tooltip("Image to download")]
    private Texture2D imageToDownload;

    // Download Button
    [SerializeField]
    [Tooltip("Download Button")]
    private GameObject downloadButton;

    // Download Success Text
    [SerializeField]
    [Tooltip("Download Success Text")]
    private GameObject downloadSuccessText;

    //Score Text Field
    [SerializeField]
    private GameObject scoreText;

    [SerializeField]
    private TMP_Text text_scoreText;

    [SerializeField]
    private TMP_Text infoText;

    private int score = 0;

    private bool hintsActive = false;

    private bool infoActive = true;

    private bool blockHints = true;

    private bool blockInfo = true;

    private bool blockUI = false;

    private StationManager stationManager;

    void Awake()
    {
        stationManager = FindObjectOfType<StationManager>();
    }
    /// <summary>
    /// Toggles the Hint Panel if it isn't blocked.
    /// </summary>
    public void ToggleHints()
    {
        if (blockHints == false)
        {
            if (hintsActive == false)
            {
                hintsPanel.SetActive(true);
                hintsActive = true;
                blockInfo = true;
            }
            else if (hintsActive == true)
            {
                hintsPanel.SetActive(false);
                hintsActive = false;
                blockInfo = false;
            }
        }
    }

    /// <summary>
    /// Toggle Welcome Panel if it isn't blocked.
    /// </summary>
    public void ToggleInfo()
    {
        if (blockInfo == false)
        {
            if (infoActive == false)
            {
                welcomePanel.SetActive(true);
                infoActive = true;
                blockHints = true;
            }
            else if (infoActive == true)
            {
                welcomePanel.SetActive(false);
                infoActive = false;
                blockHints = false;
            }
        }
    }

    /// <summary>
    /// Either blocks or unblocks the Interaction with the UI.
    /// </summary>
    public void BlockUIToggle()
    {
        if (blockUI == false)
        {
            welcomePanel.SetActive(false);
            hintsPanel.SetActive(false);
            blockUI = true;
            blockHints = true;
            blockInfo = true;
            infoActive = false;
            hintsActive = false;
        }
        else if(blockUI == true)
        {
            blockUI = false;
            blockHints = false;
            blockInfo = false;
        }
    }

    /// <summary>
    /// Toggles the HintsPanel.
    /// </summary>
    public void OnHintMaximized()
    {
        if (hintsActive == false)
        {
            hintsPanel.SetActive(true);
            hintsActive = true;
            blockHints = false;
        }
        else if (hintsActive == true)
        {
            hintsPanel.SetActive(false);
            hintsActive = false;
            blockHints = true;
        }
    }

    /// <summary>
    /// Disables Welcome Panel, unlocks the first Station and unblocks Hints.
    /// </summary>
    public void OnStartClick()
    {
        startButton.SetActive(false);
        welcomePanel.SetActive(false);
        currentHintText.SetActive(true);
        scoreText.SetActive(true);
        infoText.gameObject.SetActive(true);
        stationManager.UnlockNextStation();
        blockHints = false;
        blockInfo = false;
        infoActive = false;
    }

    /// <summary>
    /// Toggles the needed UI, enables the OnDetection()-function.
    /// </summary>
    public void OnStationDetected()
    {
        welcomePanel.SetActive(false);
        hintsPanel.SetActive(false);
        infoActive = false;
        hintsActive = false;
        stationManager.OnDetection();
    }

    /// <summary>
    /// Enable Station Complete Panel and increase score.
    /// </summary>
    public void OnStationComplete()
    {
        BlockUIToggle();
        currentHintText.SetActive(false);
        stationManager.OnAudioFinished();
        stationCompletePanel.SetActive(true);
        score++;
        text_scoreText.text = score + " / 8";
    }

    /// <summary>
    /// Disables Station Complete Panel and unlocks next Station.
    /// </summary>
    public void OnContinueClick()
    {
        stationCompletePanel.SetActive(false);
        currentHintText.SetActive(true);
        stationManager.UnlockNextStation();
        infoText.text = "Finde die nächste Station!";
        BlockUIToggle();
    }

    /// <summary>
    /// Enables the final UI, and disables unneeded UI.
    /// </summary>
    public void AllStationsFinished()
    {
        BlockUIToggle();
        infoText.gameObject.SetActive(false);
        allCompletePanel.SetActive(true);
        currentHintText.SetActive(false);
    }

    /// <summary>
    /// Allows the user to download the final Image to the phones Gallery.
    /// </summary>
    public void DownloadImage()
    {
        NativeGallery.SaveImageToGallery(imageToDownload, "PARKS", "Schnitzeljagd_IMG.png");
        downloadButton.SetActive(false);
        downloadSuccessText.SetActive(true);
    }

    /// <summary>
    /// Resets all Components to default-state.
    /// </summary>
    public void RestartGame()
    {
        infoText.text = "Finde die erste Station!";
        downloadButton.SetActive(true);
        downloadSuccessText.SetActive(false);
        allCompletePanel.SetActive(false);
        startButton.SetActive(true);
        welcomePanel.SetActive(true);
        scoreText.SetActive(false);
        score = 0;
        text_scoreText.text = score + " / 8";
        infoActive = true;
        blockHints = true;
        blockInfo = true;
        blockUI = false;
        stationManager.ResetGame();
    }
}
