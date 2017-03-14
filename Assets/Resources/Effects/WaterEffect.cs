using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class WaterEffect : MonoBehaviour {
    Vortex vortex;
    [Tooltip("the time in seconds for one vortex to pass the screen")]
    public float speed = 5;
    private float timer = 0;
    public bool vertical = false;

    public Vector2 radius = new Vector2(0.8F, 0.6F);
    public float angle = 10;
    public Vector2 center = new Vector2(0, 0.5F);
    // Use this for initialization
    void Start () { 
        vortex=Camera.main.GetComponent<Vortex>();
        vortex.enabled = true;
        vortex.radius = radius;
        vortex.angle = angle;
        vortex.center = center;
    }
	
	// Update is called once per frame
	void Update ()
    {
        vortex.angle = angle;
        vortex.radius = radius;
        vortex.center = Vector2.Lerp(new Vector2(-radius.x, center.y), new Vector2(1 + radius.x, center.y), timer / speed);
        if (vertical)
            vortex.center = Vector2.Lerp(new Vector2(center.x, 1+radius.y), new Vector2(center.x, - radius.y), timer / speed);
        timer += Time.deltaTime;
        if (timer >= speed)
        {
            timer -= speed;
            if (!vertical)
                center.y = Random.Range(0.0f, 1.0f);
            else center.x = Random.Range(0.0f, 1.0f);

        }
    }
}
