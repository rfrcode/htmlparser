using System.Collections.Generic;

public class NodeBase:INode{
    private List<INode> children = new List<INode>();

    public List<INode> Children { get => children; set => children = value; }

    public void Add(INode n) => children.Add(n);

    public void Add(List<INode> nn) => children.AddRange(nn);
    public virtual void Open(){

    }
    public virtual void Close(){
        
    }
    public virtual void Write(){
    }

}

