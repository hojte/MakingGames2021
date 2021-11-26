using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickupButtonController : MonoBehaviour
{
    public Button button;
    private TextMeshProUGUI _textMeshProUGUI;
    public float timeLeft;
    public string pickupType;
    public bool isQuickSelected;
    private string _selectStr = "";
    void Awake()
    {
        button = GetComponent<Button>();
        _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        if (isQuickSelected) _selectStr = ">";
        else _selectStr = "";
        _textMeshProUGUI.text = _selectStr+pickupType+":\n"+timeLeft.ToString().Split('.')[0];
    }
}
