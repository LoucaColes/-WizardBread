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
        transform.GetChild(0).gameObject.SetActive(false);

    }

    public void OnMouseDown()
    {
        if (buttonActive == true)
        {
            buttonActive = false;
            GetComponent<SpriteRenderer>().sprite = whiteblock;

            transform.GetChild(0).gameObject.SetActive(true);

            GameObject newTownObject = Instantiate(townObject) as GameObject;
            endLocation = newTownObject.transform.position;
            newTownObject.transform.parent = m_improvementsParent.transform;
            Town.m_instance.InputIn(ID_Number, newTownObject.GetComponent<Improvement>());
        }
    }
}
