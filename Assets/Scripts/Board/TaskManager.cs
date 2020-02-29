﻿using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TaskManager : MonoBehaviour
{
    public static Action TaskComplete;

    public Board board;
    public float taskScale = 1f;

    private List<GameObject> pieces;
    private List<GameObject> tasks;

    public void Init()
    {
        pieces = new List<GameObject>();
        tasks = new List<GameObject>();

        pieces = board.piecePrefabs;

        for (int i = 0; i < transform.childCount; i++)
        {
            tasks.Add(transform.GetChild(i).gameObject);
        }

        transform.position = new Vector2(transform.position.x + Camera.main.transform.position.x, transform.position.y + Camera.main.transform.position.y + transform.localScale.y);

        SpawnTasks();
        AnimateTasks();
    }

    public void CheckTask(Piece piece)
    {
        if (piece.matchValue == tasks[0].transform.GetChild(0).GetComponent<Piece>().matchValue)
        {
            Debug.Log("Task complete!");

            TaskComplete();

            UpdateTask();
        }
    }

    private void UpdateTask()
    {
        Transform task0 = tasks[0].transform.GetChild(0).transform;
        tasks[0].transform.GetChild(0).SetParent(null);
        task0.DOPunchScale(new Vector3(2f, 2f, 0f), 1f, 1, 1f);
        task0.DOMoveX(-board.width, 1f).OnComplete(() => MoveTaskToEnd(task0));
        for (int i = 1; i < tasks.Count; i++)
        {
            tasks[i].transform.GetChild(0).DOMoveX(tasks[i - 1].transform.position.x, 0.5f);
            tasks[i].transform.GetChild(0).SetParent(tasks[i - 1].transform);
        }
    }

    private void MoveTaskToEnd(Transform task)
    {
        task.SetParent(tasks[2].transform);
        task.transform.position = new Vector2(board.width, task.transform.position.y);
        task.DOMoveX(tasks[2].transform.position.x, 0.3f);
    }

    private void SpawnTask(Transform parent)
    {
        GameObject taskObject = Instantiate(pieces[UnityEngine.Random.Range(0, board.colorCount)]);
        taskObject.transform.SetParent(parent);
        taskObject.transform.localScale = new Vector2(taskScale, taskScale);
        taskObject.transform.localPosition = parent.localPosition;
        taskObject.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
    }

    private void AnimateTask(Transform task, float delay, float endPos)
    {
        task.transform.position += new Vector3(board.width, 0f, 0f);
        task.transform.DOMoveX(endPos, 0.5f).SetDelay(delay).SetEase(Ease.InOutSine);
    }

    private void AnimateTasks()
    {
        float delay = 0.2f;
        foreach (GameObject task in tasks)
        {
            GameObject t = task.transform.GetChild(0).gameObject;
            float pos = t.transform.position.x;
            AnimateTask(t.transform, delay, pos);
            delay += 0.2f;
        }
    }

    public void SpawnTasks()
    {
        foreach (GameObject task in tasks)
        {
            SpawnTask(task.transform);
        }
    }
}