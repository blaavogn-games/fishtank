using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InGameMenu : MonoBehaviour
{
    public EventSystem EventSystem;
    public UnityEngine.UI.Button Resume;
    public UnityEngine.UI.Button NewGame;
    public UnityEngine.UI.Button SelectTank;
    public UnityEngine.UI.Button TankHighlight;
    public GameObject Tanks;
    public int MaxLevel = 2;
    private Vector3 tankTarget;
    private int tankSelected;
    private int resumeLevel;

    private GameObject _lastSelected;
    void Start()
    {

        tankSelected = 0;
        tankTarget = new Vector3(tankSelected * -1200, 0, 0);
        resumeLevel = 2; //ToDo take level from memero
        Resume.onClick.AddListener(() =>
            {
                Debug.Log("Resume");
                StartGame(resumeLevel);
            });
        EventTrigger trigger = Resume.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener(
            (data) => {
                MoveTanksAbsolute(resumeLevel);
            });
        trigger.triggers.Add(entry);


        NewGame.onClick.AddListener(() =>
            {
                Debug.Log("New Game");
                //ToDo clear SaveGame
                StartGame(0);
            });
        trigger = NewGame.GetComponent<EventTrigger>();
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener(
            (data) => {
                MoveTanksAbsolute(0);
            });
        trigger.triggers.Add(entry);

        SelectTank.onClick.AddListener(() => ToggleTankSelect(true));
        TankHighlight.onClick.AddListener(() => StartGame(tankSelected));

        trigger = TankHighlight.GetComponent<EventTrigger>();
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Cancel;
        entry.callback.AddListener(
            (data) => {
                ToggleTankSelect(true);

            });
        trigger.triggers.Add(entry);
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Move;
        entry.callback.AddListener(
            (data) => {
                var d = (AxisEventData)data;
                if(d.moveDir == MoveDirection.Right)
                    MoveTanksRelative(1);
                if (d.moveDir == MoveDirection.Left)
                    MoveTanksRelative(-1);
            });
        trigger.triggers.Add(entry);
        ToggleTankSelect(false);
        EventSystem.SetSelectedGameObject(Resume.gameObject); 
    }

    void Update()
    {
        Tanks.transform.position = Vector3.Lerp(Tanks.transform.position, tankTarget, 0.1f);
        if (Input.GetButton("Cancel"))
            ToggleTankSelect(false);
    }

    void ToggleTankSelect(bool enable)
    {
        TankHighlight.gameObject.SetActive(enable);
        if (enable)
            EventSystem.SetSelectedGameObject(TankHighlight.gameObject);
        else
            EventSystem.SetSelectedGameObject(SelectTank.gameObject); 
    }

    void MoveTanksRelative(int i)
    {
        tankSelected += i;
        tankSelected = Math.Max(0,Math.Min(tankSelected, MaxLevel));
        tankTarget = new Vector3(tankSelected * -1200, 0, 0);
    }
    void MoveTanksAbsolute(int i)
    {
        tankSelected = i;
        tankSelected = Math.Max(0, Math.Min(tankSelected, MaxLevel));
        tankTarget = new Vector3(tankSelected * -1200, 0, 0);
    }

    void StartGame(int i)
    {
        Debug.Log("Start game " + i);
    }


}
