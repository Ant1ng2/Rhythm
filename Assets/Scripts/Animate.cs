using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{
    [SerializeField]
    private string m_axis = "";

    private bool p_isAxisInUse = false;

    private void Update()
    {
        if (m_axis != "" && GetAxisDown(m_axis))
        {
            GetComponent<Animator>().SetTrigger("Attack");
        }
    }

    private bool GetAxisDown(string axis)
    {
        if (Input.GetAxisRaw(axis) != 0)
        {
            if (!p_isAxisInUse)
            {
                p_isAxisInUse = true;
                return true;
            }
            return false;
        }
        else
        {
            p_isAxisInUse = false;
            return false;
        }
    }
}
