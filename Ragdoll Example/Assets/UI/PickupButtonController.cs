using Interactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickupButtonController : MonoBehaviour
{
    private TextMeshProUGUI _textMeshProUGUI;
    public Pickup pickup;
    public bool isQuickSelected;
    void Awake()
    {
        _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        if (pickup == null) Destroy(gameObject);
        _textMeshProUGUI.text = pickup.pickupType+":\n"+pickup.timeLeft.ToString().Split('.')[0];

        var image = GetComponent<Image>();
        if (pickup.timeOfActivation!=0) image.color = new Color(0f, 1f, 0f, 0.4f);
        else if (isQuickSelected) image.color = new Color(0f, 0.68f, 1f, 0.4f);
        else image.color = new Color(1,1,1,0.4f);
    }
}
