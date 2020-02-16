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

            //INode arbolDeNodos = parsearEnNodos(readText)
            
            //NodeWriter.Write(arbolDeNodos)
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

public interface INode{
    void Open();
    void Close();
    void Write();
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