using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Editor and Public Variables
    [SerializeField]
    private Row[] m_rows;

    [SerializeField]
    private Row m_startRow;

    [SerializeField]
    private float m_distanceBetweenBeats;

    public float m_beatsPerMinute = 60f;

    [SerializeField]
    private InputField m_sheetMusicName = null;

    [SerializeField]
    private string m_pathToAudio = "Simple-peaceful-piano-melody";
    #endregion

    #region Singleton
    public static GameManager Singleton = null;
    #endregion

    #region Cached Components
    private float p_numSecPerBeat;
    private IEnumerator current;

    private byte p_curNote = 0;

    private Queue<byte> p_sheetMusic;
    private string p_fileName = "test";

    private bool p_recording = false;
    #endregion

    #region Cached References
    private AudioSource audioSource;
    #endregion

    #region Initialize
    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    void Start()
    {
        p_numSecPerBeat = 60.0f / m_beatsPerMinute;
        float refVelocity = m_distanceBetweenBeats / p_numSecPerBeat;

        m_startRow.SetVelocity(refVelocity, m_startRow.Distance);

        foreach (Row row in m_rows)
        {
            row.SetVelocity(refVelocity, m_startRow.Distance);
        }
    }
    #endregion

    #region Spawner Methods
    public void Spawn(byte note)
    {
        byte temp = p_curNote;
        if (p_recording)
        {
            p_curNote = note;
        }

        for (int i = m_rows.Length - 1; i >= 0; i--)
        {
            if (note % 2 == 1 && temp % 2 == 0)
            {
                m_rows[i].Spawn();
            }
            if (note % 2 == 0 && temp % 2 == 1)
            {
                m_rows[i].DestroyFirstObject();
            }
            note /= 2;
            temp /= 2;
        }
    }

    IEnumerator Player()
    {
        while (p_sheetMusic.Count > 0)
        {
            byte note = p_sheetMusic.Dequeue();
            Spawn(note);
            yield return new WaitForSeconds(p_numSecPerBeat);
        }
    }
    #endregion

    #region Button Methods
    public void Play()
    {
        //Unpause the game only
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            audioSource.UnPause();
            return;
        }
        //Restart the song
        else
        {
            // Clear the board
            Stop();
            Start();

            // Set the filename of the sheet music
            if (m_sheetMusicName != null)
            {
                p_fileName = m_sheetMusicName.text;
            }

            p_sheetMusic = new Queue<byte>();
            Read(p_fileName);
            SetUpSong();
            current = Player();
            StartCoroutine(current);
        }
    }

    public void Record()
    {
        if (!p_recording)
        {
            Start();
            p_recording = true;
            SetUpSong();
            Pause();
        }
        else
        {
            Stop();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        audioSource.Pause();
    }

    public void Stop()
    {
        p_recording = false;
        p_curNote = 0;
        m_sheetMusicName.interactable = true;

        if (current != null)
        {
            StopCoroutine(current);
        }
        m_startRow.ClearRow();

        foreach (Row row in m_rows)
        {
            row.ClearRow();
        }
    }

    public void Step()
    {
        p_curNote = 0;
        StartCoroutine(StepHelper());
    }

    IEnumerator StepHelper()
    {
        Time.timeScale = 1;
        audioSource.UnPause();
        yield return new WaitForSecondsRealtime(p_numSecPerBeat);
        Pause();
    }

    public void Twinkle()
    {
        m_pathToAudio = "Twinkle";
        m_sheetMusicName.text = "Star"; 
        p_fileName = "Star";
        m_beatsPerMinute = 82;
    }

    public void HeartAndSoul()
    {
        m_pathToAudio = "Heart&Soul";
        m_sheetMusicName.text = "HeartAndSoul";
        p_fileName = "HeartAndSoul";
        m_beatsPerMinute = 245;
    }

    public void Price()
    {
        m_pathToAudio = "Price";
        m_sheetMusicName.text = "Price";
        p_fileName = "Price";
        m_beatsPerMinute = 320;
    }
    #endregion

    #region Read and Play Song
    void Read(string file)
    {
        if (p_fileName == "")
        {
            p_fileName = "test";
        }
        string path = "Assets/Resources/Sheets/" + p_fileName + ".txt";

        StreamReader reader = new StreamReader(path);
        while (!reader.EndOfStream)
        {
            byte value = Convert.ToByte(reader.ReadLine());
            p_sheetMusic.Enqueue(value);
        }
        reader.Close();
    }

    void SetUpSong()
    {
        audioSource = GetComponent<AudioSource>();
        var clip = Resources.Load<AudioClip>("Tracks/" + m_pathToAudio);
        if (clip == null)
        {
            return;
        }
        audioSource.clip = clip;
        StartCoroutine(StartNoteSpawnCheck());
    }

    IEnumerator StartNoteSpawnCheck()
    {
        m_startRow.Spawn();
        while (!m_startRow.GetComponentInChildren<StartHitController>().hit &&
            !p_recording)
        {
            yield return null;
        }
        audioSource.Play();
    }
    #endregion
}
