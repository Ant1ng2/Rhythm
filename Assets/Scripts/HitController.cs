using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitController : MonoBehaviour
{
    [SerializeField]
    private string key;

    void Start()
    {
        key = "Row" + (gameObject.layer - 7).ToString();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetAxis(key) == 1)
        {
            Vector3 otherPosition = other.gameObject.transform.position;
            Vector3 thisPosition = this.gameObject.transform.position;

            Destroy(other.gameObject);

            float distance = Mathf.Abs((otherPosition - thisPosition).magnitude);
            if (PointManager.singleton != null)
            {
                PointManager.singleton.AddPointsDistance(distance);
            }
        }
    }
}
