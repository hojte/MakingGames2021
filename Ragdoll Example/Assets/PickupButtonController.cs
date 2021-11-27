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
        _selectStr = isQuickSelected ? ">" : "";
        _textMeshProUGUI.text = _selectStr+pickupType+":\n"+timeLeft.ToString().Split('.')[0];
        
        if (isQuickSelected) GetComponent<Image>().color = new Color(0,0,1,0.4f);
    }
}
