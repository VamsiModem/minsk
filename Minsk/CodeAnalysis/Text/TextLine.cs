namespace Minsk.CodeAnalysis.Text
{
    public sealed class TextLine{
        public TextLine(SourceText text, int start, int length, int lengthIncludingLinebreak)
        {
            Text = text;
            Start = start;
            Length = length;
            LengthIncludingLinebreak = lengthIncludingLinebreak;
        }

        public SourceText Text { get; }
        public int Start { get; }
        public int Length { get; }
        public int End => Start + Length;
        public int LengthIncludingLinebreak { get; }
        public TextSpan Span => new TextSpan(Start, Length);
        public TextSpan SpanIncludingLineBreak => new TextSpan(Start, LengthIncludingLinebreak);

        public override string ToString() => Text.ToString(Span);

    }
}