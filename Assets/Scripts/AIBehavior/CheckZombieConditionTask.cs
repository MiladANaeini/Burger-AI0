using System.Collections.Generic;

public class CheckZombieCondition : Node
{
    private Kim myKim;

    public CheckZombieCondition(Kim kim) : base(new List<Node>())
    {
        myKim = kim;
    }

    // Evaluates the condition: whether a zombie is detected
    public override ReturnState EvaluateState()
    {
        if ((bool)myKim.blackboard.Data["zombieDetected"])
        {
            return ReturnState.s_Success;
        }
        return ReturnState.s_Failure;

    }
}
