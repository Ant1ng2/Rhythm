using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    private Row[] m_rows;

    [SerializeField]
    private float m_distanceBetweenBeats;

    public float m_beatsPerMinute = 60f;

    [SerializeField]
    private bool m_recording = false;
    #endregion

    #region Cached Components
    private float p_time;
    private float p_numSecPerBeat;

    private Queue<byte> p_music;
    #endregion

    #region Initialize
    void Start()
    {
        if (!m_recording)
        {
            Play();
        }
    }

    public void Play()
    {
        p_numSecPerBeat = 60.0f / m_beatsPerMinute;
        float refVelocity = m_distanceBetweenBeats / p_numSecPerBeat;
        Debug.Log(refVelocity);

        foreach (Row row in m_rows)
        {
            row.SetVelocity(refVelocity, m_rows[0].Distance);
        }

        p_music = new Queue<byte>();
        Read("test.txt");
        StartCoroutine(Spawn());
    }
    #endregion

    #region Main Updates
    IEnumerator Spawn()
    {
        while (p_music.Count > 0)
        {
            byte note = p_music.Dequeue();
            for (int i = m_rows.Length - 1; i >= 0; i--)
            {
                if (note % 2 == 1)
                {
                    m_rows[i].Spawn();
                }
                note /= 2;
            }
            yield return new WaitForSeconds(p_numSecPerBeat);
        }
    }
    #endregion

    #region Read
    void Read(string file)
    {
        string path = "Assets/Resources/Sheets" + file;

        StreamReader reader = new StreamReader(path);
        while (!reader.EndOfStream)
        {
            byte value = Convert.ToByte(reader.ReadLine());
            p_music.Enqueue(value);
        }
        reader.Close();
    }
    #endregion
}
