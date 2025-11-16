using MyNamespace;

class Program
{
	static void Main(string[] args)
	{
		while(true)
		{
			string input = ReadLine();
			if(input == null)
				break;
			if(string.IsNullOrWhiteSpace(input))
				continue;

			string stringified_input = input.DeleteSpaces();

			Lexer tokens = new Lexer(stringified_input).Tokenize();
	
			ASTNode tree = new Parser(tokens.Tokens!).Parse();

			Dictionary<string, double> vars = new Dictionary<string, double>();
			vars.Add("x", 1);
			vars.Add("y", 2);

			Dictionary<string, Func<List<double>, double>> funcs =
				new Dictionary<string, Func<List<double>, double>>();
			funcs.Add("pow", (x)=> Math.Pow(x[0], x[1]) );
			funcs.Add("sin", (x)=> Math.Sin(x[0]));

			WriteLine($"Result of {stringified_input} = {tree.Evaluate(vars, funcs)}");
		}
	}
}
