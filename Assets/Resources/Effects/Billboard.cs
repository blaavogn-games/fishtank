using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {
    private SpriteRenderer spriteRenderer;
    private Color initColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initColor = spriteRenderer.color;
    }
    
    void Update () {
        transform.LookAt(Camera.main.transform.position, -Vector3.up);
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, initColor, 0.06f);
    }

    public void Spawn()
    {
        spriteRenderer.color = new Color(0, 0, 0, 0);
    }
}
