using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]private GameObject downArrow, upArrow;
    [SerializeField]private Vector2 minPos;
    [SerializeField]private Vector2 maxPos;

    [SerializeField]private float mouseThreshold;
    
    [SerializeField]private float camSpeed;

    public bool camIsMoving = false;

    Vector3 clampPos;

    private Camera cam;

    public static CameraManager i;

    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (CustomMouse.i == null) return;
        //if(ToolManager.i.UsingHand || ToolManager.i.UsingTool) return;
        Vector3 mousePos = cam.ScreenToWorldPoint(CustomMouse.i.mousePosition);
        if(Mathf.Abs(cam.transform.position.y - mousePos.y) <= mouseThreshold)
        {
            clampPos = cam.transform.position;
            camIsMoving = false;
            return;
        }else{
            camIsMoving = true;
        }
        

        clampPos = Vector2.MoveTowards(cam.transform.position, mousePos, camSpeed * Time.deltaTime);
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        //clampPos.x = Mathf.Clamp(clampPos.x, minPos.x + width / 2f, maxPos.x - width / 2f);
        clampPos.x = cam.transform.position.x;
        clampPos.y = Mathf.Clamp(clampPos.y, minPos.y + height / 2f, maxPos.y - height / 2f);
        //clampPos.y = cam.transform.position.y;
        clampPos.z = -10f;

        downArrow.SetActive(true);
        upArrow.SetActive(true);

        if(Mathf.Abs(cam.transform.position.y - (minPos.y + height / 3f)) < 4f)
        {
            downArrow.SetActive(false);
        }
        if(Mathf.Abs(cam.transform.position.y - (maxPos.y - height / 2f)) < 4f)
        {
            upArrow.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        //cam.transform.position = Vector2.MoveTowards(cam.transform.position, cam.ScreenToWorldPoint(Input.mousePosition), camSpeed * Time.deltaTime);
        
        cam.transform.position = clampPos;
    }

    public void BlinkArrow()
    {
        StartCoroutine(EBlinkArrow());
    }

    public IEnumerator EBlinkArrow()
    {
        float blinkTime = 60f;
        SpriteRenderer UpRenderer = upArrow.GetComponent<SpriteRenderer>();
        SpriteRenderer DownRenderer = downArrow.GetComponent<SpriteRenderer>();
        while (blinkTime > 0f)
        {
            blinkTime -= Time.deltaTime * 2f;
            
            UpRenderer.enabled = ((int)blinkTime % 2 == 0);
            DownRenderer.enabled = ((int)blinkTime % 2 == 0);
            

            yield return null;
        }

        UpRenderer.enabled = true;
        DownRenderer.enabled = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Camera.main.gameObject.transform.position, mouseThreshold);
        Gizmos.color = Color.yellow;
        Vector2 downLeft = new Vector2(minPos.x, minPos.y);
        Vector2 downRight = new Vector2(maxPos.x, minPos.y);
        Vector2 upLeft = new Vector2(minPos.x, maxPos.y);
        Vector2 upRight = new Vector2(maxPos.x, maxPos.y);

        Gizmos.DrawLine(downLeft, downRight);
        Gizmos.DrawLine(downRight, upRight);
        Gizmos.DrawLine(upRight, upLeft);
        Gizmos.DrawLine(downLeft, upLeft);
    }
}
