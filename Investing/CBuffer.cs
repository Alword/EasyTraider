using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Investing
{
    public class CBuffer
    {
        private int height;

        public int Height
        {
            get { return height; }
            //set { height = value; }
        }

        private int width;

        public int Width
        {
            get { return width; }
            //set { width = value; }
        }

        private char[,] cBuffer;
        private char[,] newBuffer;

        private int line = 0;
        private int column = 0;

        public CBuffer(int height, int width)
        {
            this.height = height;
            this.width = width;
            cBuffer = new char[this.height, this.width];
            newBuffer = (char[,])cBuffer.Clone();
        }

        public void WriteLine(string input = "")
        {
            int i = line;
            int j = column;

            foreach (char x in input)
            {
                newBuffer[i, j] = x;
                j++;
                if (j == width)
                {
                    i++;
                    j = 0;
                }
                if (i == height)
                {
                    RowsUp();
                    i--;
                }
            }
            i++;
            if (i == height)
            {
                RowsUp();
                i--;
            }
            line = i;
            column = 0;
        }

        public void Write(string input)
        {
            int i = line;
            int j = column;

            foreach (char x in input)
            {
                newBuffer[i, j] = x;
                j++;
                if (j == width)
                {
                    i++;
                    j = 0;
                }
                if (i == height)
                {
                    RowsUp();
                    i--;
                }
            }
            line = i;
            column = j;

        }

        public void Refresh()
        {
            var x = Console.CursorLeft;
            var y = Console.CursorTop;
            var changes = CommitChanges();
            foreach (var key in changes.Keys)
            {
                Console.CursorTop = key.Item1;
                Console.CursorLeft = key.Item2;
                Console.Write(changes[key]);
            }
            Console.CursorLeft = x;
            Console.CursorTop = y;
        }

        public void Revert()
        {
            line = 0;
            column = 0;
        }

        private Dictionary<Tuple<int, int>, char> CommitChanges()
        {
            Dictionary<Tuple<int, int>, char> result = new Dictionary<Tuple<int, int>, char>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (cBuffer[i, j] != newBuffer[i, j])
                    {
                        result.Add(new Tuple<int, int>(i, j), newBuffer[i, j]);
                        cBuffer[i, j] = newBuffer[i, j];
                    }
                    newBuffer[i, j] = ' ';
                }
            }
            return result;
        }

        private void RowsUp()
        {
            int i = 0;
            for (; i < height - 1; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    cBuffer[i, j] = cBuffer[i + 1, j];
                }
            }
            for (int j = 0; j < width; j++)
            {
                cBuffer[i, j] = ' ';
            };
        }
    }
}