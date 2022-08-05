using System;
using System.Collections.Generic;
using System.Text;
public class Boolfuck
{
    public static string interpret(string code, string input)
    {
        //List of all jumps saved before execution even starts
        //to avoid searching pairs during execution
        Dictionary<int, int> borders = new Dictionary<int, int>();
        Stack<int> partial = new Stack<int>();

        //this is simple bracket map generation that will give us map
        //that will speed up jumps
        for (int i = 0; i < code.Length; i++)
        {
            if (code[i] == '[')
            {
                partial.Push(i);
            }
            else if (code[i] == ']')
            {
                int opening = partial.Pop();
                borders.Add(i, opening);
                borders.Add(opening, i);
            }
        }
        List<int> result = new List<int>();
        List<bool> tape = new List<bool>(new bool[3000]);
        //put it in the middle i guess?
        //task is kinda confusing ngl
        int ptr = tape.Count / 2;
        int opPtr = 0;

        //which letter of the input are we reading
        int inputChunckId = 0;
        //which bit of the chunk we read last
        int inputChunkBit = 0;

        //which letter of the out are we writing to
        int outChunk = 0;
        //which bit of the out letter are we writing to
        int outChunkBit = 0;

        while (opPtr < code.Length)
        {
            switch (code[opPtr])
            {
                case '+':
                    tape[ptr] = !tape[ptr];
                    break;
                case ',':
                    break;
                case ';':
                    if (outChunkBit == 0)
                    {
                        result.Add(0);
                    }
                    result[outChunk] = (int)result[outChunk] | ((tape[ptr] ? 1 : 0) << outChunkBit);
                    if (++outChunkBit >= 8)
                    {
                        outChunk++;
                        outChunkBit = 0;
                    }
                    break;
                case '>':
                    ptr++;
                    break;
                case '<':
                    ptr--;
                    break;
                case '[':
                    if (!tape[ptr])
                    {
                        opPtr = borders[opPtr];
                    }
                    break;
                case ']':
                    if (tape[ptr])
                    {
                        opPtr = borders[opPtr];
                    }
                    break;
            }
            opPtr++;
        }

        for (int i = 0; i < result.Count; i++)
        {
            System.Console.WriteLine(result[i]);
        }
        string str = string.Empty;
        foreach (int val in result)
        {
            str += (char)val;
        }
        return str;
    }

    public static void Main(string[] args)
    {
        string result = interpret(";", "");
        string str = "\u0000";
        //num & (1 << i) >> i read ith bit of the number
        int num = (int)'a';
        for (int i = 0; i < 8; i++)
        {
            //  System.Console.WriteLine($"{(num & (1 << i)) >> i}");
        }
        //System.Console.WriteLine(result[0] == '\0');
        return;
    }
}