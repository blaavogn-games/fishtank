using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class HungerEffect : MonoBehaviour {
    Fisheye eye;
    BlurOptimized blur;
    Player player;
    float hunger;
    private float timer = 0;

    [Tooltip("in seconds")]
    public float MaxHunger = 120;

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

    // Use this for initialization
    void Start ()
    {
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

        //Set hunger to the players hunger level
        if (hunger < player.hunger)
            ResetValues();
        hunger = player.hunger;

        //Blur effect
        if (hunger <= BlurEffectStart && hunger >= BlurEffectEnd)
        {
            blur.enabled = true;
            blur.blurSize = Mathf.Lerp(0, MaxBlurSize, (timer - (MaxHunger - BlurEffectStart)) / (BlurEffectStart-BlurEffectEnd));
        }
        else if (hunger < BlurEffectEnd)
        {
            blur.blurSize = MaxBlurSize;
        }

        //Fisheye effect
        if (hunger <= EyeEffectStart && hunger >= EyeEffectEnd)
        {
            eye.enabled = true;
            eye.strengthX = Mathf.Lerp(0, MaxStrengthX, (timer-(MaxHunger-EyeEffectStart)) / (EyeEffectStart - EyeEffectEnd));
            eye.strengthY = Mathf.Lerp(0, MaxStrengthY, (timer - (MaxHunger - EyeEffectStart)) / (EyeEffectStart - EyeEffectEnd));
        }
        else if (hunger < EyeEffectEnd)
        {
            eye.strengthX = MaxStrengthX;
            eye.strengthY = MaxStrengthY;
        }

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
}
