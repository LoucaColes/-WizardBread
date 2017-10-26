using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ButtonsIDListener : MonoBehaviour {

    public Button ID_0, ID_1, ID_2, ID_3, ID_4, ID_5, ID_6, ID_7, ID_8, ID_9, RESET;

        void Start()
    {
        Button btn_0 = ID_0.GetComponent<Button>();
        btn_0.onClick.AddListener(TaskOnClick);

        Button btn_1 = ID_1.GetComponent<Button>();
        btn_1.onClick.AddListener(TaskOnClick);

        Button btn_2 = ID_2.GetComponent<Button>();
        btn_2.onClick.AddListener(TaskOnClick);

        Button btn_3 = ID_3.GetComponent<Button>();
        btn_3.onClick.AddListener(TaskOnClick);

        Button btn_4 = ID_4.GetComponent<Button>();
        btn_4.onClick.AddListener(TaskOnClick);

        Button btn_5 = ID_5.GetComponent<Button>();
        btn_5.onClick.AddListener(TaskOnClick);

        Button btn_6 = ID_6.GetComponent<Button>();
        btn_6.onClick.AddListener(TaskOnClick);

        Button btn_7 = ID_7.GetComponent<Button>();
        btn_7.onClick.AddListener(TaskOnClick);

        Button btn_8 = ID_8.GetComponent<Button>();
        btn_8.onClick.AddListener(TaskOnClick);

        Button btn_9 = ID_9.GetComponent<Button>();
        btn_9.onClick.AddListener(TaskOnClick);

        Button btn_Reset = RESET.GetComponent<Button>();
        btn_Reset.onClick.AddListener(TaskOnClick);
    }


    public void TaskOnClick()
    {
        Debug.Log("Button clicked with ID: " + name);
    }
   
}