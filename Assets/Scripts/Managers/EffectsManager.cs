using UnityEngine;
using TMPro;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance;

    public GameObject textPrefab;

    public GameObject attackEffectPrefab;

    private void Awake()
    {
        Instance = this;
    }
    public static void DoFloatingText(Vector3 position, string text, Color c)
    {
        EffectsManager effectsManager = FindObjectOfType<EffectsManager>();
        GameObject floatingText = Instantiate(effectsManager.textPrefab, position, Quaternion.identity);
        floatingText.GetComponent<TMP_Text>().color = c;
        floatingText.GetComponent<DamagePopup>().displayText = text;
    }
}
