using System;

public class CharIdentifier : Identifier
{
    public char symbol;

    public CharIdentifier(char symbol, string indentifier) : base(indentifier)
    {
        this.symbol = symbol;
    }

    public override string ToString()
    {
        return String.Format("(\'{0}\', \"{1}\")", symbol, indentifier);
    }
}