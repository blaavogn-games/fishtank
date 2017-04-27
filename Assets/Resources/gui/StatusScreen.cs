using UnityEngine;
using System.Collections;

public class StatusScreen : MonoBehaviour {
    private float alpha = 0;
    private const int DrawDepth = -1000;
    private Texture2D fadeTexture;
    private Rect rect;
    private bool fadeout;
    public float FadeSpeed = 3;
    public Color BackgroundColour = Color.black;
    public Color TextColour = Color.white;
    private bool canContinue = false;
    private int pills = 0;
    private int showPills = 0;
    private int maxPills = 0;
    private GUIStyle labelStyle;

    private int deaths = 0;
    private int totalDeaths = 0;
    private int showDeaths = 0;

    private float timeTaken = 0;
    private float showTimeTaken = 0;

    // Use this for initialization
    private void Start ()
    {
        fadeTexture = new Texture2D(1, 1);
        rect = new Rect(0, 0, Screen.width, Screen.height);
    }
	
	// Update is called once per frame
    private void Update () {
        if (!canContinue || !Input.anyKeyDown) return;
        Destroy(gameObject);
        World.i.NextLevel();
    }
    public void WinScreen(int deaths, int pills, int maxPills, float timeTaken, int totalDeaths)
    {
        Debug.Log("Deaths: " + deaths + "\npills: " + pills + "/" + maxPills);
        Debug.Log("Time Taken: " + timeTaken);
        this.pills = pills;
        this.maxPills = maxPills;
        this.deaths = deaths;
        this.timeTaken = timeTaken;
        this.totalDeaths = totalDeaths;
        rect = new Rect(0, 0, Screen.width, Screen.height);
        fadeout = true;
        canContinue = false;
    }
    private void OnGUI()
    {
        if (!fadeout) return;

        BackgroundColour.a = alpha;
        GUI.color = BackgroundColour;
        GUI.depth = DrawDepth;

        GUI.DrawTexture(rect, fadeTexture);
        GUI.DrawTexture(rect, fadeTexture);
        alpha += (Time.deltaTime/FadeSpeed);

        if (!(alpha > 1)) return;

        labelStyle = GUI.skin.GetStyle("Label");
        labelStyle.alignment = TextAnchor.UpperLeft;
        alpha = 1;
        GUI.color = TextColour;
        GUI.Label(new Rect(0.02f * Screen.width, 0.02f * Screen.height, 0.25f * Screen.width, 0.05f * Screen.height), "Pills eaten: " + pills + " / " + maxPills, labelStyle);
        GUI.Label(new Rect(0.02f * Screen.width, 0.07f * Screen.height, 0.25f * Screen.width, 0.1f * Screen.height), "Level Deaths: " + deaths, labelStyle);
        GUI.Label(new Rect(0.02f * Screen.width, 0.09f * Screen.height, 0.25f * Screen.width, 0.12f * Screen.height), "Total Deaths: " + totalDeaths, labelStyle);
        GUI.Label(new Rect(0.02f * Screen.width, 0.14f * Screen.height, 0.25f * Screen.width, 0.17f * Screen.height), "Time Taken: " + timeTaken.ToString("F2") + " seconds", labelStyle);
        canContinue = true;
        labelStyle.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "PRESS ANY KEY TO CONTINUE", labelStyle);
    }
}
