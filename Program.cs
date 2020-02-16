using System;
using System.IO;
using System.Collections.Generic;

namespace htmlparser
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"htmlToRead.html";
            string readText =  File.ReadAllText(path);

            //NodeParser nodeParser= new NodeParser();
            //INode nodeTree = nodeParser.Parse(readText);
            
            
            //NodeWriter.Write(nodeTree);
            Console.WriteLine(readText);
        }
    }
}

public class NodeWriter{
    public void Write(INode node){
        node.Open();
        foreach(INode n in node.Children)
        {
            Write(n);
        }
        node.Close();
    }
}


public class NodeParser{
    // todo falta parsear entities
    private int Pos = 0;
    //todo
    public List<INode> Parse(string input, int startPos = 0){
        List<INode> output = new List<INode>();
        int lastPos =  input.Length - startPos;
        int localPos = startPos; // todo necesario?
        INode currentNode;
        string txt = "";
        while (localPos <= lastPos){
            // todo revisar condicion
            if (input[localPos] == '<'){
                if(!input.Substring(localPos + 1).StartsWith("/>")){
                    output.Add(new Text(txt));
                    txt = "";
                    currentNode = NodeFactory(input, Pos);
                    currentNode.Add(Parse(input, Pos));
                    // Pos ya ha cambiado, entonces
                    continue;
                }
                else {
                    Pos += 3;
                    return output;
                }
                // todo procesar entities

            }
            txt += input[Pos];
            Pos++;
        }
        output.Add(new Text(txt));
        // todo esta mal?
        return output;

    }

    public INode NodeFactory(string input, int startPos){
        // todo var subcadena
        // todo no procesa tag desconocidos
        // todo no procesa <tag/>
        if (input.Substring(startPos).StartsWith("<p>") || input.Substring(startPos).StartsWith("<P>")){
            Pos += 3;
            return new P();
        }
        else if (input.Substring(startPos).StartsWith("<u>") || input.Substring(startPos).StartsWith("<U>")){
            Pos += 3;
            return new U();
        }
        else if (input.Substring(startPos).StartsWith("<i>") || input.Substring(startPos).StartsWith("<I>")){
            Pos += 3;
            return new I();
        }
        else if (input.Substring(startPos).StartsWith("<strong>") || input.Substring(startPos).StartsWith("<STRONG>")){
            Pos += 8;
            return new Strong();
        }
        else {
            // todo mejorar
            throw new Exception("incorrect input");
        }

    }
}

public interface INode{
    void Open();
    void Close();
    void Write();
    void Add(INode n);
    void Add(List<INode> nn);
    List<INode> Children { get; set; }
}

public class P:NodeBase  {
    public override void Close() => Console.WriteLine();
}

public class I:NodeBase{
    private ConsoleColor oldColor;
    public override void Open(){
    oldColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Yellow;
   }
   public override void Close(){
       Console.ForegroundColor = oldColor; 
   }
}

public class U:NodeBase{
    private ConsoleColor oldColor;
    public override void Open(){
        oldColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Blue;
    }
    public override void Close(){
        Console.ForegroundColor = oldColor; 
    }
}

public class Strong:NodeBase{
    private ConsoleColor oldColor;
    public override void Open(){
        oldColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
    }
    public override void Close(){
        Console.ForegroundColor = oldColor; 
    }
}

class Text:NodeBase{
    private string textContent;

    public Text(string value) => textContent = value;

    public string TextContent { get => textContent; set => textContent = value; }

    //open nada
    public override void Write(){
        Console.Write(TextContent);
    }
}