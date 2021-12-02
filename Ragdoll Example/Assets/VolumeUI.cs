using Sound;
using TMPro;
using UnityEngine;

public class VolumeUI : MonoBehaviour
{
    public TextMeshProUGUI _uiText;

    void Update()
    {
        _uiText.text = "- Vol: " +AudioUtility.masterAudioAmplify+ " +";
    }
}
