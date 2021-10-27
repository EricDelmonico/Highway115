using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Cutscene : MonoBehaviour
{
    //private ControlsInput controls;
    //public enum Control { NEXT }

    public IntroControls nextSprite;
    
    public Sprite[] mainSprites;
    public Sprite[] spriteLoop;
    public GameObject toTheBeatText;
    public bool isIntro;
    private int currentMainSpriteNum = 0;
    private int currentLoopSpriteNum = 0;

    private void Awake()
    {
        nextSprite = new IntroControls();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!mainSprites[0])
        {
            Debug.Log("There aren't any sprites in the sprite list");
        }
        NextImage();
        nextSprite.Intro.NextImage.performed += _ => NextImage();
    }
    
    /// <summary>
    /// Switches the current image
    /// Progresses through main sprites and then loops through loop sprites
    /// </summary>
    void NextImage()
    {
        if(currentMainSpriteNum < mainSprites.Length)
        {
            transform.GetComponent<UnityEngine.UI.Image>().sprite = mainSprites[currentMainSpriteNum];
        }
        else
        {
            if (currentLoopSpriteNum >= spriteLoop.Length)
            {
                currentLoopSpriteNum = 0;
            }
            transform.GetComponent<UnityEngine.UI.Image>().sprite = spriteLoop[currentLoopSpriteNum];
            currentLoopSpriteNum++;
        }
        if (currentMainSpriteNum == mainSprites.Length)
        {
            toTheBeatText.SetActive(true);
        }
        if (currentMainSpriteNum == mainSprites.Length + 1)
        {
            nextSprite.Intro.NextImage.performed += Calibrate;
        }
        currentMainSpriteNum++;
    }
    
    void OnEnable()
    {
        nextSprite.Enable();
    }
    
    void OnDisable()
    {
        nextSprite.Disable();
    }

    private void Calibrate(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        bool calibrated = Conductor.Instance.CalibrateOffset();
    
        if (calibrated)
        {
            if(isIntro)
            {
                nextSprite.Disable();
                SceneManager.LoadScene(2);
            }
        }
    }
}
