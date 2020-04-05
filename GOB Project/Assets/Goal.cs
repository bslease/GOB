using System.Collections;
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
}

public class Action
{
    public string name;
    public string targetGoal;
    public float utility;

    public Action (string actionName, string goalName, float utilityValue)
    {
        name = actionName;
        targetGoal = goalName;
        utility = utilityValue;
    }

    public float GetGoalChange(Goal goal)
    {
        return (goal.name == targetGoal ? utility : 0f);
    }
}