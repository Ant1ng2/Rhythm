using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    #region Editor variables
    [SerializeField]
    private int beat = 60;

    [SerializeField]
    private string[] m_axises;

    [SerializeField]
    private InputField m_input = null;
    #endregion

    #region Cached Components
    private float p_numSecPerBeat;
    private bool p_writing;

    private List<byte> p_music;
    private Dictionary<string, bool> p_isNoteAtPosition;
    private Dictionary<string, bool> p_isAxisInUse;
    #endregion

    #region Initialize
    void Start()
    {
        p_music = new List<byte>();

        p_isNoteAtPosition = new Dictionary<string, bool>();
        p_isAxisInUse = new Dictionary<string, bool>();
        foreach (string axis in m_axises)
        {
            p_isNoteAtPosition.Add(axis, false);
            p_isAxisInUse.Add(axis, false);
        }
    }

    public void Record()
    {
        if (p_writing)
        {
            Write();
        }
        else
        {
            p_writing = true;
            m_input.interactable = false;
            p_numSecPerBeat = 60.0f / beat;
        }
    }

    public void Stop()
    {
        p_writing = false;
    }
    #endregion

    #region Inputs
    void Update()
    {
        if (p_writing)
        {
            byte prior = DictToByte();
            foreach (string axis in m_axises)
            {
                if (GetAxisDown(axis))
                {
                    p_isNoteAtPosition[axis] = !p_isNoteAtPosition[axis];
                }
            }
            byte current = DictToByte();
            if (current != prior)
            {
                GameManager.Singleton.Spawn(current);
            }
        }
    }

    private bool GetAxisDown(string axis)
    {
        if (Input.GetAxisRaw(axis) != 0)
        {
            if (!p_isAxisInUse[axis])
            {
                p_isAxisInUse[axis] = true;
                return true;
            }
            return false;
        }
        else 
        {
            p_isAxisInUse[axis] = false;
            return false;
        }
    }
    #endregion

    #region Dictionary Methods
    private byte DictToByte()
    {
        byte value = 0;
        foreach (string axis in m_axises)
        {
            value *= 2;
            if (p_isNoteAtPosition[axis])
            {
                value += 1;
            }
        }
        return value;
    }

    private void Clear()
    {
        foreach (string axis in m_axises)
        {
            p_isNoteAtPosition[axis] = false;
        }
    }
    #endregion

    #region Button Methods (except Record)
    public void Step()
    {
        byte result = DictToByte();
        Clear();
        Debug.Log(result);
        p_music.Add(result);
    }
    #endregion

    #region Writes
    void Write()
    {
        p_writing = false;
        m_input.interactable = true;
        WriteToFile();
    }

    private void WriteToFile()
    {
        string fileName = m_input.text;
        if (fileName == "")
        {
            fileName = "test.txt";
        }
        string path = "Assets/Resources/Sheets/" + fileName + ".txt";
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        if (p_music != null)
        {
            foreach (byte i in p_music)
            {
                writer.WriteLine(i);
            }
        }
        writer.Close();
    }
    #endregion
}
