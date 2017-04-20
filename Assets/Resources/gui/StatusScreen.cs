using UnityEngine;
using System.Collections;

public class StatusScreen : MonoBehaviour {
    float alpha = 0;
    int drawDepth = -1000;
    Texture2D fadeTexture;
    Rect rect;
    bool fadeout;
    public float fadeSpeed = 3;
    public Color BackgroundColour = Color.black;
    public Color TextColour = Color.white;
    bool canContinue = false;
    int pills = 0;
    int showPills = 0;
    int maxPills = 0;
    GUIStyle labelStyle;

    int deaths = 0;
    int showDeaths = 0;

    float timeTaken = 0;
    float showTimeTaken = 0;

    // Use this for initialization
    void Start ()
    {
        fadeTexture = new Texture2D(1, 1);
        rect = new Rect(0, 0, Screen.width, Screen.height);
    }
	
	// Update is called once per frame
	void Update () {
        if (canContinue)
        {
            if (Input.anyKey)
            {
                Destroy(gameObject);
                World.i.NextLevel();
            }
        }
	}
    public void WinScreen(int deaths, int pills, int maxPills, float timeTaken)
    {
        Debug.Log("Deaths: " + deaths + "\npills: " + pills + "/" + maxPills);
        Debug.Log("Time Taken: " + timeTaken);
        this.pills = pills;
        this.maxPills = maxPills;
        this.deaths = deaths;
        this.timeTaken = timeTaken;
        rect = new Rect(0, 0, Screen.width, Screen.height);
        fadeout = true;
        canContinue = false;
    }
    private void OnGUI()
    {
        if (fadeout)
        {

            BackgroundColour.a = alpha;
            GUI.color = BackgroundColour;
            GUI.depth = drawDepth;

            GUI.DrawTexture(rect, fadeTexture);
            GUI.DrawTexture(rect, fadeTexture);
            GUI.DrawTexture(rect, fadeTexture);
            GUI.DrawTexture(rect, fadeTexture);
            alpha += (Time.deltaTime/fadeSpeed);
            if (alpha > 1)
            {
                labelStyle = GUI.skin.GetStyle("Label");
                labelStyle.alignment = TextAnchor.UpperLeft;
                alpha = 1;
                GUI.color = TextColour;
                GUI.Label(new Rect(0.02f * Screen.width, 0.02f * Screen.height, 0.25f * Screen.width, 0.05f * Screen.height), "Pills eaten: " + showPills + " / " + maxPills, labelStyle);
                if (showPills < pills)
                {
                    showPills += 1;
                }
                if (showPills >= pills)
                {
                    if (showDeaths < deaths)
                    {
                        deaths += 1;
                    }
                    GUI.Label(new Rect(0.02f * Screen.width, 0.07f * Screen.height, 0.25f * Screen.width, 0.1f * Screen.height), "Deaths: " + showDeaths, labelStyle);
                    if (showDeaths >= deaths)
                    {
                        if (showTimeTaken < timeTaken)
                        {
                            showTimeTaken += timeTaken / 20;
                        }
                        GUI.Label(new Rect(0.02f * Screen.width, 0.12f * Screen.height, 0.25f * Screen.width, 0.15f * Screen.height), "Time Taken: " + showTimeTaken.ToString("F2") + " seconds", labelStyle);
                        if(showTimeTaken >= timeTaken)
                        {
                            canContinue = true;
                            labelStyle.alignment = TextAnchor.MiddleCenter;
                            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "PRESS ANY KEY TO CONTINUE", labelStyle);
                        }
                    }
                }

            }
        }
    }
}
