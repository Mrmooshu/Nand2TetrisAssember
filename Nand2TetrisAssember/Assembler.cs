using System;
using System.IO;
using System.Collections.Generic;

namespace Nand2TetrisAssember
{
    class Assembler
    {
        public static SymbolTable symbols = new SymbolTable();
        public static String inFilePath = "";
        public static int variableCount = 0;
        public static int instructionCount = -1;

        public static String intTo16BitBinary(int num)
        {
            String value = Convert.ToString(num, 2);
            value = new String('0', 16- value.Length) + value;
            return value;
        }

        public static void preDefinedSymbols(SymbolTable table)
        {
            String[] symbols = new string[] {"R0","R1","R2","R3","R4","R5","R6","R7","R8","R9","R10","R11","R12","R13","R14","R15","SCREEN","KBD","SP","LCL","ARG","THIS","THAT"};
            int[] values = new int[] {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16384,24576,0,1,2,3,4};
            for (int i=0;i<symbols.Length;i++)
            {
                table.addEntry(symbols[i],values[i]);
            }
        }
        public static void Main(String[] args)
        {
            preDefinedSymbols(symbols);
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
                while (parse.hasMoreCommands())//removes comments and adds labels in first scan
                {
                    parse.advance();
                    if (parse.currentCommand.Contains("//"))
                    {
                        if (parse.currentCommand.StartsWith("//"))
                        {
                            parse.lines[parse.currentCount] = "";
                        }
                        else
                        {
                            parse.lines[parse.currentCount] = parse.lines[parse.currentCount].Substring(0, parse.lines[parse.currentCount].IndexOf("//"));
                        }
                    }
                    if (parse.commandType() == types.C_COMMAND && parse.lines[parse.currentCount].Trim() != "")
                    {
                        instructionCount++;
                    }
                    else if (parse.commandType() == types.A_COMMAND)
                    {
                        instructionCount++;
                    }
                    else if (parse.commandType() == types.L_COMMAND)
                    {
                        symbols.addEntry(parse.symbol(), instructionCount+1);
                    }
                }

                LinkedList<String> outFileLines = new LinkedList<string>();
                instructionCount = -1;
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
                        instructionCount++;
                        outFileLines.AddLast("111" + Code.comp(parse.Comp()) + Code.dest(parse.dest()) + Code.jump(parse.jump()));
                    }
                    else if (parse.commandType() == types.A_COMMAND)
                    {
                        if (int.TryParse(parse.symbol(), out int value))
                        {
                            outFileLines.AddLast(intTo16BitBinary(Convert.ToInt16(parse.symbol())));
                        }
                        else
                        {
                            if (symbols.contains(parse.symbol()))
                            {
                                outFileLines.AddLast(intTo16BitBinary(symbols.getAddress(parse.symbol())));
                            }
                            else
                            {
                                symbols.addEntry(parse.symbol(), 16 + variableCount);
                                outFileLines.AddLast(intTo16BitBinary(16 + variableCount++));
                            }
                        }  
                    }
                }
                File.WriteAllLines(outFilePath, outFileLines);//write our binary into a hack file
            }
        }
    }
}
