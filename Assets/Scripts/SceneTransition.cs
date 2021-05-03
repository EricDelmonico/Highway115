using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Animator transition;
    public EndTile endObject;

    [SerializeField]
    private float transitionTime = 1f;

    // Update is called once per frame
    void Update()
    {
        if(endObject.levelEnded == true)
        {
            TransitionNextScene();
        }
    }

    public void TransitionNextScene()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        // play animation
        transition.SetTrigger("Start");

        // wait
        yield return new WaitForSeconds(transitionTime);

        // Load scene
        SceneManager.LoadScene(sceneIndex);
    }
}
