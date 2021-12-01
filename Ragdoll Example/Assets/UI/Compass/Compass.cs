using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public int makerCount;
    
    public RectTransform compasRect;
    public float visibilityAngle = 180f;
    public float heightDifferenceMultiplier = 2f;
    public float minScale = 0.5f;
    public float distanceMinScale = 50f;
    public float compasMarginRatio = 0.8f;

    public GameObject MarkerDirectionPrefab;

    Transform virtualCameraTransform;
    Dictionary<Transform, CompassMarker> m_ElementsDictionnary = new Dictionary<Transform, CompassMarker>();

    float m_WidthMultiplier;
    float m_heightOffset;

    void Awake()
    {
        CinemachineVirtualCamera virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        virtualCameraTransform = virtualCamera.transform;

        m_WidthMultiplier = compasRect.rect.width / visibilityAngle;
        m_heightOffset = -compasRect.rect.height / 2;
    }

    void Update()
    {
        makerCount = m_ElementsDictionnary.Count;
        foreach (var element in m_ElementsDictionnary)
        {
            float distanceRatio = 1;
            float heightDifference = 0;
            float angle;
            if (element.Key == null)
            {
                UnregisterCompassElement(element.Key);
                print("force unregistered " + element.Key);
                return;
            }
            if(virtualCameraTransform == null) virtualCameraTransform = FindObjectOfType<CinemachineVirtualCamera>().transform;
            if (element.Value.isDirection)
            {
                angle = Vector3.SignedAngle(virtualCameraTransform.forward, element.Key.transform.localPosition.normalized, Vector3.up);
            }
            else
            {
                var cameraPosition = virtualCameraTransform.position;
                Vector3 targetDir = (element.Key.transform.position - cameraPosition).normalized;

                targetDir = Vector3.ProjectOnPlane(targetDir, Vector3.up);
                Vector3 playerForward = Vector3.ProjectOnPlane(virtualCameraTransform.forward, Vector3.up);
                angle = Vector3.SignedAngle(playerForward, targetDir, Vector3.up);

                Vector3 adjustedCameraPos = new Vector3(cameraPosition.x, cameraPosition.y - 6, cameraPosition.z);
                Vector3 directionVector = element.Key.transform.position - adjustedCameraPos;

                heightDifference = (directionVector.y) * heightDifferenceMultiplier;
                heightDifference = Mathf.Clamp(heightDifference, -compasRect.rect.height / 2 * compasMarginRatio, compasRect.rect.height / 2 * compasMarginRatio);

                distanceRatio = directionVector.magnitude / distanceMinScale;
                distanceRatio = Mathf.Clamp01(distanceRatio);
            }

            if (angle > -visibilityAngle / 2 && angle < visibilityAngle / 2)
            {
                element.Value.canvasGroup.alpha = 0.8f;
                element.Value.canvasGroup.transform.localPosition = new Vector2(m_WidthMultiplier * angle, heightDifference + m_heightOffset);
                element.Value.canvasGroup.transform.localScale = Vector3.one * Mathf.Lerp(1, minScale, distanceRatio);
            }
            else
            {
                element.Value.canvasGroup.alpha = 0;
            }
        }
    }

    public void ResetList(CinemachineVirtualCamera cinemachineVirtualCamera)
    {
        // var tmp = new Dictionary<Transform, CompassMarker> (m_ElementsDictionnary);
        // foreach (var keyValuePair in tmp)
        // {
        //     if(!keyValuePair.Value.isDirection)
        //         UnregisterCompassElement(keyValuePair.Key);
        // }
        virtualCameraTransform = cinemachineVirtualCamera.transform;
    }

    public void RegisterCompassElement(Transform element, CompassMarker marker)
    {
        if (m_ElementsDictionnary.ContainsKey(element))
        {
            print("Already added to elements: "+element);
            return;
        }
        m_ElementsDictionnary.Add(element, marker);
        print("added: " + element);
        marker.transform.SetParent(compasRect);
    }

    public void UnregisterCompassElement(Transform element)
    {
        print("remove: "+element);
        if (m_ElementsDictionnary.TryGetValue(element, out CompassMarker marker) && marker.canvasGroup != null)
            Destroy(marker.canvasGroup.gameObject);
        m_ElementsDictionnary.Remove(element);
    }
}
