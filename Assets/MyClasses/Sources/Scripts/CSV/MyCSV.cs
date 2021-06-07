/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyCSV (version 1.5)
 */

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyClasses
{
    public static class MyCSV
    {
        #region ----- Public Method -----

        /// <summary>
        /// Deserialize csv file by row.
        /// </summary>
        /// <param name="charComma">a character to detect a comma</param>
        /// <param name="charQuotationMarks">a character to detect a quatation marks</param>
        /// <param name="charCarriageReturnInCell">a string to detect a line break in cell</param>
        public static Dictionary<string, string[]> DeserializeByRowAndRowName(string input, bool isIgnoreFirstRow = false, char charComma = ',', char charQuotationMarks = '\"', string charCarriageReturnInCell = "\\n")
        {
            Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();

            using (Stream stream = _ConvertStringToStream(input))
            {
                bool isFirstRowRead = !isIgnoreFirstRow;

                StreamReader streamReader = new StreamReader(stream);
                while (!streamReader.EndOfStream)
                {
                    if (!isFirstRowRead)
                    {
                        streamReader.ReadLine();
                        isFirstRowRead = true;
                    }
                    else
                    {
                        List<string> listCell = _DeserializeRowCells(streamReader.ReadLine(), charComma, charQuotationMarks, charCarriageReturnInCell);
                        if (listCell.Count > 0)
                        {
                            string rowName = listCell[0];
                            listCell.RemoveAt(0);
                            dictionary[rowName] = listCell.ToArray();
                        }
                    }
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Deserialize csv file by row.
        /// </summary>
        /// <param name="charComma">a character to detect a comma</param>
        /// <param name="charQuotationMarks">a character to detect a quatation marks</param>
        /// <param name="charCarriageReturnInCell">a string to detect a line break in cell</param>
        public static List<string[]> DeserializeByRow(string input, bool isIgnoreFirstRow = false, char charComma = ',', char charQuotationMarks = '\"', string charCarriageReturnInCell = "\\n")
        {
            List<string[]> listRow = new List<string[]>();

            using (Stream stream = _ConvertStringToStream(input))
            {
                bool isFirstRowRead = !isIgnoreFirstRow;

                StreamReader streamReader = new StreamReader(stream);
                while (!streamReader.EndOfStream)
                {
                    if (!isFirstRowRead)
                    {
                        streamReader.ReadLine();
                        isFirstRowRead = true;
                    }
                    else
                    {
                        listRow.Add(_DeserializeRowCells(streamReader.ReadLine(), charComma, charQuotationMarks, charCarriageReturnInCell).ToArray());
                    }
                }
            }

            return listRow;
        }

        /// <summary>
        /// Deserialize csv file by cell.
        /// </summary>
        /// <param name="charComma">a character to detect a comma</param>
        /// <param name="charQuotationMarks">a character to detect a quatation marks</param>
        /// <param name="charCarriageReturnInCell">a string to detect a line break in cell</param>
        public static List<string> DeserializeByCell(string input, bool isIgnoreFirstRow = false, char charComma = ',', char charQuotationMarks = '\"', string charCarriageReturnInCell = "\\n")
        {
            List<string> listCell = new List<string>();

            using (Stream stream = _ConvertStringToStream(input))
            {
                bool isFirstRowRead = !isIgnoreFirstRow;

                StreamReader streamReader = new StreamReader(stream);
                while (!streamReader.EndOfStream)
                {
                    if (!isFirstRowRead)
                    {
                        streamReader.ReadLine();
                        isFirstRowRead = true;
                    }
                    else
                    {
                        listCell.AddRange(_DeserializeRowCells(streamReader.ReadLine(), charComma, charQuotationMarks, charCarriageReturnInCell).ToArray());
                    }
                }
            }

            return listCell;
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Deserialize all cells in row.
        /// </summary>
        private static List<string> _DeserializeRowCells(string input, char charComma = ',', char charQuotationMarks = '\"', string charCarriageReturnInCell = "\\n")
        {
            List<string> listCell = new List<string>();
            StringBuilder stringBuilder = new StringBuilder();
            bool isQuote = false;
            bool isHasQuotationMarksInCell = false; // '""' represents '"' when exporting CSV file

            char[] characters = input.ToCharArray();
            for(int i = 0; i < characters.Length; ++i)
            {
                char c = characters[i];
                if (c == charComma && !isQuote)
                {
                    listCell.Add(stringBuilder.ToString().Replace(charCarriageReturnInCell, "\n"));
                    stringBuilder.Length = 0;
                }
                else if (c == '\n')
                {
                    isQuote = !isQuote;
                }
                else if (c == charQuotationMarks)
                {
                    if (isQuote && isHasQuotationMarksInCell)
                    {
                        isHasQuotationMarksInCell = false;
                    }
                    else if (isQuote && i < characters.Length - 1 && characters[i + 1] != charComma)
                    {
                        stringBuilder.Append(c);
                        isHasQuotationMarksInCell = true;
                    }
                    else
                    {
                        isQuote = !isQuote;
                        isHasQuotationMarksInCell = false;
                    }
                }
                else
                {
                    stringBuilder.Append(c);
                }
            }

            listCell.Add(stringBuilder.ToString().Replace(charCarriageReturnInCell, "\n"));

            return listCell;
        }

        /// <summary>
        /// Convert string to Stream.
        /// </summary>
        private static Stream _ConvertStringToStream(string content)
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);
            streamWriter.Write(content);
            streamWriter.Flush();
            memoryStream.Position = 0;
            return memoryStream;
        }

        #endregion
    }
}
