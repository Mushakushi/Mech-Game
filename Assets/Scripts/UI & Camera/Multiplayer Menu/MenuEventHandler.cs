using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Linq;

public class MenuEventHandler : MonoBehaviour
{
    /// <summary>
    /// Represents one layer of a menu 
    /// </summary>
    /// <remarks>(e.g. options)</remarks>
    [System.Serializable]
    private struct MenuLayer
    {
        /// <summary>
        /// Name of this layer
        /// </summary>
        /// <remarks>Used for searching</remarks>
        public string name; 

        /// <summary>
        /// Parent GameObject of this layer 
        /// </summary>
        public GameObject parent; 

        /// <summary>
        /// First selected GameObject (button) in layer 
        /// </summary>
        public GameObject firstSelected; 
    }

    /// <summary>
    /// Layers in menu
    /// </summary>
    [SerializeField] private List<MenuLayer> layers;

    /// <summary>
    /// Active layer index, default 0
    /// </summary>
    [SerializeField] [ReadOnly] private int activeLayer; 

    /// <summary>
    /// Previous layer index 
    /// </summary>
    [SerializeField] [ReadOnly] private int previousLayer;

    /// <summary>
    /// Audio to play
    /// </summary>
    [SerializeField] private BGM bgm;

    /// <summary>
    /// Text to display loading progress
    /// </summary>
    [SerializeField] private TMP_Text progressText;

    /// <summary>
    /// Text to display errors
    /// </summary>
    [SerializeField] private TMP_Text errorText;

    public void Awake()
    {
        // check text 
        if (progressText) progressText.gameObject.SetActive(false);
        if (progressText) errorText.gameObject.SetActive(false);

        // Disables all layers except first layer in layers
        if (layers.Count > 0)
        {
            foreach (MenuLayer layer in layers) SetActive(layer.parent, false);
            SetActive(layers[0].parent, true); // could also toggle active, but doesn't matter..
        }
    }

    private void Start()
    {
        GlobalSettings.EnforceJoiningState();
        if (bgm.intro && bgm.loop) AudioPlayer.PlayBGM(bgm);
    }

    /// <summary>
    /// Opens layer with name <paramref name="name"/>
    /// </summary>
    /// <param name="name">Name of the layer to open</param>
    public void OpenLayer(string name)
    {
        // disable currently (soon to be previously) active layer
        ToggleActive(layers[activeLayer].parent); 

        // find target layer and activate it 
        for (int i = 0; i < layers.Count; i++)
        {
            if (layers[i].name == name)
            {
                SetActiveLayer(i); 
                return; 
            }  
        }

        Debug.LogError($"Layer '{name}' could not be found!"); 
    }

    /// <summary>
    /// Opens layer at index <paramref name="index"/>
    /// </summary>
    /// <param name="index">Name of the layer to open</param>
    public void OpenLayer(int index)
    {
        // disable currently (soon to be previously) active layer
        ToggleActive(layers[activeLayer].parent);

        // open player
        if (index < 0 || index > layers.Count) Debug.LogError($"Layer at index {index} could not be found!"); 
        else SetActiveLayer(index);
    }

    /// <summary>
    /// Opens previous layer, closes current layer
    /// </summary>
    public void CloseLayer() => OpenLayer(previousLayer); 

    /// <summary>
    /// Sets MenuLayer at index <paramref name="index"/> to active
    /// </summary>
    /// <param name="index">Index of MenuLayer layer in layers</param>
    private void SetActiveLayer(int index)
    {
        // record index of last active layer 
        previousLayer = activeLayer;
        activeLayer = index;

        // enable (now) currently active layer
        ToggleActive(layers[activeLayer].parent);

        // event system quirk, has to be set to null before reassignment
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(layers[index].firstSelected);
    }

    /// <summary>
    /// Sets gameObject parent <paramref name="parent"/> and all children's activeSelf to <paramref name="value"/>
    /// </summary>
    /// <param name="parent">Parent of children</param>
    private void SetActive(GameObject parent, bool value)
    {
        parent.SetActive(value);
        foreach (RectTransform child in parent.GetComponent<RectTransform>())
            child.gameObject.SetActive(value);
    }

    /// <summary>
    /// Toggles gameObject parent <paramref name="parent"/> and all children's activeSelf
    /// </summary>
    /// <param name="parent">Parent of children</param>
    private void ToggleActive(GameObject parent)
    {
        parent.SetActive(!parent.activeSelf); 
            foreach (RectTransform child in parent.GetComponent<RectTransform>())
                child.gameObject.SetActive(!child.gameObject.activeSelf); 
    }

    /// <summary>
    /// Loads scene with name <paramref name="scene"/>
    /// </summary>
    /// <param name="scene">The name of the scene</param>
    public void LoadScene(string scene) => StartCoroutine(Scene.Load(scene)); 

    /// <summary>
    /// Load battle scene with boss name <paramref name="name"/>
    /// </summary>
    /// <param name="bossName">Name of the boss to load</param>
    public void LoadBattleScene(string bossName)
    {
        if (!progressText) return;
        if (GlobalSettings.isMultiplayerGame && PlayerInputManager.instance.playerCount <= 1)
        {
            ShowError("CONNECT PLAYER 2!");
            return; 
        }
        BattleGroupManager.LoadLevelData(bossName);
        progressText.gameObject.SetActive(true);
        StartCoroutine(Scene.Load("Battle Scene", (x) => progressText.text = $"Loading...({x*100}%)")); 
    }

    /// <summary>
    /// Displays error message <paramref name="message"/> as tmp text
    /// </summary>
    /// <param name="message">Text to display</param>
    private void ShowError(string message)
    {
        if (errorText)
        {
            errorText.text = message;
            errorText.gameObject.SetActive(true);
            StartCoroutine(CoroutineUtility.WaitForSeconds(0.5f, () => errorText.gameObject.SetActive(false)));
        }
        else Debug.LogError("Error text is not assigned! Please assign it in the editor.");
    }

    /// <summary>
    /// Enables multiplyaer if <paramref name="isMultiplayerGame"/> is true, disables otherwise
    /// </summary>
    /// <param name="isMultiplayerGame">Whether or not to disable joining</param>
    /// <remarks>Set this before the battle scene, BattleGroupManager will read this on initiate</remarks>
    public void SetMultiplayerState(bool isMultiplayerGame) => GlobalSettings.isMultiplayerGame = isMultiplayerGame;

    /// <summary>
    /// Enables one-hit mode if <paramref name="isOneHitMode"/> is true, disables otherwise
    /// </summary>
    /// <param name="isOneHitMode">Whether or not to disable one-hit mode</param>
    public void SetOneHitMode(bool isOneHitMode) => GlobalSettings.SetOneHitMode(isOneHitMode);
}
