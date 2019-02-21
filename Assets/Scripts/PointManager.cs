using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager singleton;

    #region Editor Variables
    [SerializeField]
    private CircleCollider2D m_Note;

    [SerializeField]
    private CircleCollider2D m_Hit;

    [SerializeField]
    private int m_Multiplier = 10;
    #endregion

    #region Private Variables
    private int points = 0;
    #endregion

    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
    }

    public void AddPointsDistance(float value)
    {
        float totalDistance = m_Hit.radius + m_Note.radius;
        points += m_Multiplier - Mathf.RoundToInt(m_Multiplier * value / totalDistance);
        Debug.Log(points);
    }
}
