using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TranslatableTextComponent : MonoBehaviour
{
    private TextMeshProUGUI textGUI;
    [SerializeField] private TranslatableTextAsset translatableText;

    // Start is called before the first frame update
    private void Start()
    {
        if (GetComponent<TextMeshProUGUI>() is TextMeshProUGUI textGUI)
        {
            this.textGUI = textGUI;
            textGUI.text = translatableText.GetTranslationIn(TranslatableTextManager.GetGameLang());
            this.Register();
        }
        else
            throw new System.Exception("No TextMeshProUGUI component found!");
    }

    private void OnDestroy()
    {
        this.Unregister();
    }

    public void UpdateTextToNewLang(LANGUAGE newGameLang)
    {
        if (textGUI != null)
            textGUI.text = translatableText.GetTranslationIn(newGameLang);
        else
            throw new System.Exception("Missing TextGUI Component!");
    }
}
