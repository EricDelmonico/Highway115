using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Cutscene : MonoBehaviour
{
    //private ControlsInput controls;
    //public enum Control { NEXT }

    public InputAction next;
    
    public Sprite[] mainSprites;
    public Sprite[] spriteLoop;
    public bool isIntro;
    private int currentMainSpriteNum = 0;
    private int currentLoopSpriteNum = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!mainSprites[0])
        {
            Debug.Log("There aren't any sprites in the sprite list");
        }
        NextImage();
        next.performed += _ => NextImage();
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
        if (currentMainSpriteNum == mainSprites.Length + 1)
        {
            next.performed += Calibrate;
        }
        currentMainSpriteNum++;
    }
    
    void OnEnable()
    {
        next.Enable();
    }
    
    void OnDisable()
    {
        next.Disable();
    }

    private void Calibrate(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        bool calibrated = Conductor.Instance.CalibrateOffset();
    
        if (calibrated)
        {
            if(isIntro)
            {
                SceneManager.LoadScene(2);
            }
        }
    }
}
