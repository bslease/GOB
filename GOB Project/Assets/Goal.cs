﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal
{
    public string name;
    public float value;

    public Goal (string goalName, float goalValue)
    {
        name = goalName;
        value = goalValue;
    }

    public float GetDiscontentment(float newValue)
    {
        return newValue * newValue;
    }
}

public class Action
{
    public string name;
    //public string targetGoal;
    //public float utility;
    public List<Goal> targetGoals;

    public Action (string actionName)
    {
        name = actionName;
        targetGoals = new List<Goal>();
    }

    //public float GetGoalChange(Goal goal)
    //{
    //    return goal.name == targetGoal ? utility : 0f;
    //}

    public float GetGoalChange(Goal goal)
    {
        //return goal.name == targetGoal ? utility : 0f;

        // find the goal in the list of goals
        foreach (Goal target in targetGoals)
        {
            if (target.name == goal.name)
            {
                return target.value;
            }
        }
        return 0f;
    }
}