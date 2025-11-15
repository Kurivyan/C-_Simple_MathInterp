namespace MyNamespace;

public class Evaluator
{
	public ASTNode root;
	public Dictionary<string, double> variables;
	public Dictionary<string, Func<List<double>, double>> functions;

	public Evaluator(ASTNode ast,
										Dictionary<string, double> vars,
										Dictionary<string,
																Func<List<double>, double>> funcs)
	{
		this.root = ast;
		this.variables = vars;
		this.functions = funcs;
	}
}

public static class EvaluateExtension
{

	public static double Evaluate(this ASTNode node, Dictionary<string, double> vars,
														Dictionary<string, Func<List<double>, double>> funcs)
	{
		return node switch
		{
			BinaryNode b => b.Evaluate(vars, funcs),
			UnaryNode u => u.Evaluate(vars, funcs),
			NumNode n => n.Evaluate(vars, funcs),
			VarNode v => v.Evaluate(vars, funcs),
			GroupNode g => g.Evaluate(vars, funcs),
			FuncNode f => f.Evaluate(vars, funcs),
			_ => throw new Exception("Unknown AST node type")
		};
	}
	public static double Evaluate(this BinaryNode root, Dictionary<string, double> vars,
														Dictionary<string, Func<List<double>, double>> funcs)
	{
		double left = root.left.Evaluate(vars, funcs);
		double right = root.right.Evaluate(vars, funcs);
		return root.op switch
			{
				"+" => left + right,
				"-" => left - right,
				"/" => left / right,
				"*" => left * right,
				"^" => Math.Pow(left, right),
				_ => throw new Exception("unknown binary operator")
			};
	}

	public static double Evaluate(this UnaryNode root, Dictionary<string, double> vars,
														Dictionary<string, Func<List<double>, double>> funcs)
	{
		double core = root.core.Evaluate(vars, funcs);
		return root.op switch
			{
				"+" => core,
				"-" => -1 * core,
				_ => throw new Exception("unknown unary operator")
			};
	}

	public static double Evaluate(this NumNode root, Dictionary<string, double> vars,
														Dictionary<string, Func<List<double>, double>> funcs)
	{
		return root.value;
	}

	public static double Evaluate(this VarNode root, Dictionary<string, double> vars,
														Dictionary<string, Func<List<double>, double>> funcs)
	{
		return vars[root.id];
	}

	public static double Evaluate(this FuncNode root, Dictionary<string, double> vars,
														Dictionary<string, Func<List<double>, double>> funcs)
	{
		List<double> parsed_args = new List<double>();
		foreach(var arg in root.args)
        {
            parsed_args.Add(arg.Evaluate(vars, funcs));
        }
		return funcs[root.id](parsed_args);
	}

	public static double Evaluate(this GroupNode root, Dictionary<string, double> vars,
														Dictionary<string, Func<List<double>, double>> funcs)
	{
		return root.core.Evaluate(vars, funcs);
	}

}	
