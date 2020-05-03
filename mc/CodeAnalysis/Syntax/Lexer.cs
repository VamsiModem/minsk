using System.Collections.Generic;

namespace Minsk.Code.Syntax
{
    internal sealed class Lexer
    {
        public Lexer(string text)
        {
            _text = text;
        }

        private readonly string _text;
        private List<string> _diagnostics = new List<string>();
        public IEnumerable<string> Diagnostics => _diagnostics;
        private int _position;
        private char Current{
            get{
                if(_position >= _text.Length){
                    return '\0';
                }
                return _text[_position];
            }
        }
        private void Next(){ _position++; }
        public SyntaxToken Lex(){
            if(_position >= _text.Length){
                return new SyntaxToken(SyntaxKind.EOFToken, _position, "\0", null);
            }
            if(char.IsDigit(Current)){
                var start = _position;
                while(char.IsDigit(Current))
                    Next();
                
                var length = _position - start;
                var text = _text.Substring(start, length);
                if(!int.TryParse(text, out var value)){
                    _diagnostics.Add($"The number {_text} can not be represented by an int32");
                }
                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }

            if(char.IsWhiteSpace(Current)){
                var start = _position;
                while(char.IsWhiteSpace(Current))
                    Next();
                
                var length = _position - start;
                var text = _text.Substring(start, length);
                return new SyntaxToken(SyntaxKind.WhiteSpaceToken, start, text, null);
            }

             if(char.IsLetter(Current)){
                var start = _position;
                while(char.IsLetter(Current))
                    Next();
                
                var length = _position - start;
                var text = _text.Substring(start, length);
                var kind = SyntaxFacts.GetKeyWordKind(text);
                return new SyntaxToken(SyntaxKind.WhiteSpaceToken, start, text, null);
            }

            switch (Current)
            {
                case '+':
                    return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
                case '-':
                    return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
                case '*':
                    return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
                case '/':
                    return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
                case '(':
                    return new SyntaxToken(SyntaxKind.LParenToken, _position++, "(", null);
                case ')':
                    return new SyntaxToken(SyntaxKind.RParenToken, _position++, ")", null);
            }
            _diagnostics.Add($"Error: bad character input: '{Current}'");
            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position-1, 1), null);
        }
    }
}