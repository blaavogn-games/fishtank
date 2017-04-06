using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {
    public static World i;
    [HideInInspector]
    public Vector3 SpawnPoint = Vector3.zero;
    [Header("Controls")]
    public bool FlightControls;
    public bool MouseLook;
    string textToShow;
    Color guiColour = Color.white;

    private void Awake()
    {
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
        if (Input.GetKeyDown(KeyCode.M))
            SetMouseLook();
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
}
