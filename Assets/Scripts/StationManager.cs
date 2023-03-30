using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StationManager : MonoBehaviour
{
    // Textfield of current Hint.
    [SerializeField]
    [Tooltip("Current Hint Textfield in Game Scene")]
    private TMP_Text currentHintText;

    // Textfield of Hint 1 in Hints Panel.
    [SerializeField]
    [Tooltip("Textfield of Hint 1 in Hints Panel")]
    private TMP_Text hint1PanelText;

    // Textfield Headline of Hint 2 in Hints Panel.
    [SerializeField]
    [Tooltip("Textfield Headline of Hint 2 in Hints Panel")]
    private TMP_Text hint2PanelHL;

    // Image of Hint 2 in Hints Panel.
    [SerializeField]
    [Tooltip("Image of Hint 2 in Hints Panel")]
    private Image hint2PanelImage;

    // Maximized Image of Hint 2 in Hints Panel.
    [SerializeField]
    [Tooltip("Maximized Image of Hint 2 in Hints Panel")]
    private Image hint2PanelImageMax;

    // Textfield Headline of Hint 3 in Hints Panel.
    [SerializeField]
    [Tooltip("Textfield Headline of Hint 3 in Hints Panel")]
    private TMP_Text hint3PanelHL;

    // Image of Hint 3 in Hints Panel.
    [SerializeField]
    [Tooltip("Image of Hint 3 in Hints Panel")]
    private Image hint3PanelImage;

    // Maximized Image of Hint 3 in Hints Panel.
    [SerializeField]
    [Tooltip("Maximized Image of Hint 3 in Hints Panel")]
    private Image hint3PanelImageMax;

    // Button to unlock next Hint in Hints Panel.
    [SerializeField]
    [Tooltip("Button to unlock next Hint in Hints Panel")]
    private Button nextHintButton;

    // Button to skip current Station in Hints Panel.
    [SerializeField]
    [Tooltip("Button to skip current Station in Hints Panel.")]
    private Button skipStationButton;

    // Image of Puzzle Piece in Station Complete Panel.
    [SerializeField]
    [Tooltip("Image of Puzzle Piece in Station Complete Panel.")]
    private Image puzzlePiecePanelImage;

    // Max Image of Puzzle Piece in Station Complete Panel.
    [SerializeField]
    [Tooltip("Max Image of Puzzle Piece in Station Complete Panel.")]
    private Image puzzlePiecePanelImageMax;

    // List of all Stations in the correct Order.
    [SerializeField]
    [Tooltip("List of all Stations in the correct Order")]
    private Station[] stationList;

    // Amount of Stations.
    private int stationLenght;

    // The current active Station.
    private Station currentStation;

    // The current active Station Index.
    private int currentStationIndex = 0;

    // The current active Hint Index.
    private int currentHintIndex = 1;

    // Hint 1 of current active Station.
    private string currentStationHint1;

    // Hint 2 Image Sprite of current active Station.
    private Sprite currentStationHint2Image;

    // Hint 3 Image Sprite of current active Station.
    private Sprite currentStationHint3Image;

    // Puzzle Piece Image of current active Station.
    private Sprite currentStationpuzzlePieceImage;

    private UIManager uiManager;

    void Awake()
    {
        stationLenght = stationList.Length;
        uiManager = FindObjectOfType<UIManager>();
    }

    /// <summary>
    /// Function to unlock the next Hint.
    /// </summary>
    public void OnNextHintUnlock()
    {
        if (currentHintIndex == 1)
        {
            hint2PanelHL.gameObject.SetActive(true);
            hint2PanelImage.gameObject.SetActive(true);
            currentHintIndex += 1;
        }
        else if (currentHintIndex == 2)
        {
            nextHintButton.gameObject.SetActive(false);
            hint3PanelHL.gameObject.SetActive(true);
            hint3PanelImage.gameObject.SetActive(true);
            skipStationButton.gameObject.SetActive(true);
            currentHintIndex += 1;
        }
    }
    
    /// <summary>
    /// Function to unlock the next Station and update all parameters and UI elements.
    /// </summary>
    public void UnlockNextStation()
    {
        // Disable active Station if not first Station.
        if (currentStationIndex != 0)
        {
            currentStation.gameObject.SetActive(false);
        }

        // Endscreen after last Station.
        if(currentStationIndex == stationLenght)
        {
            uiManager.AllStationsFinished();
            return;
        }

        // Hides the UI elements for the second hint and unhides the Button to unlock it.
        hint2PanelHL.gameObject.SetActive(false);
        hint2PanelImage.gameObject.SetActive(false);
        hint3PanelHL.gameObject.SetActive(false);
        hint3PanelImage.gameObject.SetActive(false);
        nextHintButton.gameObject.SetActive(true);
        skipStationButton.gameObject.SetActive(false);
        currentHintIndex = 1;

        // updates all parameters of the current Station.
        currentStation = stationList[currentStationIndex];
        currentStationHint1 = currentStation.hint1;
        currentStationHint2Image = currentStation.hint2Image;
        currentStationHint3Image = currentStation.hint3Image;
        currentStationpuzzlePieceImage = currentStation.puzzlePieceImage;

        // updates all UI elements for the current Station.
        currentHintText.text = currentStationHint1;
        hint1PanelText.text = currentStationHint1;
        hint2PanelImage.sprite = currentStationHint2Image;
        hint2PanelImageMax.sprite = currentStationHint2Image;
        hint3PanelImage.sprite = currentStationHint3Image;
        hint3PanelImageMax.sprite = currentStationHint3Image;
        puzzlePiecePanelImage.sprite = currentStationpuzzlePieceImage;
        puzzlePiecePanelImageMax.sprite = currentStationpuzzlePieceImage;

        // Enable current Station and increase Station Index.
        currentStation.gameObject.SetActive(true);
        currentStationIndex += 1;
    }

    /// <summary>
    /// Enables all unlockable hints when the current active Station is detected.
    /// </summary>
    public void OnDetection()
    {
        nextHintButton.gameObject.SetActive(false);
        skipStationButton.gameObject.SetActive(false);
        hint2PanelImageMax.gameObject.SetActive(false);
        hint3PanelImageMax.gameObject.SetActive(false);
        hint2PanelHL.gameObject.SetActive(true);
        hint2PanelImage.gameObject.SetActive(true);
        hint3PanelHL.gameObject.SetActive(true);
        hint3PanelImage.gameObject.SetActive(true);
    }

    /// <summary>
    /// Disables maximized Images when Audio is finished playing.
    /// </summary>
    public void OnAudioFinished()
    {
        hint2PanelImageMax.gameObject.SetActive(false);
        hint3PanelImageMax.gameObject.SetActive(false);
    }

    /// <summary>
    /// Enables the Speaker Object and Calls the function OnInteraction().
    /// </summary>
    public void SkipStation()
    {
        skipStationButton.gameObject.SetActive(false);
        ARInteractable currentSpeaker = currentStation.GetComponentInChildren<ARInteractable>(true);
        currentSpeaker.gameObject.SetActive(true);
        currentSpeaker.OnInteraction();
    }

    /// <summary>
    /// Resets the currentStationIndex.
    /// </summary>
    public void ResetGame()
    {
        currentStationIndex = 0;
    }
}
