using System.Collections.Generic;

namespace AI.BehaviourTree
{
    public class Selector : Node
    {
        public Selector() : base() { }

        public Selector(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            foreach (Node node in Children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        State = NodeState.SUCCESS;
                        return State;
                    case NodeState.RUNNING:
                        State = NodeState.RUNNING;
                        return State;
                    default:
                        continue;
                }
            }

            State = NodeState.FAILURE;
            return State;
        }
    }
}