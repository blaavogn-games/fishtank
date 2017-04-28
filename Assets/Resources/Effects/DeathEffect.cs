using UnityEngine;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine.UI;
using UnityStandardAssets.Cameras;

public class DeathEffect : MonoBehaviour
{
    public Transform CamTarget;
    public GameObject deathCanvas;
    private Text text;
    private Image background;
    [Header("Effect Controls")]
    [Tooltip("In seconds")] public float TextFadeTime = 5;
    [Tooltip("In seconds")] public float FadeOutTime = 5;
    public float CamSpeed = 1;
    [Tooltip("Relative to the point in the world the player dies in")]
    public Vector3 CamEndPosition;
    
    private float beginTime = 0;
    private Color textColor = Color.white;
    private Color backgroundColor = Color.black;
    private float textAlpha = 0;
    private float backgroundAlpha = 0;
    private AutoCam cam;

	// Use this for initialization
    private void Start ()
    {
        deathCanvas = Instantiate(deathCanvas);
        text = deathCanvas.GetComponent<DeathCanvas>().text;
        background = deathCanvas.GetComponent<DeathCanvas>().background;

        cam = Camera.main.transform.parent.transform.parent.GetComponent<AutoCam>();
        cam.SetTarget(CamTarget);
        CamEndPosition=new Vector3(transform.position.x+CamEndPosition.x,transform.position.y+CamEndPosition.y,transform.position.z+CamEndPosition.z);
	    beginTime = Time.time;
        textColor.a = textAlpha;
        text.color = textColor;
        backgroundColor.a = backgroundAlpha;
        background.color = backgroundColor;
    }
	
	// Update is called once per frame
    private void Update ()
	{
	    CamTarget.position = Vector3.MoveTowards(CamTarget.position, CamEndPosition, CamSpeed * Time.deltaTime);

        textColor.a = textAlpha;
	    text.color = textColor;

	    if (Time.time >= beginTime + TextFadeTime)
	    {
	        backgroundColor.a = backgroundAlpha;
	        background.color = backgroundColor;

	        if (backgroundAlpha < 1)
	            backgroundAlpha += (Time.deltaTime / FadeOutTime);
	    }

        if (textAlpha < 1)
	        textAlpha += (Time.deltaTime / TextFadeTime);

	    if (Time.time >= beginTime + TextFadeTime + FadeOutTime)
	        World.i.RestartLevel(true);
    }
}
