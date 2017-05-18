using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class HungerEffect : MonoBehaviour {
    /*Fisheye eye;
    BlurOptimized blur;
    Player player;
    float hunger;
    private float timer = 0;
    Texture2D fadeTexture;
    Rect rect;

    [Tooltip("in seconds")]
    public float MaxHunger = 120;
    public float CurrentHunger;

    [Header("Hunger bar controls")]
    public bool DisplayBar = true;
    public Texture2D FullBarTexture;
    public Color FullColour = Color.green;
    public Texture2D EmptyBarTexture;
    public Color EmptyColour = Color.gray;
    [Tooltip("0 to 1 relative to the screen size where 1 is the entire screen")]
    public Vector2 BarPosition = new Vector2(0.02f, 0.06f);
    [Tooltip("0 to 1 relative to the screen size where 1 is the entire screen")]
    public Vector2 BarSize = new Vector2(0.25f, 0.05f);
    [Tooltip("in pixels")]
    public Vector2 BackgroundSizeOffset = new Vector2(5, 5);
    //public Color EmptyColour = Color.red;
    float barFill = 0;

    [Header("Blur controls")]
    [Tooltip("At what hunger level to start the effect (in seconds)")]
    public float BlurEffectStart = 80;
    [Tooltip("At what hunger level the effect has reached it's maximum and stays like that (in seconds)")]
    public float BlurEffectEnd = 30;
    [Range(0.0f, 10.0f)]
    public float MaxBlurSize = 3;
    [Range(1.0f, 4.0f)]
    public int blurIterations = 1;

    [Header("Fisheye controls")]
    [Tooltip("At what hunger level to start the effect (in seconds)")]
    public float EyeEffectStart = 50;
    [Tooltip("At what hunger level the effect has reached it's maximum and stays like that (in seconds)")]
    public float EyeEffectEnd = 10;
    [Range(0.0f, 1.5f)]
    public float MaxStrengthX = 0.7f;
    [Range(0.0f, 1.5f)]
    public float MaxStrengthY = 0.7f;

    [Header("Blinking colour controls")]
    [Tooltip("At what hunger level to start the effect (in seconds)")]
    public float BlinkEffectStart = 50;
    [Tooltip("At what hunger level the effect has reached it's maximum and stays like that (in seconds)")]
    public float BlinkEffectEnd = 10;
    public Color color = Color.grey;
    [Range(0.0f, 1.0f)]
    public float StartAlpha = 0.8f;
    [Range(0.0f, 1.0f)]
    public float EndAlpha = 0.8f;
    [Tooltip("in seconds")]
    public float BlinkSpeedStart = 3.0f;
    [Tooltip("in seconds")]
    public float BlinkSpeedEnd = 0.8f;

    private float blinkAlpha = 0;
    private float blinkSpeed = 0;

    private float alpha = 0;
    private float blinkTimer = 0;
    private float fadeDirection = 1;

    [Header("Current blink stats")]
    public float currentAlpha;
    public float currentBlinkSpeed;

    // Use this for initialization
    void Start ()
    {
        if(EmptyBarTexture == null)
            EmptyBarTexture = new Texture2D(1, 1);
        if(FullBarTexture == null)
            FullBarTexture = new Texture2D(1, 1);


        fadeTexture = new Texture2D(1, 1);
        rect = new Rect(0, 0, Screen.width, Screen.height);
        //Swapping start and end values if they are plotted in incorectly
        if (BlurEffectStart < BlurEffectEnd || EyeEffectStart < EyeEffectEnd)
        {
            float temp = BlurEffectStart;
            BlurEffectStart = BlurEffectEnd;
            BlurEffectEnd = temp;

            temp = EyeEffectStart;
            BlurEffectStart = EyeEffectEnd;
            EyeEffectEnd = temp;

            Debug.LogError("Effect start hunger lower than end hunger, swapping values");
        }

        //Assigning variables to scripts in the scene
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        hunger = player.hunger;
        eye = Camera.main.GetComponent<Fisheye>();
        blur = Camera.main.GetComponent<BlurOptimized>();

        player.hunger = MaxHunger;
        player.MaxHunger = MaxHunger;
        //initialising values
        ResetValues();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Set hunger bar size
        barFill = hunger / MaxHunger;

        //Set hunger to the players hunger level
        if (hunger < player.hunger)
            ResetValues();
        hunger = player.hunger;
        CurrentHunger = hunger;
        //Blur effect
        BlurEffect();
        //Fisheye effect
        EyeEffect();
        //Blinking effect
        BlinkEffect();

        //Decreases hunger and kills player if it gets to 0
        player.hunger -= Time.deltaTime;
        if (hunger <= 0)
            Debug.Log("You're Dead from Starvation");
        //increase timer
        timer += Time.deltaTime;
    }
    void ResetValues()
    {
        timer = 0;
        blur.enabled = false;
        blur.downsample = 0;
        blur.blurSize = 0;
        blur.blurIterations = blurIterations;

        eye.enabled = false;
        eye.strengthY = 0;
        eye.strengthX = 0;
    }

    void BlurEffect()
    {
        if (hunger <= BlurEffectStart && hunger >= BlurEffectEnd)
        {
            blur.enabled = true;
            blur.blurSize = Mathf.Lerp(0, MaxBlurSize, (timer - (MaxHunger - BlurEffectStart)) / (BlurEffectStart - BlurEffectEnd));
        }
        else if (hunger < BlurEffectEnd)
        {
            blur.enabled = true;
            blur.blurSize = MaxBlurSize;
        }
    }

    void EyeEffect()
    {
        if (hunger <= EyeEffectStart && hunger >= EyeEffectEnd)
        {
            eye.enabled = true;
            eye.strengthX = Mathf.Lerp(0, MaxStrengthX, (timer - (MaxHunger - EyeEffectStart)) / (EyeEffectStart - EyeEffectEnd));
            eye.strengthY = Mathf.Lerp(0, MaxStrengthY, (timer - (MaxHunger - EyeEffectStart)) / (EyeEffectStart - EyeEffectEnd));
        }
        else if (hunger < EyeEffectEnd)
        {
            eye.enabled = true;
            eye.strengthX = MaxStrengthX;
            eye.strengthY = MaxStrengthY;
        }
    }

    void BlinkEffect()
    {
        if (hunger <= BlinkEffectStart && hunger >= BlinkEffectEnd)
        {
            blinkAlpha = Mathf.Lerp(StartAlpha, EndAlpha, (timer - (MaxHunger - BlinkEffectStart)) / (BlinkEffectStart - BlinkEffectEnd));
            blinkSpeed = Mathf.Lerp(BlinkSpeedStart, BlinkSpeedEnd, (timer - (MaxHunger - BlinkEffectStart)) / (BlinkEffectStart - BlinkEffectEnd));
        }
        else if (hunger < BlinkEffectEnd)
        {
            blinkAlpha = EndAlpha;
            blinkSpeed = BlinkSpeedEnd;
        }
        else
            return;

        if (blinkTimer >= blinkSpeed)
        {
            fadeDirection *= -1;
            blinkTimer = blinkSpeed;
        }
        if (blinkTimer <= 0)
        {
            fadeDirection *= -1;
            blinkTimer = 0;
        }
        blinkTimer += (fadeDirection*Time.deltaTime);

        alpha = Mathf.Lerp(0, blinkAlpha, blinkTimer/blinkSpeed);
        currentAlpha = alpha;
        currentBlinkSpeed = blinkSpeed;
    }

    private void OnGUI()
    {
        if (DisplayBar)
        {
            GUI.color = EmptyColour;
            GUI.DrawTexture(new Rect(
                (BarPosition.x * Screen.width)-BackgroundSizeOffset.x, 
                (BarPosition.y * Screen.height)-BackgroundSizeOffset.y, 
                (BarSize.x * Screen.width)+(BackgroundSizeOffset.x * 2), 
                (BarSize.y * Screen.height)+ (BackgroundSizeOffset.y * 2)), 
                EmptyBarTexture);
            
            GUI.color = FullColour;
            if (hunger > 0)
                GUI.DrawTexture(new Rect(BarPosition.x*Screen.width,BarPosition.y*Screen.height, (BarSize.x*Screen.width)*barFill,BarSize.y*Screen.height), FullBarTexture);
        }
        if (alpha > 0)
        {
            color.a = alpha;
            GUI.color = color;
            GUI.depth = -10000;

            GUI.DrawTexture(rect, fadeTexture);
        }
    }*/
}
