// See https://aka.ms/new-console-template for more information

// We're basically building an expression tree here
//  1 + 2 * 3 
//
//      +
//     / \
//    1   *
//       / \  
//      2   3

while (true)
{
    var line = Console.ReadLine();
    if(string.IsNullOrWhiteSpace(line))
        return;
    
    var lexer = new Lexer(line);
    while (true)
    {
        var token = lexer.NextToken();
        if(token.Kind == SyntaxKind.EndOfFileToken)
            break;
        Console.Write($"{token.Kind}: '{token.Text}'");

        if(token.Value != null)
            Console.Write($" {token.Value}");

        Console.WriteLine();
    }    
}

enum SyntaxKind
{
    NumberToken,
    WhitespaceToken,
    PlusToken,
    MinusToken,
    StarToken,
    SlashToken,
    OpenParenthesisToken,
    CloseParenthesisToken,
    BadToken,
    EndOfFileToken
}

class SyntaxToken
{
    public SyntaxToken(SyntaxKind kind, int position, string text, object? value)
    {
        Kind = kind;
        Position = position;
        Text = text;
        Value = value;
    }

    public SyntaxKind Kind { get; }
    public int Position { get; }
    public string Text { get; }
    public object? Value { get; }
}

/// <summary>
/// Lexer produces tokens. Think of these as leaves on a tree
/// </summary>
class Lexer
{
    private readonly string _text;
    private int _position;

    public Lexer(string text)
    {
        _text = text;
    }

    private char Current 
    {
        get 
        {
            if(_position >= _text.Length)
                return '\0';
            else 
                return _text[_position];
        }
    }

    private void Next()
    {
        _position++;
    }

    public SyntaxToken NextToken()
    {
        // <numbers>
        // + - * / ( )
        // <whitespaces>

        if(_position >= _text.Length)
            return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);

        if(char.IsDigit(Current))
        {
            var start = _position;
            while(char.IsDigit(Current)) //Keep reading
                Next();

            var length = _position - start;
            var text = _text.Substring(start, length);
            int.TryParse(text, out var value);
            return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
        }

        if(char.IsWhiteSpace(Current))
        {
            var start = _position;
            while(char.IsWhiteSpace(Current)) //Keep reading
                Next();

            var length = _position - start;
            var text = _text.Substring(start, length);
            return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
        }

        if(Current == '+')
            return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
        else if(Current == '-')
            return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
        else if(Current == '*')
            return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
        else if(Current == '/')
            return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
        else if(Current == '(')
            return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
        else if(Current == ')')
            return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);
        

        return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
    }
}

abstract class SyntaxNode
{
    public abstract SyntaxKind Kind { get; }
}

abstract class ExpressionSyntax : SyntaxNode
{
    
}



/// <summary>
/// Parser produces sentences, which are trees. Effectively syntax nodes.
/// Parsers are typically optimized for speed. They won't try to lex the entire input,
/// they may consume some tokens at a time and process. We're going to just take the entire input.
/// </summary>
class Parser
{
    private readonly SyntaxToken[] _tokens;
    private int _position;

    public Parser(string text)
    {
        var tokens = new List<SyntaxToken>();

        var lexer = new Lexer(text);
        SyntaxToken token;
        do
        {
            token = lexer.NextToken();

            if(token.Kind != SyntaxKind.BadToken &&
                token.Kind != SyntaxKind.BadToken)
            {
                tokens.Add(token);
            }

        } while (token.Kind != SyntaxKind.EndOfFileToken);

        _tokens = tokens.ToArray();
    }

    private SyntaxToken Peek(int offset)
    {
        var index = _position + offset;
        if(index >= _tokens.Length)
            return _tokens[_tokens.Length - 1];

        return _tokens[index];
    }

    private SyntaxToken Current => Peek(0);

}
