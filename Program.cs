/**
Bool fuck esoteric language interpreter 
Originally written for Esolang Interpreters #4 - Boolfuck Interpreter kata on Code Wars

https://www.codewars.com/kata/esolang-interpreters-number-4-boolfuck-interpreter
*/
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
        //we store result as int list for simplicity 
        List<int> result = new List<int>();
        
        List<bool> tape = new List<bool>(new bool[50]);
        //put it in the middle i guess?
        //task is kinda confusing ngl
        int ptr = tape.Count / 2;
        //Operation pointer(index of currently executed operation)
        int opPtr = 0;

        //which letter of the input are we reading
        int inChunk = 0;
        //which bit of the chunk we read last
        int inChunkBit = 0;

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
                    //if we reached end of input we need to put 0s
                    if (inChunk >= input.Length)
                    {
                        tape[ptr] = false;
                    }
                    else
                    {
                        //this is a bit of bit math
                        //to get specific bit we first create a mask by shifting 1 to needed position
                        // which will make other positions to be filled with 0s
                        tape[ptr] = ((input[inChunk] & (1 << inChunkBit)) >> inChunkBit) == 1;
                        //if we read more then 8 bits we more to the next byte
                        if (++inChunkBit >= 8)
                        {
                            inChunk++;
                            inChunkBit = 0;
                        }
                    }
                    break;
                case ';':
                    //if chunk bit is 0 that means we haven't put number in yet
                    if (outChunkBit == 0)
                    {
                        result.Add(0);
                    }
                    //take value and shift it into needed position
                    result[outChunk] = (int)result[outChunk] | ((tape[ptr] ? 1 : 0) << outChunkBit);
                    //if we reached 8 we filled out our chunk
                    if (++outChunkBit >= 8)
                    {
                        outChunk++;
                        outChunkBit = 0;
                    }
                    break;
                case '>':
                    ptr++;
                    //expanding behavior
                    //just add more data to the end
                    if (ptr >= tape.Count)
                    {
                        tape.AddRange(new bool[50]);
                    }
                    break;
                case '<':
                    ptr--;
                    //expanding behavior
                    //just add more data in the front
                    //and update pointer
                    if (ptr < 0)
                    {
                        tape.InsertRange(0, new bool[50]);
                        ptr += 50;
                    }
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
        
        string str = string.Empty;
        foreach (int val in result)
        {
            str += (char)val;
        }
        return str;
    }

    public static void Main(string[] args)
    {
        //idk do some calls
        return;
    }
}