namespace MyNamespace;

public enum TokenType 
{
	ID, NUM,
	PLUS, MINUS, STAR, SLASH, CARET,
	COMMA, LPAREN, RPAREN,
	END
}

public record Token
{
	public TokenType type;
	public string? value;

	public Token(TokenType type, string value = "")
	{
		this.type = type; 
		this.value = value;
	}
	
	public static bool operator ==(Token token, TokenType type)
		=> token.type == type;
	
	public static bool operator !=(Token token, TokenType type)
		=> token.type == type;
}

public class Lexer
{
	public string input;
	public List<Token>? Tokens;

	public Lexer(string input)
	{
		this.input = input;
	}

	public Lexer(string input, List<Token> tokens)
    {
		this.input = input;
		this.Tokens = tokens;
    }

}

public static class LexerExtension
{
	public static string DeleteSpaces(this string obj)
	{
		string res = string.Empty;
		string[] subs = obj.Split(' ');
		foreach (string sub in subs)
		{
			res += sub;
		}
		return res;
	}
	
	public static Lexer Tokenize(this Lexer obj)
	{
		List<Token> output = new List<Token>();
		int index = 0;
		
		while(index < obj.input.Length)
		{
			(int index, Token token) ret = extract(obj.input, index);
			output.Add(ret.token);
			index = ret.index;
		}
		return new Lexer(obj.input, output);
	}

	public static (int index, Token token) extract(string str, int i)
	{
		if(i >= str.Length)
			return (i, new Token(TokenType.END));
		
		if(Char.IsLetter(str[i]))
			return extract_id(str, i);

		if(Char.IsDigit(str[i]))
			return extract_num(str, i);

		return extract_op(str, i);
	}

	public static (int index, Token type) extract_id(string str, int i)
	{
		int current = i;
		while(i < str.Length && Char.IsLetter(str[i]) || str[i] == '_')
			i++;
		return (i,
						 new Token(TokenType.ID, str.Substring(current, i - current)));
	}

	public static (int index, Token type) extract_num(string str, int i)
	{
		int current = i;
		while(i < str.Length && Char.IsDigit(str[i]))
		{
			i++;
        }
		if(i < str.Length && str[i] == '.')
		{
			i++;
			while (i < str.Length && Char.IsDigit(str[i]))
			{
				i++;
			}
		}
		return (i,
						new Token(TokenType.NUM, str.Substring(current, i - current)));
	}

	public static (int index, Token type) extract_op(string str, int i)
	{
		Dictionary<string, TokenType> operators =
		new Dictionary<string,TokenType>{
		{"-", TokenType.MINUS},
		{"+", TokenType.PLUS},
		{"*", TokenType.STAR},
		{"/", TokenType.SLASH},
		{"^", TokenType.CARET},
		{",", TokenType.COMMA},
		{"(", TokenType.LPAREN},
		{")", TokenType.RPAREN}
		};

		if (operators.ContainsKey(str[i].ToString()))
			return (i + 1, new Token(operators[str[i].ToString()], str[i].ToString()));
		else
			throw new Exception();
	}
}
