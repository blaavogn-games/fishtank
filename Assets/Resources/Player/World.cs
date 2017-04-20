using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class World : MonoBehaviour {
    public static World i;
    [HideInInspector]
    public Vector3 SpawnPoint = Vector3.zero;
    private GameObject spawnObject;
    [Header("Tracking")]
    public int AmountOfLevels = 3;
    [Header("Controls")]
    public bool FlightControls;
    public bool MouseLook;
    string textToShow;
    Color guiColour = Color.white;
    int[] deaths;
    int[] pills;
    float[] timeTaken;
    [HideInInspector]
    public float beginTime = 0;
    public int showDeaths = 0;
    public int showPills = 0;
    public float showTime = 0;
    [HideInInspector]
    private List<string> pillsTaken;
    public List<string> savedPills;

    private void Awake()
    {
        pillsTaken = new List<string>();
        savedPills = new List<string>();
        deaths = new int[AmountOfLevels];
        pills = new int[AmountOfLevels];
        timeTaken = new float[AmountOfLevels];
        if (i == null)
        {
            i = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }
    void InitPrefs()
    {
        FlightControls = ToBool(PlayerPrefs.GetInt("Flight Controls", 0));
        MouseLook = ToBool(PlayerPrefs.GetInt("MouseLook", 1));
        
    }
	// Use this for initialization
	void Start () {
        InitPrefs();
	}
	
	// Update is called once per frame
	void Update () {
        if (guiColour.a > 0)
        {
            guiColour.a -= 0.005f;
        }
        if (Input.GetKeyDown(KeyCode.F))
            SetFlightControls();
        if (Input.GetMouseButtonDown(1))
            SetMouseLook();
        if (Input.GetKey(KeyCode.Alpha0))
            GotoLevel(0);
        if (Input.GetKey(KeyCode.Alpha1))
            GotoLevel(1);
        if (Input.GetKey(KeyCode.Alpha2))
            GotoLevel(2);
        if (Input.GetKey(KeyCode.Alpha3))
            GotoLevel(3);
        if (Input.GetKey(KeyCode.R))
            RestartLevel(false);
        showTime = Time.time-beginTime;
    }

    public void SetFlightControls() {
        FlightControls = !FlightControls;
        PlayerPrefs.SetInt("Flight Controls", ToInt(FlightControls));

        if (FlightControls)
            ShowGuiText("Flight Controls enabled!");
        else
            ShowGuiText("Flight Controls disabled!");
    }
    public void SetMouseLook()
    {
        MouseLook = !MouseLook;
        PlayerPrefs.SetInt("MouseLook", ToInt(MouseLook));

        if (MouseLook)
            ShowGuiText("Mouselook enabled!");
        else
            ShowGuiText("Mouselook disabled!");
    }


    bool ToBool(int input)
    {
        if (input <= 0)
            return false;
        return true;
    }
    int ToInt(bool input)
    {
        if (input)
            return 1;
        return 0;
    }

    void ShowGuiText(string text)
    {
        textToShow = text;
        guiColour.a = 1;
    }
    private void OnGUI()
    {
        GUI.color = guiColour;
        GUI.Label(new Rect(0.02f * Screen.width, 0.02f * Screen.height, 0.25f * Screen.width, 0.05f * Screen.height), textToShow);
    }
    private void OnApplicationQuit()
    {
        //PlayerPrefs.SetInt("Flight Controls", ToInt(FlightControls));
        //PlayerPrefs.SetInt("MouseLook", ToInt(MouseLook));
    }
    public int Death()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        if (level > 0)
            level--;
        deaths[level] += 1;
        //PlayerPrefs.SetInt("Level1Deaths", PlayerPrefs.GetInt("Level1Deaths", 0) + 1);
        showDeaths = deaths[level];
        return deaths[level];
    }
    public int Pill(string instanceName)
    {
        pillsTaken.Add(instanceName);
        int level = SceneManager.GetActiveScene().buildIndex;
        if (level > 0)
            level--;
        pills[level] += 1;
        showPills = pills[level];
        return pills[level];
    }
    public void WinLevel()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        if (level > 0)
            level--;
        timeTaken[level] = beginTime - Time.time;
        GotoLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void GotoLevel(int level)
    {
        SceneManager.LoadScene(level);
    }
    public void RestartLevel(bool useCheckpoint)
    {
        if (!useCheckpoint)
        {
            ResetStats();
        }
        else
        {
            int level = SceneManager.GetActiveScene().buildIndex;
            if (level > 0)
                level--;
            pills[level] = savedPills.Count;
            showPills = pills[level];
        }
        GotoLevel(SceneManager.GetActiveScene().buildIndex);
    }
    public void CheckPoint(GameObject gameO)
    {
        if (SpawnPoint != gameO.transform.position && spawnObject != gameO)
        {
            spawnObject = gameO;
            SpawnPoint = gameO.transform.position;
            Debug.Log("Spawn set to: " + gameO.transform.position.ToString());
            Debug.Log("Saved pills: " + pillsTaken.Count);
            for(int i = 0; i < pillsTaken.Count; i++)
            {
                if(!savedPills.Contains(pillsTaken[i]))
                    savedPills.Add(pillsTaken[i]);
            }
        }
    }
    public void ResetStats()
    {
        for(int i = 0; i < AmountOfLevels; i++)
        {
            deaths[i] = 0;
            pills[i] = 0;
            timeTaken[i] = 0;
        }
        showPills = 0;
        showDeaths = 0;
        showTime = 0;
        SpawnPoint = Vector3.zero;
        pillsTaken.Clear();
        savedPills.Clear();
    }
}
