using System;
using System.IO;
using System.Collections.Generic;

namespace Nand2TetrisAssember
{
    class Assembler
    {
        public static String intTo16BitBinary(int num)
        {
            String value = Convert.ToString(num, 2);
            value = new String('0', 16- value.Length) + value;
            return value;
        }
        public static void Main(String[] args)
        {
            SymbolTable symbols = new SymbolTable();
            String inFilePath = "";
            if (args.Length > 0)
            {
                inFilePath = args[0];
            }
            String outFilePath = inFilePath.Substring(0, inFilePath.LastIndexOf('.')) + ".hack";
            if (!File.Exists(inFilePath))
            {
                Console.WriteLine("file not found");
            }
            else
            {
                Parser parse = new Parser(inFilePath);
                while (parse.hasMoreCommands())//looks for comments and removes them in first scan
                {
                    parse.advance();
                    if (parse.currentCommand.Contains("//"))
                    {
                        parse.lines[parse.currentCount] = parse.lines[parse.currentCount].Substring(0, parse.lines[parse.currentCount].IndexOf("//"));
                    }
                }

                LinkedList<String> outFileLines = new LinkedList<string>();
                parse.currentCount = -1; //sets counter to start positon for second scan
                while (parse.hasMoreCommands())
                {
                    parse.advance();
                    while (parse.currentCommand.Trim() == "" || parse.currentCommand == null)//skips empty lines or lines with only spaces
                    {
                        parse.advance();
                    }

                    if (parse.commandType() == types.C_COMMAND)
                    {
                        String line = "111" + Code.comp(parse.Comp()) + Code.dest(parse.dest()) + Code.jump(parse.jump());
                        outFileLines.AddLast(line);
                    }
                    else if (parse.commandType() == types.A_COMMAND || parse.commandType() == types.L_COMMAND)
                    {
                        String line = intTo16BitBinary(Convert.ToInt16(parse.symbol()));
                        outFileLines.AddLast(line);
                    }
                }
                File.WriteAllLines(outFilePath, outFileLines);//write our binary into a hack file
            }
        }
    }
}
