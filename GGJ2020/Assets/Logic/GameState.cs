﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Logic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public Quest CurrentQuest;
    public List<BodyPartVisual> BeltContent;
    public List<BodyPartVisual> PartQueue;
    public List<Body> BodyQueue;
    public int Gold;
    public float Difficulty;
    private static QuestGenerator _questGenerator = new QuestGenerator();
    public float QuestTimer;
    public QuestResult LastQuestResult;
    private int _difficultyDelta = 2;
    private bool paused;
    public Party Party;

    private enum QuestState
    {
        New,
        Complete
    }

    private QuestState questState;

    public void Start()
    {
        Restart();
    }

    public void Pause()
    {
        StateEventManager.SendStatePause();
        paused = true;
    }

    public void Play()
    {
        StateEventManager.SendStatePlay();
        paused = false;
    }

    public void Update()
    {
        if (paused) return;
        if (QuestTimer > 0)
        {
            QuestTimer -= Time.deltaTime;
        }
        else
        {
            if (questState == QuestState.New)
            {
                FinishQuest();
                IncreaseDifficulty();
            }
            else
            {
                StartNewQuest();
            }
        }
    }

    public void AddBody(GameObject bodyObject)
    {
        if (bodyObject == null) return;
        var rslt = new Body();
        var vis = bodyObject.GetComponent<BodyPartVisual>();
        foreach (var item in bodyObject.GetComponentsInChildren<SnappingPoint>())
        {
            if (item.AssignedPart == null)
            {
                continue;
            }
            var bpv =item.AssignedPart.GetComponent<BodyPartVisual>();
            if (bpv == null)
            {
                continue;
            }
            rslt.Slots.Add(item.AssignedPart.GetComponent<BodyPartVisual>().AssignedPart);
        }

        rslt.Part = vis.AssignedPart;
        BodyQueue.Add(rslt);
        Party.Bodies.Add(rslt);
    }

    public void FinishQuest()
    {
        foreach (var body in BodyQueue)
        {
            Party.Bodies.Add(body);
        }

        questState = QuestState.Complete;
        LastQuestResult = CurrentQuest?.GetResult(Party);
        Gold += (int)LastQuestResult.Gold;
        QuestEventManager.SendQuestFinished(CurrentQuest, LastQuestResult);
    }

    public void StartNewQuest()
    {
        Party = new Party();
        questState = QuestState.New;
        CurrentQuest = _questGenerator.GenerateQuest(Difficulty);
        QuestTimer = CurrentQuest.MaxDuration;
        QuestEventManager.SendQuestStarted(CurrentQuest);
    }

    public void IncreaseDifficulty()
    {
        Difficulty += _difficultyDelta;
        _difficultyDelta += 1;
    }

    public void Restart()
    {
        Difficulty = 0;
        Gold = 0;


        StartNewQuest();
        QuestTimer = CurrentQuest.MaxDuration;

        BeltContent = new List<BodyPartVisual>();
        PartQueue = new List<BodyPartVisual>();
        BodyQueue = new List<Body>();
        LastQuestResult = null;
    }
}