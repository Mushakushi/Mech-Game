using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 


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
    /// Disables all layers except first layer in layers
    /// </summary>
    /// <remarks>All menus should be enabled by default for now</remarks>
    public void Awake()
    {
        if (layers.Count > 0)
        {
            foreach (MenuLayer layer in layers) ToggleActive(layer.parent);
            ToggleActive(layers[0].parent);
        }
    }

    private void Start()
    {
        AudioPlayer.PlayBGM(bgm);
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
        BattleGroupManager.LoadLevelData(bossName); 
        LoadScene("Battle Scene"); 
    }
}
