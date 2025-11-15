namespace MyNamespace; 

public class Parser
{
	public ASTNode root;
	public List<Token> tokens;

	public Parser(List<Token> list)
	{
		this.tokens = list;
	}
	
	public Parser(ASTNode root, List<Token> list)
	{
		this.root = root;
		this.tokens = list;
	}
}

public static class ParserExtension
{
	public static ASTNode Parse(this Parser obj)
	{
		var curent = obj.tokens.GetEnumerator();
		curent.MoveNext();
		return parse_sum(curent);
	}

	public static ASTNode parse_sum(IEnumerator<Token> curent)
	{
		ASTNode left = parse_mul(curent);
		if(curent.Current == null)
			return left;
		Token token = curent.Current;
		if(curent.match(TokenType.PLUS) || curent.match(TokenType.MINUS))
		{
			string op = token.value!;
			ASTNode right = parse_mul(curent);
			return (new BinaryNode(op, left, right));
		}
		return left;
	}

	public static ASTNode parse_mul(IEnumerator<Token> curent)
	{
		ASTNode left = parse_pow(curent);
		if(curent.Current == null)
			return left;
		Token token = curent.Current;
		if(curent.match(TokenType.STAR) || curent.match(TokenType.SLASH))
		{
			string op = token.value!;
		 	ASTNode right = parse_pow(curent);
			return (new BinaryNode(op, left, right));
		}
		return left;
	}

	public static ASTNode parse_pow(IEnumerator<Token> curent)
	{
		ASTNode left = parse_unary(curent);
		if(curent.Current == null)
			return left;
		Token token = curent.Current;
		if(curent.match(TokenType.CARET))
		{
			string op = token.value!;
			ASTNode right = parse_pow(curent);
			return (new BinaryNode(op, left, right));
		}
		return left;
	}

	public static ASTNode parse_unary(IEnumerator<Token> curent)
	{
		Token token = curent.Current;
		if(curent.match(TokenType.PLUS) || curent.match(TokenType.MINUS))
		{
			string op = token.value!;
			ASTNode root = parse_unary(curent);
			return new UnaryNode(op, root);
		}
		return parse_primary(curent);
	}

	public static ASTNode parse_primary(IEnumerator<Token> curent)
	{
		Token token = curent.Current;
		if(curent.match(TokenType.NUM))
			return new NumNode(token.value!);

		if(curent.match(TokenType.LPAREN))
			return parse_group(curent);
		
		if(curent.match(TokenType.ID))
			return parse_function(curent, token);

		throw(new Exception("NoTokenMatched"));
	}

	public static ASTNode parse_group(IEnumerator<Token> curent)
	{
		ASTNode root = parse_sum(curent);
		if(!curent.match(TokenType.RPAREN))
			throw new Exception("Expected ), parse_group");
		return (new GroupNode(root));
	}

	public static ASTNode parse_function(IEnumerator<Token> curent, Token token)
    {
		if(curent.match(TokenType.LPAREN))
        {
            List<ASTNode> args = new List<ASTNode>();
			if(!curent.match(TokenType.RPAREN))
            {
                do
                {
					ASTNode arg = parse_sum(curent);
					args.Add(arg);
                } while(curent.match(TokenType.COMMA));
            }
			if(curent.match(TokenType.RPAREN))
				return new FuncNode(token.value!, args);
        }

		return new VarNode(token.value!);
    }

	public static bool match(this IEnumerator<Token> curent, TokenType type)
	{
		if (curent.Current == null)
			return false;
		if (curent.Current.type == type)
		{
			curent.MoveNext();
			return true;
		}
		return false;
	}
}
