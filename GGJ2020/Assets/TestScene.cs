﻿using System.Collections;
using System.Collections.Generic;
using Logic;
using Logic.Quests;
using UnityEngine;

public class TestScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var testPart = new Part();

        var testBody = new Body();
       // testBody.Slots[0].AssignedPart = testPart;
        

        var testParty = new Party();
        testParty.Bodies.Add(testBody);
        var qg = new QuestGenerator();

        var test = qg.GenerateQuest(10);
        Debug.Log(test.Description);
        var result = test.GetResult(testParty);
        Debug.Log(result.Gold.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
