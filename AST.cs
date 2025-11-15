public abstract record class ASTNode {

};

public record BinaryNode : ASTNode {
	public string op;
	public ASTNode left, right;

	public BinaryNode(string op, ASTNode left, ASTNode right)
    {
        this.op = op;
        this.left = left;
        this.right = right;
    }
};

public record UnaryNode : ASTNode {
	public string op;
	public ASTNode core;

	public UnaryNode(string op, ASTNode core)
	{
		this.op = op;
		this.core = core;
	}
};

public record  GroupNode : ASTNode {
	public ASTNode core;

	public GroupNode(ASTNode core)
	{
		this.core = core;
	}
};

public record VarNode : ASTNode {
	public string id;

	public VarNode(string id)
	{
		this.id = id;
	}
};

public record FuncNode : ASTNode {
  public string id;
	public List<ASTNode> args;
	public FuncNode(string id, List<ASTNode> list)
    {
        this.id = id;
		this.args = list;
    }
}

public record NumNode : ASTNode {
	public double value;

	public NumNode(string value)
	{
		this.value = Convert.ToDouble(value);
	}
};
