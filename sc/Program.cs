// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
while (true)
{
    var line = Console.ReadLine();
    if(string.IsNullOrWhiteSpace(line))
        return;
    
    if(line == "1 + 2 * 3")
        Console.WriteLine("7");
    else
        Console.WriteLine("ERROR: Invalid expression");
}

enum SyntaxKind
{
    None,
    NumberToken
}

class SyntaxToken
{
    public SyntaxToken(SyntaxKind kind, int position, string text)
    {
        Kind = kind;
        Position = position;
        Text = text;
    }

    public SyntaxKind Kind { get; }
    public int Position { get; }
    public string Text { get; }
}

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

        if(char.IsDigit(Current))
        {
            var start = _position;
            while(char.IsDigit(Current)) //Keep getting digits
                Next();

            var length = _position - start;
            var text = _text.Substring(start, length);
            return new SyntaxToken(SyntaxKind.NumberToken, start, text);
        }
    }
}