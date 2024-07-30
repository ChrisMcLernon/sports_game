using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TitleAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text tmpText;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float randomOffset;
    public float bounceSpeed = 1.0f; // Speed of bouncing
    public float bounceHeight = 1.0f; // Height of bouncing
    public float rotationSpeed = 30.0f; // Speed of rotation
    private Color32[] originalColors;
    private bool isHovering = false;
    public float colorChangeSpeed = 1.0f;

    void Start()
    {
        tmpText = GetComponent<TMP_Text>();
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;

        StoreOriginalColors();

        // Generate random offset to differentiate the animations of multiple text elements
        randomOffset = Random.Range(0f, 2 * Mathf.PI);
    }

    void Update()
    {
        // Calculate the bounce effect
        float newY = initialPosition.y + Mathf.Sin(Time.time * bounceSpeed + randomOffset) * bounceHeight;
        transform.localPosition = new Vector3(initialPosition.x, newY, initialPosition.z);

        // Calculate the gentle rotation effect
        float newRotationZ = Mathf.Sin(Time.time * rotationSpeed + randomOffset) * rotationSpeed;
        transform.localRotation = initialRotation * Quaternion.Euler(0, 0, newRotationZ);

        if (isHovering)
        {
            ApplyRotatingRainbowColors();
        }
    }

 public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        RestoreOriginalColors();
    }

    private void StoreOriginalColors()
    {
        TMP_TextInfo textInfo = tmpText.textInfo;
        originalColors = new Color32[textInfo.characterCount];
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (textInfo.characterInfo[i].isVisible)
            {
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                originalColors[i] = tmpText.textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].colors32[vertexIndex];
            }
        }
    }

    private void ApplyRotatingRainbowColors()
    {
        TMP_TextInfo textInfo = tmpText.textInfo;
        tmpText.ForceMeshUpdate();
        Color32[] newVertexColors;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (textInfo.characterInfo[i].isVisible)
            {
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                newVertexColors = tmpText.textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].colors32;

                Color32 rainbowColor = GetRainbowColor(i);
                newVertexColors[vertexIndex + 0] = rainbowColor;
                newVertexColors[vertexIndex + 1] = rainbowColor;
                newVertexColors[vertexIndex + 2] = rainbowColor;
                newVertexColors[vertexIndex + 3] = rainbowColor;
            }
        }
        tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    private Color32 GetRainbowColor(int index)
    {
        float t = (Time.time * colorChangeSpeed + index) % 1.0f;
        return Color.HSVToRGB(t, 1, 1);
    }

    private void RestoreOriginalColors()
    {
        TMP_TextInfo textInfo = tmpText.textInfo;
        tmpText.ForceMeshUpdate();
        Color32[] newVertexColors;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (textInfo.characterInfo[i].isVisible)
            {
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                newVertexColors = tmpText.textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].colors32;

                newVertexColors[vertexIndex + 0] = originalColors[i];
                newVertexColors[vertexIndex + 1] = originalColors[i];
                newVertexColors[vertexIndex + 2] = originalColors[i];
                newVertexColors[vertexIndex + 3] = originalColors[i];
            }
        }
        tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
    
}
