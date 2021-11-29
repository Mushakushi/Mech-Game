using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using System.IO;

public class Boss : MonoBehaviour
{

    [Header("Dialogue")]
    [SerializeField] TextAsset file;
    protected string[] dialogue;
    [SerializeField] TextMeshProUGUI tmpro;
    [SerializeField] int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = DialogueManager.ParseText(file.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        tmpro.text = dialogue[i];
    }


}
