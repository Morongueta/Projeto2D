using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomMouse : MonoBehaviour
{
    [Header("Keyboard Mouse")]
    [SerializeField] private bool simulatedMouse;

    [SerializeField] private Canvas canvas;
    private Vector2 mousePositionSimulated;
    [SerializeField]private float mouseSpeed;

    [Header("UI Mouse")]
    [SerializeField] private Image UIMouse;

    [SerializeField] private Sprite defaultMouse;
    [SerializeField] private Sprite copyMouse;

    private Button lastButton;

    public bool pointingUI;
    [Header("World Mouse")]
    [SerializeField] private Sprite defaultHand;
    [SerializeField] private Sprite pointingHand;
    [SerializeField] private Sprite clickHand;
    [SerializeField] private Sprite eraserHand;
    [SerializeField] private Sprite penHand;
    [SerializeField] private LayerMask customUILayer;
    [SerializeField] private LayerMask UILayer;
    [SerializeField] private LayerMask curriculumUILayer;

    private WorldButton lastWorldButton;

    public bool pointing;


    private SpriteRenderer render;

    public Vector3 mousePosition;
    public static CustomMouse i;
    private void Awake()
    {
        i = this;
        render = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        Cursor.visible = false;

        mousePositionSimulated = canvas.renderingDisplaySize / 2f;
    }
    private void Update()
    {
        mousePosition = Input.mousePosition;

        if(simulatedMouse)
        {
            float xAxis = Input.GetAxisRaw("HORIZONTAL0");
            float yAxis = Input.GetAxisRaw("VERTICAL0");

            Vector2 moveAxis = new Vector2(xAxis,yAxis);

            moveAxis.Normalize();

            mousePositionSimulated += moveAxis * mouseSpeed * Time.deltaTime; 

            mousePositionSimulated.x = Mathf.Clamp(mousePositionSimulated.x, 0, canvas.renderingDisplaySize.x);
            mousePositionSimulated.y = Mathf.Clamp(mousePositionSimulated.y, 0, canvas.renderingDisplaySize.y);

            mousePosition = mousePositionSimulated;

            HandleMouseInteractions();
        }else{
            RaycastHit2D hitWorld = Physics2D.CircleCast(Camera.main.ScreenToWorldPoint(mousePosition), .05f, Vector2.zero, .15f, customUILayer);
            RaycastHit2D hitCurriculumUI = Physics2D.CircleCast(mousePosition, .1f, Vector2.zero, .15f, curriculumUILayer);
            pointing = hitWorld.collider != null;
            pointingUI = hitCurriculumUI.collider != null;
        }

        
        if(EventSystem.current.IsPointerOverGameObject())
        {
            UIMouse.enabled = true;
            render.enabled = false;
            DrawUIMouse(mousePosition);
        }else{
            UIMouse.enabled = false;
            render.enabled = true;
            DrawMouse(mousePosition);
        }

    }

    private void HandleMouseInteractions()
    {
        RaycastHit2D hitWorld = Physics2D.CircleCast(Camera.main.ScreenToWorldPoint(mousePosition), .05f, Vector2.zero, .15f, customUILayer);
        RaycastHit2D hitCurriculumUI = Physics2D.CircleCast(mousePosition, .1f, Vector2.zero, .15f, curriculumUILayer);
        RaycastHit2D hitUI = Physics2D.CircleCast(mousePosition, .1f, Vector2.zero, .15f, UILayer);
        pointing = hitWorld.collider != null;
        pointingUI = hitCurriculumUI.collider != null;

        WorldButton worldButton = null;

        Button button = null;

        if(pointing)
        {
            worldButton = hitWorld.collider.GetComponent<WorldButton>();

            if(worldButton != lastWorldButton && lastWorldButton != null) lastWorldButton.OnMouseExit();
            if(worldButton != lastWorldButton && worldButton != null)
            {
                worldButton.OnMouseEnter();
                lastWorldButton = worldButton;
            }
            if(worldButton != null && Input.GetButtonDown("VERDE0")) worldButton.OnMouseDown();
        }

        if(hitUI.collider != null)
        {
            button = hitUI.collider.GetComponent<Button>();

            if(button != null && Input.GetButtonDown("VERDE0")) button.onClick?.Invoke();
        }

    }


    private void DrawMouse(Vector3 mousePos)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(mousePos);
        pos.z = 0;
        transform.position = pos;

        Sprite handSprite = null;

        if(ToolManager.i == null)
        {
            Cursor.visible = false;
            render.sprite = defaultHand;
            return;
        }

        switch (ToolManager.i.GetTool())
        {
            case Tool.HAND:
                handSprite = (ToolManager.i.UsingHand) ? clickHand : defaultHand;
                break;
            case Tool.ERASER:
                handSprite = eraserHand;
                break;
            case Tool.PEN:
                handSprite = penHand;
                break;
        }
        if(pointing) handSprite = pointingHand;
        Cursor.visible = (handSprite == null);

        render.sprite = handSprite;
    }

    public void SetPenMouseSprite(Sprite spr)
    {
        penHand = spr;
    }

    private void DrawUIMouse(Vector3 mousePos)
    {
        Vector3 pos = mousePos;
        pos.z = 0;
        UIMouse.transform.position = pos;

        Sprite handSprite = defaultMouse;

        if(pointingUI) handSprite = copyMouse;

        Cursor.visible = (handSprite == null);

        UIMouse.sprite = handSprite;
    }

}
