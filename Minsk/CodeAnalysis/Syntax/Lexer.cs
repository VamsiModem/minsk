using System.Collections.Generic;
using Minsk.CodeAnalysis.Text;
namespace Minsk.CodeAnalysis.Syntax
{
    internal sealed class Lexer
    {
        private readonly SourceText _text;
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private int _position;
        private int _start;
        private SyntaxKind _kind;
        private object _value;
       
         public Lexer(SourceText text)
        {
            _text = text;
        }


        public DiagnosticBag Diagnostics => _diagnostics;
        private char Current => Peek(0);
        private char LookAhead => Peek(1);
        private char Peek(int offset){
            var index = _position + offset;
            if(index >= _text.Length){
                return '\0';
            }
            return _text[index];
        }
        public SyntaxToken Lex(){
            _start = _position;
            _kind = SyntaxKind.BadToken;
            _value = null;
            

            switch (Current)
            {
                case '\0':
                     _kind = SyntaxKind.EOFToken;
                     break;
                case '+':
                    _kind = SyntaxKind.PlusToken;
                    _position++;
                    break;
                case '-':
                    _kind = SyntaxKind.MinusToken;
                    _position++;
                    break;
                case '*':
                    _kind = SyntaxKind.StarToken;
                    _position++;
                    break;
                case '/':
                    _kind = SyntaxKind.SlashToken;
                    _position++;
                    break;
                case '(':
                    _kind = SyntaxKind.LParenToken;
                    _position++;
                    break;
                case ')':
                    _kind = SyntaxKind.RParenToken;
                    _position++;
                    break;
                case '{':
                    _kind = SyntaxKind.LBraceToken;
                    _position++;
                    break;
                case '}':
                    _kind = SyntaxKind.RBraceToken;
                    _position++;
                    break;
                case '&':
                    if(LookAhead == '&'){
                        _position += 2;
                        _kind = SyntaxKind.AmpresandAmpresandToken;
                        break;
                    }
                    
                    break;
                case '|':
                    if(LookAhead == '|'){
                        _position += 2;
                        _kind = SyntaxKind.PipePipeToken;
                        break;
                    }
                    break;
                case '=':
                    _position++;
                    if(Current != '='){
                        _kind = _kind = SyntaxKind.EqualsToken;
                    }else{
                        _position ++;
                        _kind = _kind = SyntaxKind.EqualsEqualsToken;
                    }
                        
                    break;                    
                case '!':
                    _position++;
                    if(Current != '='){
                        _kind = _kind = SyntaxKind.BangToken;
                    }else{
                        _position ++;
                        _kind = _kind = SyntaxKind.BangEqualsToken;
                    } 
                    break; 
                case '0': case '1': case '2': case '3': case '4':    
                case '5': case '6': case '7': case '8': case '9':   
                     ReadNumberToken();
                     break; 
                case ' ':
                case '\t':
                case '\n':
                case '\r':
                    ReadWhiteSpaceToken();
                    break;
                default:
                    if (char.IsLetter(Current))
                    {
                        ReadIdentifierOrKeywordToken();
                    }

                    else if (char.IsWhiteSpace(Current))
                    {
                        ReadWhiteSpaceToken();
                    }
                    else {
                        _diagnostics.ReportBadCharacter(_position, Current);
                        _position++;
                    }
                    break;
                        
            }
            var length = _position - _start;
            var text = SyntaxFacts.GetText(_kind);
            if(text is null){
                text = _text.ToString(_start, length);
            }
            return new SyntaxToken(_kind, _start, text, _value);
        }

        private void ReadIdentifierOrKeywordToken()
        {
            while (char.IsLetter(Current))
                _position++;

            var length = _position - _start;
            var text = _text.ToString(_start, length);
            _kind = SyntaxFacts.GetKeyWordKind(text);
        }

        private void ReadWhiteSpaceToken()
        {
            while (char.IsWhiteSpace(Current))
                _position++;

            _kind = SyntaxKind.WhiteSpaceToken;
        }

        private void ReadNumberToken()
        {
            while (char.IsDigit(Current))
                 _position++;


            var length = _position - _start;
            var text = _text.ToString(_start, length);
            if (!int.TryParse(text, out var value))
            {
                _diagnostics.ReportInvalidNumber(new TextSpan(_start, length), text, typeof(int));
            }
            _value = value;
            _kind = SyntaxKind.NumberToken;
        }
    }
}