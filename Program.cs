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

            NodeParser nodeParser= new NodeParser();

            INode nodeTree = new NodeBase();
            nodeTree.Add(nodeParser.Parse(readText));
            
            
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
    private string input = "";
    //todo
    public List<INode> Parse(string value){
        if (value != input){
            Pos = 0;
            input = value;
        }
        
        List<INode> output = new List<INode>();
        int lastPos =  input.Length - Pos;
        //int localPos gSubstring(Pos; // todo necesario?
        INode currentNode;
        string txt = "";
        while (Pos <= lastPos){
            // todo revisar condicion
            if (input[Pos] == '<'){
                if(input[Pos + 1] != '/'){
                    output.Add(new Text(txt));
                    txt = "";
                    currentNode = NodeFactory();
                    currentNode.Add(Parse(input));
                    // Pos ya ha cambiado, entonces
                    continue;
                }
                else {
                    Pos = input.Substring(Pos).IndexOf(">") + 1;
                    return output;
                }
                // todo procesar entities

            }
            txt += input[Pos];
            Pos++;
        }
        output.Add(new Text(txt));
        return output;

    }

    public INode NodeFactory(){
        // todo var subcadena
        // todo no procesa tag desconocidos
        // todo no procesa <tag/>
        if (input.Substring(Pos).StartsWith("<p>") || input.Substring(Pos).StartsWith("<P>")){
            Pos += 3;
            return new P();
        }
        else if (input.Substring(Pos).StartsWith("<u>") || input.Substring(Pos).StartsWith("<U>")){
            Pos += 3;
            return new U();
        }
        else if (input.Substring(Pos).StartsWith("<i>") || input.Substring(Pos).StartsWith("<I>")){
            Pos += 3;
            return new I();
        }
        else if (input.Substring(Pos).StartsWith("<strong>") || input.Substring(Pos).StartsWith("<STRONG>")){
            Pos += 8;
            return new Strong();
        }
        // todo esto se puede substituir por "para tag desconocido"
        else if (input.Substring(Pos).StartsWith("<body>") || input.Substring(Pos).StartsWith("<BODY>")){
            Pos += 6;
            return new NodeBase();
        }
        else {
            // todo control de que hay dentro tag desconocido
            //foreach(char c in input.Substring(Pos)){


            //}
            
            // todo mejorar

            throw new Exception("incorrect input" + input.Substring(Pos));
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