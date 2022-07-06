using System;

public class StringIdentifier : Identifier
{
    public string String;

    public StringIdentifier(StringIdentifier id) : base(id.indentifier)
    {
        this.String = id.String;
    }

    public StringIdentifier(string String, string indentifier) : base(indentifier)
    {
        this.String = String;
    }

    public override string ToString()
    {
        return String.Format("(\"{0}\", \"{1}\")", String, indentifier);
    }
}