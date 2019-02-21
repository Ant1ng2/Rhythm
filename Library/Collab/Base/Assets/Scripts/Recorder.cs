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
    private string[] axises;

    [SerializeField]
    private bool m_record = false;

    [SerializeField]
    private InputField m_input;
    #endregion

    #region Cached Components
    private float numSecPerBeat;
    private bool written;

    private List<byte> music;
    private bool[] note;
    #endregion

    #region Initialize
    void Start()
    {
        if (m_record)
        {
            Record();
        }
    }

    public void Record()
    {
        m_input.interactable = false;
        numSecPerBeat = 60.0f / beat;
        music = new List<byte>();
        note = new bool[axises.Length];
        written = false;

        StartCoroutine(Calculate());
    }
    #endregion

    #region Note Calculation
    IEnumerator Calculate()
    {
        while (!written)
        {
            for (int i = 0; i < axises.Length; i++)
            {
                if (Input.GetAxis(axises[i]) == 1)
                {
                    note[i] = true;
                }
                else
                {
                    note[i] = false;
                }                
            }
            byte result = arrayToByte();
            Debug.Log(result);
            music.Add(result);
            note = new bool[axises.Length];
            yield return new WaitForSeconds(numSecPerBeat);
        }
        yield return null;
    }

    private byte arrayToByte()
    {
        byte value = 0;
        foreach (bool i in note)
        {
            value *= 2;
            if (i)
            {
                value += 1;
            }
        }
        return value;
    }
    #endregion

    #region Writes and Exit Button
    void Write()
    {
        string fileName = m_input.text;
        if (fileName == "")
        {
            fileName = "test.txt";
        }
        string path = "Assets/Resources/Sheets" + fileName;
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        foreach (byte i in music)
        {
            writer.WriteLine(i);
        }
        writer.Close();
        m_input.interactable = true;
    }

    public void Exit()
    {
        written = true;
        Write();
    }
    #endregion
}
