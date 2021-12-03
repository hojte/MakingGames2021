using System;
using UnityEngine;

public class CompassElement : MonoBehaviour
{
    [Tooltip("The marker on the compass for this element")]
    public CompassMarker compassMarkerPrefab;
    [Tooltip("Text override for the marker, if it's a direction")]
    public string textDirection;

    private bool didRegister;
    private CompassMarker markerInstance;

    Compass m_Compass;

    void Start()
    {
        m_Compass = FindObjectOfType<Compass>();

        markerInstance = Instantiate(compassMarkerPrefab);

        markerInstance.Initialize(this, textDirection);
        /*if (m_Compass != null)
        {
            m_Compass.RegisterCompassElement(transform, markerInstance);
            didRegister = true;
        }*/
    }

    private void Update()
    {
        if (m_Compass == null) m_Compass = FindObjectOfType<Compass>();
        else if (!didRegister)
        {
            m_Compass.RegisterCompassElement(transform, markerInstance);
            didRegister = true;
        }
    }

    void OnDestroy()
    {
        m_Compass.UnregisterCompassElement(transform);
    }

    public void UnregisterFromCompass()
    {
        m_Compass.UnregisterCompassElement(transform);
    }

    public void RegisterFromCompass()
    {
        var markerInstance = Instantiate(compassMarkerPrefab);

        markerInstance.Initialize(this, textDirection);
        m_Compass.RegisterCompassElement(transform, markerInstance);
    }
}
