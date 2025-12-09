using System;
using System.Collections.Generic;

[Serializable]
public class KahootData
{
    public int id;
    public string title;
    public string description;
    public List<Question> questions;
}

[Serializable]
public class Question
{
    public string statement;
    public List<Answer> answers;
    public string rightAnswer;
    public int duration;
}

[Serializable]
public class Answer
{
    public string a;
    public string b;
    public string c;
    public string d;
}
