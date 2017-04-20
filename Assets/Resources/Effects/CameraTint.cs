using UnityEngine;
using System.Collections;

public class CameraTint : MonoBehaviour
{
    public Color color = Color.red;
    public float MaxAlpha = 0.4f;
    public float WarningDistance = 40;
    public float BlinkSpeedFar = 1.0f;
    public float BlinkSpeedNear = 0.01f;

    private float blinkSpeed = 0;
    
    private float blinkTimer = 0;
    private float fadeDirection = 1;

    [Header("Current blink stats")]
    public float currentAlpha;
    public float currentBlinkSpeed;

    float alphaTimer = 0;
    
    float alpha = 0;
    int drawDepth = -1000;
    Texture2D fadeTexture;
    Rect rect;
    float currentDistance = Mathf.Infinity;

    // Use this for initialization
    void Start()
    {
        fadeTexture = new Texture2D(1, 1);
        rect = new Rect(0, 0, Screen.width, Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        currentDistance = closestEnemyDistance();
        if (currentDistance < WarningDistance)
        {
            blinkSpeed = Mathf.Lerp(BlinkSpeedNear, BlinkSpeedFar, currentDistance / WarningDistance);
            alpha = Mathf.Lerp(0, MaxAlpha, blinkTimer / blinkSpeed);
        }
        else
        {
            blinkSpeed = BlinkSpeedFar;
            alpha = 0;
        }

        if (blinkTimer <= 0 || blinkTimer >= blinkSpeed)
        {
            blinkTimer = Mathf.Max(0, fadeDirection * blinkSpeed);
            fadeDirection *= -1;
        }
        blinkTimer += (fadeDirection * Time.deltaTime);

        currentAlpha = alpha;
        currentBlinkSpeed = blinkSpeed;
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
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        float distance = Mathf.Infinity;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return distance;

        Vector3 position = player.transform.position;
        foreach (GameObject go in gos)
        {
            float curDist = Vector3.Distance(go.transform.position, position);
            if (curDist < distance)
                distance = curDist;
        }
        return distance;
    }
}
