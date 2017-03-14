using UnityEngine;
using System.Collections;

public class ColourFade : MonoBehaviour {
    Texture2D fadeTexture;
    public bool fadeIn = true;
    public float fadeSpeed = 0.2f;
    public int drawDepth = -1000;
    public Color color;

    float alpha = 1.0f;
    float fadeDir = -1;
    // Use this for initialization
    void Start () {
        fadeTexture = new Texture2D(1, 1);
        if (!fadeIn)
        {
            fadeDir = 1;
            alpha = 0;
        }
	}
	
	// Update is called once per frame
	void Update () {
    }

    private void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        color.a = alpha;
        GUI.color = color;
        GUI.depth = drawDepth;

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
    }
}
