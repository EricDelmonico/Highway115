using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: could probably be named better...?
public enum HitFeedback
{
    // On beat is even
    Perfect = 0,
    Great = 2,

    // Off beat is odd
    Early = 1,
    Late = 3
}

public class Conductor : MonoBehaviour
{
    private static Conductor instance;
    /// <summary>
    /// Creates a conductor from the passed in prefab
    /// </summary>
    /// <param name="prefab">The conductor prefab</param>
    /// <returns>The created conductor</returns>
    public static Conductor CreateInstance(GameObject prefab) => (instance = Instantiate(prefab).GetComponent<Conductor>());
    /// <summary>
    /// The existing conductor
    /// </summary>
    public static Conductor Instance => instance ?? 
        throw new System.InvalidOperationException("Conductor was never created. " +
            "Check ConductorCreator or add one to the scene if one doesn't exist.");

    [Header("Stuff to edit:")]
    [Tooltip("BPM of the song in this conductor's audio source")]
    public float bpm;
    [Tooltip("Amount of time in seconds that the player can be early before the beat is considered a 'miss'")]
    public float allowedEarlyTime = 0.05f;
    [Tooltip("Amount of time in seconds that the player can be late before the beat is considered a 'miss'")]
    public float allowedLateTime = 0.05f;
    [Tooltip("How many beats before the offset is considered calibrated")]
    public int beatsForCalibration = 20;

    [Tooltip("How long moving items in the scene will take to move.")]
    /// <summary>
    /// How long moving items in the scene will take to move. This is here to
    /// keep the seconds to move consistent between all movable objects.
    /// </summary>
    public float secondsToMove = 0.1f;

    [Header("**Below stuff is only here for debug/visibility purposes. None of it needs to be edited**")]
    public float secondsPerBeat;
    public float songPosition; // in seconds
    public float timeWhenSongStarted;
    public AudioSource musicSource;
    public float beatOffset; // Calibrated by the player
    public float lastBeatSeconds; // The time since song start at which the previous beat occured
    public List<float> validationNums;

    // Same thing as allowed times but for "perfect"
    private float perfectLateTime;
    private float perfectEarlyTime;
    private HitFeedback feedback;

    /// <summary>
    /// Fires every time a beat occurs. Subscribe to this event in order 
    /// to have things happen whenever a beat is hit.
    /// </summary>
    public event System.EventHandler BeatOccurred;

    /// <summary>
    /// Fires after all fields of the conductor have been initialized in the start method.
    /// </summary>
    public event System.EventHandler ConductorInitializationFinished;

    // Start is called before the first frame update
    void Start()
    {
        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();

        //Calculate the number of seconds in each beat
        secondsPerBeat = 60f / bpm;

        //Record the time when the music starts
        timeWhenSongStarted = (float)AudioSettings.dspTime;

        //Start the music
        musicSource.Play();

        validationNums = new List<float>();

        lastBeatSeconds = 0;

        // Subject to change...
        perfectEarlyTime = allowedEarlyTime / 2;
        perfectLateTime = allowedLateTime / 2;

        // After secondsPerBeat is initialized, make sure the time
        // to move is less than the seconds in a beat with a reasonable buffer.
        // Otherwise, movement would be a little broken.
        if (secondsToMove > secondsPerBeat * 0.8f)
        {
            secondsToMove = secondsPerBeat * 0.8f;
        }

        RaiseConductorInitializationFinished();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Menu.isPaused)
        {
            //determine how many seconds since the song started
            songPosition = (float)(AudioSettings.dspTime - timeWhenSongStarted - beatOffset);

            // If this is true, a beat happened
            if (songPosition > lastBeatSeconds + secondsPerBeat)
            {
                lastBeatSeconds += secondsPerBeat;

                // Beat occurred
                RaiseBeatOccured();
            }
        }
    }

    // TODO: Figure out where to put input, and have it be calibrated here
    /// <summary>
    /// Calibrates how much the song position needs to be offset based on 
    /// where the beats are. This is done by the player. Result is stored in
    /// <see cref="beatOffset"/>
    /// </summary>
    /// <returns>true if calibration is finished</returns>
    public bool CalibrateOffset()
    {
        if (validationNums.Count < beatsForCalibration)
        {
            float diff;
            songPosition = (float)(AudioSettings.dspTime - timeWhenSongStarted - beatOffset);
            diff = songPosition - lastBeatSeconds;
            validationNums.Add(diff);
            return false;
        }
        // Calibration is finished
        else
        {
            beatOffset = Avg(validationNums);

            // TODO?: Clear so calibration can be done again whenever
            //validationNums.Clear();
            return true;
        }
    }

    /// <summary>
    /// Checks this moment for beat accuracy, will return feedback to tell the
    /// caller whether the action was late, early, perfect, or great.
    /// </summary>
    /// <returns>Feedback on the accuracy of the action</returns>
    public HitFeedback CheckBeatAccuracy()
    {
        songPosition = (float)(AudioSettings.dspTime - timeWhenSongStarted - beatOffset);
        float diff = songPosition - lastBeatSeconds;

        // If the player was less than 1/4 beat off, the hit was good
        if (diff < allowedLateTime)
        {
            if (diff < perfectLateTime)
            {
                feedback = HitFeedback.Perfect;
            }
            else
            {
                feedback = HitFeedback.Great;
            }
        }
        // Slightly early
        else if (diff > secondsPerBeat - allowedEarlyTime)
        {
            if (diff > secondsPerBeat - perfectEarlyTime)
            {
                feedback = HitFeedback.Perfect;
            }
            else
            {
                feedback = HitFeedback.Great;
            }
        }
        else
        {
            if (diff > secondsPerBeat / 2)
            {
                feedback = HitFeedback.Early;
            }
            else
            {
                feedback = HitFeedback.Late;
            }
        }

        return feedback;
    }

    /// <summary>
    /// Simple average function
    /// </summary>
    /// <param name="nums">list of nums to average together</param>
    /// <returns></returns>
    private float Avg(List<float> nums)
    {
        float total = 0;
        foreach (float num in nums)
        {
            total += num;
        }

        return total / nums.Count;
    }

    public bool showGUI = true;
    /// <summary>
    /// Some simple UI for debug purposes
    /// </summary>
    private void OnGUI()
    {
        if (showGUI)
        {
            GUI.BeginGroup(new Rect(0, 0, 500, 200));
            GUI.Label(new Rect(0, 50, 500, 200), $"<size=20>{feedback.ToString()}</size>");
            GUI.Label(new Rect(0, 100, 500, 100), $"<size=20>Calibration: {beatsForCalibration - validationNums.Count}. Offset: {Avg(validationNums)}</size>");
            GUI.Label(new Rect(0, 150, 500, 100), $"<size=20>Last beat seconds: {lastBeatSeconds}. </size>");
            GUI.EndGroup();
        }
    }

    private void RaiseBeatOccured()
    {
        if (BeatOccurred != null)
        {
            BeatOccurred.Invoke(this, System.EventArgs.Empty);
        }
    }

    private void RaiseConductorInitializationFinished()
    {
        if (ConductorInitializationFinished != null)
        {
            ConductorInitializationFinished.Invoke(this, System.EventArgs.Empty);
        }
    }
}
