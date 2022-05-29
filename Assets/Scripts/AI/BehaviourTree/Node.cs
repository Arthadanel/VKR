using System.Collections.Generic;

namespace AI.BehaviourTree
{
    public class Node
    {
        public Node Parent;
        protected List<Node> Children;
        
        protected NodeState State;

        public int Weight { get; private set; } = 1;
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Node()
        {
            Parent = null;
        }
        
        public Node(List<Node> children)
        {
            foreach (Node child in children)
                _Attach(child);
        }

        public void SetWeight(int weight)
        {
            Weight = weight;
        }

        private void _Attach(Node node)
        {
            node.Parent = this;
            Children.Add(node);
        }
        
        public virtual NodeState Evaluate() => NodeState.FAILURE;
        
        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }
        public object GetData(string key)
        {
            if (_dataContext.TryGetValue(key, out var value))
                return value;

            Node node = Parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;
                node = node.Parent;
            }
            return null;
        }
        public bool RemoveData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Node node = Parent;
            while (node != null)
            {
                bool cleared = node.RemoveData(key);
                if (cleared)
                    return true;
                node = node.Parent;
            }
            return false;
        }
    }
}