using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    private GameObject m_note;

    public float speed = 1.0f;
    #endregion

    #region Private Variables
    #endregion

    #region Main Methods
    public GameObject Spawn()
    {
        GameObject noteInstance = Instantiate(m_note, transform.position, transform.rotation);
        noteInstance.layer = gameObject.layer;
        noteInstance.GetComponent<Rigidbody2D>().velocity = speed * noteInstance.GetComponent<NoteController>().m_Velocity;
        noteInstance.transform.localScale = transform.parent.localScale;
        return noteInstance;
    }
    #endregion
}
