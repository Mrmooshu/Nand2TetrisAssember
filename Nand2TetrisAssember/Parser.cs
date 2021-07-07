using System;
using System.IO;

namespace Nand2TetrisAssember
{
    class Parser
    {
        public String[] lines;
        public String currentCommand;
        public int currentCount;
        public Parser(String filename)
        {
            currentCount = -1;
            lines = File.ReadAllLines(filename);
        }

        public bool hasMoreCommands()
        {
            return currentCount < lines.Length-1;
        }
        public void advance()
        {
            currentCount++;
            currentCommand = lines[currentCount].Trim();
        }

        public types commandType()
        {
            if (currentCommand.StartsWith("@"))
            {
                return types.A_COMMAND;
            }
            else if (currentCommand.StartsWith("(") && currentCommand.EndsWith(")"))
            {
                return types.L_COMMAND;
            }
            else
            {
                return types.C_COMMAND;
            }
        }

        public String symbol()
        {
            if (commandType() == types.A_COMMAND)
            {
                return currentCommand.Substring(1);
            }
            else if (commandType() == types.L_COMMAND)
            {
                return currentCommand.Substring(1, currentCommand.IndexOf(')')-1);
            }
            else
            {
                return null;
            }
        }

        public String dest()
        {
            if (commandType() == types.C_COMMAND)
            {
                int equalIndex = currentCommand.IndexOf("=");
                if (equalIndex != -1)
                {
                    return currentCommand.Substring(0, equalIndex);
                }
            }
            return null;
        }

        public String Comp()
        {
            if (commandType() == types.C_COMMAND)
            {
                int equalIndex = currentCommand.IndexOf("=");
                int semiIndex = currentCommand.IndexOf(";");
                String commandCopy = currentCommand;
                if (equalIndex != -1)
                {
                    commandCopy = commandCopy.Substring(equalIndex + 1);
                }
                if (semiIndex != -1)
                {
                    commandCopy = commandCopy.Substring(0, semiIndex);
                }
                return commandCopy;
            }
            return null;
        }

        public String jump()
        {
            if (commandType() == types.C_COMMAND)
            {
                int semiIndex = currentCommand.IndexOf(";");
                if (semiIndex != -1)
                {
                    return currentCommand.Substring(semiIndex + 1);
                }
            }
            return null;
        }
    }
}
