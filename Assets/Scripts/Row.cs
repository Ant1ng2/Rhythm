using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    private GameObject m_attackHitBox;

    [SerializeField]
    private Spawner m_spawner;
    #endregion

    #region Cached Variables
    private Stack<GameObject> p_notes;

    //Movement related
    private float p_speed = 0.0f;
    private Vector2 p_direction = new Vector2(-1, 0);
    #endregion

    public float Distance { private set; get; }

    #region Initialize
    void Awake()
    {
        p_direction = m_attackHitBox.transform.position - m_spawner.transform.position;
        Distance = p_direction.magnitude;
        p_direction.Normalize();

        p_notes = new Stack<GameObject>();
    }
    #endregion

    #region Main Methods
    public void SetVelocity(float velocity, float refDistance)
    {
        p_speed = velocity * Distance / refDistance;
        m_spawner.speed = p_speed;
    }

    public void Spawn()
    {
        p_notes.Push(m_spawner.Spawn());
    }

    public void DestroyFirstObject()
    {
        if (p_notes.Count > 0)
        {
            Destroy(p_notes.Pop());
        }
    }

    public void ClearRow()
    {
        foreach (GameObject note in p_notes)
        {
            Destroy(note);
        }
        if (m_attackHitBox.GetComponent<StartHitController>() != null)
        {
            m_attackHitBox.GetComponent<StartHitController>().hit = false;
        }
    }
    #endregion
}
