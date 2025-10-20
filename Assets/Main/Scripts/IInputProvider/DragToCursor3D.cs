using UnityEngine;
using UnityEngine.Serialization;

public class DragToCursor3D : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask cardLayer;
    [SerializeField] private bool useTouchInput = false;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeAmplitude;
    [SerializeField] private float shakeFrequency;
    [SerializeField] private float groundOffset = 0.5f;
    [SerializeField] private Store store;
    [FormerlySerializedAs("editorWindow")] [SerializeField] private  StoreView storeView;
    private IInputProvider inputProvider;
    private CameraShakeSystem cameraShake;
    private Vector3 originPos;
    public GameObject selectedObject { get; set; }
    private bool isDragging = false;
    private bool isPressed = false;
    private Vector2 pressPosition;
    [SerializeField] private float dragThreshold;

    void Start()
    {
        inputProvider = new PlatformInputProvider().GetPlatformInputProvider();
        
        cameraShake = FindObjectOfType<CameraShakeSystem>();
        
        if (mainCamera == null) mainCamera = Camera.main;
    }

    void Update()
    {
        if (inputProvider.IsReleased() && isPressed)
        {
            if (!isDragging && selectedObject != null)
            {
                OpenEditor(selectedObject.transform);
            }
            else if(isDragging && selectedObject != null)
            {
                Put(); 
            }
            
            isPressed = false;
            selectedObject = null;
            isDragging = false;
            return;
        }

        Vector2 screenPos = inputProvider.GetInputPosition();
        Ray ray = mainCamera.ScreenPointToRay(screenPos);

        Take(ray);
        Drag(ray);
    }

    private void Drag(Ray ray)
    {
        if (isPressed && selectedObject != null)
        {
            float distance = Vector2.Distance(inputProvider.GetInputPosition(), pressPosition);
            if (distance > dragThreshold)
            {
                isDragging = true;
                Vector3 targetPos = Vector3.zero;

                if (Physics.Raycast(ray, out RaycastHit hitPoint))
                {
                    targetPos = hitPoint.point;
                }
            
                targetPos.y = originPos.y + groundOffset;
                selectedObject.transform.position = Vector3.Lerp(selectedObject.transform.position, targetPos, Time.deltaTime * moveSpeed);
            }
        }
    }

    private void Take(Ray ray)
    {
        if(selectedObject) return;
        if (!inputProvider.IsPressed()) return;
        
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, cardLayer))
        {
            pressPosition = inputProvider.GetInputPosition();
            selectedObject = hit.collider.gameObject;
            originPos = selectedObject.transform.position;
            
            isPressed = true;
            isDragging = false;
        }
    }

    private void Put()
    {
        Vector3 putPos = selectedObject.transform.position;
        putPos.y = originPos.y;
        selectedObject.transform.position = putPos;
   
        cameraShake.ShakeCamera(shakeDuration, shakeAmplitude, shakeFrequency);
    }
    
    void OpenEditor(Transform mapObject)
    {
        store.SelectCard(selectedObject.GetComponent<Card>());
        storeView?.gameObject.SetActive(true);
        // if (editorTitle != null)
        //     editorTitle.text = $"Редактирование: {mapObject.name}";
    }

    public void CloseEditor()
    {
        storeView?.gameObject.SetActive(false);
    }
}