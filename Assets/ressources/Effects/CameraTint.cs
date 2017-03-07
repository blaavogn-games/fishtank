using UnityEngine;
using System.Collections;

public class CameraTint : MonoBehaviour
{
    public Color color = Color.red;
    public float MaxAlpha = 0.8f;
    public float WarningDistance = 60;
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
            alpha = MaxAlpha-(currentDistance / WarningDistance);
        }
    }

    private void OnGUI()
    {
        color.a = alpha;
        GUI.color = color;
        GUI.depth = drawDepth;

        GUI.DrawTexture(rect, fadeTexture);
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
