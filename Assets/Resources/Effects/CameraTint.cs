using UnityEngine;
using System.Collections;

public class CameraTint : MonoBehaviour
{
    public Color color = Color.red;
    public float MaxAlpha = 0.4f;
    public float WarningDistance = 40;
    public float BlinkSpeedFar = 1.0f;
    public float BlinkSpeedNear = 0.01f;
    public float shakeFactor = 0.1f;
    [Header("Chase Cam Controls")]
    [Range(0,180)]
    public float ChaseCamAngle = 60;
    [Tooltip("If you want it to be less than warning distance, anything higher than that makes no difference")]
    public float ChaseCamDistance = 60;

    public float ChaseCamSnapSpeed = 0.8f;
    public float InstantSnapRadius = 7;
    private float originalSnapSpeed = 5;

    private float blinkSpeed = 0;
    
    private float blinkTimer = 0;
    private float fadeDirection = 1;
    public UnityStandardAssets.Cameras.AutoCam cam;
    private GameObject cameraTarget;
    private GameObject player;
    private GameObject[] enemies;
    private GameObject chasingEnemy;
    private Transform camTarget;

    float alphaTimer = 0;
    
    float alpha = 0;
    int drawDepth = -1000;
    Texture2D fadeTexture;
    Rect rect;
    float currentDistance = Mathf.Infinity;
    ScreenShake shake;
    bool camSet = false;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cameraTarget = player;
        shake = Camera.main.GetComponent<ScreenShake>();
        fadeTexture = new Texture2D(1, 1);
        rect = new Rect(0, 0, Screen.width, Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        currentDistance = closestEnemyDistance();
        if (currentDistance < WarningDistance && chasingEnemy != null)
        {
            blinkSpeed = Mathf.Lerp(BlinkSpeedNear, BlinkSpeedFar, currentDistance / WarningDistance);
            alpha = Mathf.Lerp(0, MaxAlpha, blinkTimer / blinkSpeed);
            shake.ShakeScreen(1, alpha * shakeFactor);
            if (Vector3.Angle(player.transform.forward, chasingEnemy.transform.forward) < ChaseCamAngle && currentDistance<ChaseCamDistance && camTarget != null)
            {
                if (!camSet)
                {
                    cam.SetSpeed(ChaseCamSnapSpeed);
                    camSet = true;
                    cam.SetTarget(camTarget);
                }
                if(Vector3.Distance(cam.transform.position,chasingEnemy.transform.position)<InstantSnapRadius || Vector3.Distance(player.transform.position, chasingEnemy.transform.position)< Vector3.Distance(player.transform.position,cam.transform.position))
                    cam.SetSpeed(originalSnapSpeed);
            }
            else
            {
                if (camSet)
                {
                    cam.SetSpeed(originalSnapSpeed);
                    cam.SetTarget(player.transform);
                    camSet = false;
                }
            }
        }
        else
        {
            if (camSet)
            {
                cam.SetSpeed(originalSnapSpeed);
                cam.SetTarget(player.transform);
                camSet = false;
            }
            chasingEnemy = null;
            shake.ShakeScreen(0, 0);
            blinkSpeed = BlinkSpeedFar;
            alpha = 0;

        }

        if (blinkTimer <= 0 || blinkTimer >= blinkSpeed)
        {
            blinkTimer = Mathf.Max(0, fadeDirection * blinkSpeed);
            fadeDirection *= -1;
        }
        blinkTimer += (fadeDirection * Time.deltaTime);
    }

    private void OnGUI()
    {
        if (alpha > 0)
        {
            color.a = alpha;
            GUI.color = color;
            GUI.depth = drawDepth;

            GUI.DrawTexture(rect, fadeTexture);
        }
    }
    private float closestEnemyDistance()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float distance = Mathf.Infinity;
        if (player == null)
            return distance;

        Vector3 position = player.transform.position;
        foreach (GameObject go in enemies)
        {
            if (go.GetComponent<Enemy>() != null)
            {
                float curDist = Vector3.Distance(go.transform.position, position);
                if (curDist < distance)
                {
                    if (go.GetComponent<Enemy>().state == Enemy.State.CHARGE || go.GetComponent<Enemy>().state == Enemy.State.INSIGHT)
                    {
                        distance = curDist;
                        chasingEnemy = go;
                        camTarget = chasingEnemy.GetComponent<Enemy>().CameraTarget;
                    }
                }
            }
        }
        return distance;
    }
}
