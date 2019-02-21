using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    #region Editor Variables
    public Vector2 m_Velocity = new Vector2(-1, 0);
    #endregion

    #region Private Variables
    private Rigidbody2D rb;

    public float Speed = 1.0f;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 velocity = Speed * m_Velocity;
        rb.velocity = velocity;
    }
}
