using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

public class PostLevelCanvas : MonoBehaviour
{

    private enum State { BEFORE, SAVEDTEXT, PILLS, DEATHS, TIME, DONE };
    public Text AquariumSavedText;
    public Color AquariumSavedColor = Color.white;
    public float AquariumSavedFadeTime = 5;
    [Header("Pills Text Controls")]
    public Image PillsBG;
    public Color PillsBGColor = Color.blue;
    public Text PillsText;
    public Color PillsTextColor = Color.white;
    [Header("Pills Text Controls")]
    public Image DeathsBG;
    public Color DeathsBGColor = Color.blue;
    public Text DeathsText;
    public Color DeathsTextColor = Color.white;
    [Header("Pills Text Controls")]
    public Image TimeBG;
    public Color TimeBGColor = Color.blue;
    public Text TimeText;
    public Color TimeTextColor = Color.white;
    [Header("General Controls")]
    public float BackgroundFadeTime = 1;
    public float TextCountSpeed = 3;

    private State state;
    private float bgTimer=0;
    private float textTimer=0;
    private bool active = false;
    private Dictionary<string, float> stats;
    private PostLevelScript postLevelScript;

	// Use this for initialization
	void Start () {
        state= State.BEFORE;
	}
	
	// Update is called once per frame
	void Update () {
	    switch (state)
	    {   
	            case State.SAVEDTEXT:
	                textTimer += Time.deltaTime;
	                AquariumSavedColor.a = Mathf.Lerp(0, 1, textTimer / AquariumSavedFadeTime);
	                AquariumSavedText.color = AquariumSavedColor;
	                if (textTimer > AquariumSavedFadeTime)
	                {
	                    state = State.PILLS;
	                    textTimer = 0;
	                }
	                break;
            case State.PILLS:
                bgTimer += Time.deltaTime;
                PillsBGColor.a = Mathf.Lerp(0, 1, bgTimer / BackgroundFadeTime);
                PillsTextColor.a = PillsBGColor.a;
                PillsBG.color = PillsBGColor;
                PillsText.color = PillsTextColor;
                PillsText.text = "Pills:\n0 / " + stats["totalPills"];
                if (bgTimer > BackgroundFadeTime)
                {
                    textTimer += Time.deltaTime;
                    if (stats["pills"] == 0)
                        textTimer += TextCountSpeed;
                    PillsText.text = "Pills:\n" + Mathf.Floor(Mathf.Lerp(0,stats["pills"],textTimer/TextCountSpeed)) + " / " + stats["totalPills"];
                }
                if (textTimer >= TextCountSpeed)
                {
                    textTimer = 0;
                    bgTimer = 0;
                    state = State.DEATHS;
                }
                break;
	        case State.DEATHS:
	            bgTimer += Time.deltaTime;
	            DeathsBGColor.a = Mathf.Lerp(0, 1, bgTimer / BackgroundFadeTime);
	            DeathsTextColor.a = DeathsBGColor.a;
	            DeathsBG.color = DeathsBGColor;
	            DeathsText.color = DeathsTextColor;
	            DeathsText.text = "Deaths:\n0";
	            if (bgTimer > BackgroundFadeTime)
	            {
	                textTimer += Time.deltaTime;
	                if (stats["deaths"] == 0)
	                    textTimer += TextCountSpeed;
	                DeathsText.text = "Deaths:\n" + Mathf.Floor(Mathf.Lerp(0, stats["deaths"], textTimer / TextCountSpeed));
	            }
	            if (textTimer >= TextCountSpeed)
	            {
	                textTimer = 0;
	                bgTimer = 0;
	                state = State.TIME;
	            }
                break;
	        case State.TIME:
	            bgTimer += Time.deltaTime;
	            TimeBGColor.a = Mathf.Lerp(0, 1, bgTimer / BackgroundFadeTime);
	            TimeTextColor.a = TimeBGColor.a;
	            TimeBG.color = TimeBGColor;
	            TimeText.color = TimeTextColor;
	            TimeText.text = "Time:\n0s";
	            if (bgTimer > BackgroundFadeTime)
	            {
	                var timeString = MakeTimeString(stats["timeTaken"] * (textTimer / TextCountSpeed));
	                textTimer += Time.deltaTime;
	                TimeText.text = "Time:\n" + timeString;
	            }
	            if (textTimer >= TextCountSpeed)
	            {
	                textTimer = 0;
	                bgTimer = 0;
                    postLevelScript.Done();
	                state = State.DONE;
	            }
	            break;

        }
	
	}

    private string MakeTimeString(float time)
    {
        if (time < 1)
            return "0s";
        var thisTime = time;
        float seconds = 0;
        float minutes = 0;
        float hours = 0;
        var builder = new StringBuilder();
        while (thisTime >= 60)
        {
            while (thisTime >= 3600)
            {
                hours++;
                thisTime -= 3600;
            }
            minutes++;
            thisTime -= 60;
        }
        seconds = Mathf.Floor(thisTime);
        if (hours > 0)
            builder.Append("" + hours + "h ");
        if (minutes > 0)
            builder.Append("" + minutes + "m ");
        if (seconds > 0)
            builder.Append("" + seconds + "s");
        return builder.ToString();
    }

    public void Activate(PostLevelScript script)
    {
        if (!active)
        {
            GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().PlayEndMusic();
            state = State.SAVEDTEXT;
            stats = World.i.WinLevel();
            postLevelScript = script;
        }
        active = true;
    }
}
