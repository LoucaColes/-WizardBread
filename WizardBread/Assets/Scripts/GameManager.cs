﻿using UnityEngine;

public class GameManager : MonoBehaviour
{
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
