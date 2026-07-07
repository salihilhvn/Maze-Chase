using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class FloatingJoystick : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [InputControl(layout = "Vector2")]
    [SerializeField] private string m_ControlPath;

    protected override string controlPathInternal
    {
        get => m_ControlPath;
        set => m_ControlPath = value;
    }

    [Header("UI References")]
    public RectTransform background; // Görünür olan joystick dış çemberi
    public RectTransform knob;       // İçteki hareketli top

    [Header("Settings")]
    public float movementRange = 100f;

    private void Start()
    {
        // Başlangıçta görsel kısmı gizle
        if(background != null)
            background.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (background == null) return;

        background.gameObject.SetActive(true);
        
        // Tıklanan yeri bu (tam ekran) panelin koordinatlarına çevir
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            GetComponent<RectTransform>(), 
            eventData.position, 
            eventData.pressEventCamera, 
            out Vector2 localPoint
        );
        
        background.anchoredPosition = localPoint;
        knob.anchoredPosition = Vector2.zero;
        
        SendValueToControl(Vector2.zero);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (background == null) return;

        // Sürükleme pozisyonunu arka plana göre hesapla
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background, 
            eventData.position, 
            eventData.pressEventCamera, 
            out Vector2 localPoint
        );
        
        localPoint = Vector2.ClampMagnitude(localPoint, movementRange);
        knob.anchoredPosition = localPoint;

        // X ve Y değerlerini normalize edip Input System'e gönder
        Vector2 normalizedPos = localPoint / movementRange;
        SendValueToControl(normalizedPos);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (background == null) return;

        background.gameObject.SetActive(false);
        knob.anchoredPosition = Vector2.zero;
        
        SendValueToControl(Vector2.zero);
    }
}
