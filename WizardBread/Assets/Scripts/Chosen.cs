using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chosen : MonoBehaviour {

    public Sprite whiteblock;
    public GameObject townObject;
    
    public int ID_Number;

    private GameObject m_improvementsParent;
    


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
        GetComponent<SpriteRenderer>().sprite = whiteblock;
        Debug.Log(name + " pressed");
        

        GameObject newTownObject = Instantiate(townObject) as GameObject;
       // _anim = newTownObject.FindComponentOfType<Animation>();
        newTownObject.transform.parent = m_improvementsParent.transform;
        newTownObject.transform.position = townObject.transform.position;

    }
}
