using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chosen : MonoBehaviour {

    public Sprite whiteblock;
    public GameObject townObject;
    public float speed = 0.01f;

    public int ID_Number;

    private Vector3 endLocation;
    private GameObject m_improvementsParent;
    private GameObject newTownObject;
    Vector3 arrival = new Vector3(0.0f, 2.0f);
    private bool buttonActive = true;
    


    private void Start()
    {
        m_improvementsParent = GameObject.Find("Improvements");
        if (!m_improvementsParent)
        {
            m_improvementsParent = new GameObject("Improvements");
        }
    }

    public void OnMouseDown()
    {
        if (Town.m_instance.m_state == Town.State.Waiting)
        {
            if (buttonActive == true)
            {
                buttonActive = false;
                GetComponent<SpriteRenderer>().sprite = null;

                GameObject newTownObject = Instantiate(townObject) as GameObject;
                newTownObject.transform.parent = m_improvementsParent.transform;
                Town.m_instance.InputIn(ID_Number, newTownObject.GetComponent<Improvement>());
            }
        }
    }
}
