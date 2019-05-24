using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NexGenIpos.Scheduler.Util
{
    /// <summary>
    /// Allows you to iterate over any text file with a given delimter (ie csv file delimited with a comma)
    /// </summary>
    public class DelimitedTextFileParser : IEnumerable<string[]>
    {
        private readonly string _fileName;
        private readonly char _delimiter;
        private readonly bool _ignoreHeader;

        /// <summary>
        /// Gets the total size of the file being parsed (in characters)
        /// </summary>
        /// <value>The total size.</value>
        public long TotalSize { get; private set; }

        /// <summary>
        /// Gets the total number characters that have been iterated over.
        /// </summary>
        /// <value>The total completed.</value>
        public long TotalCompleted { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelimitedTextFileParser"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="delimiter">The delimiter.</param>
        public DelimitedTextFileParser(string fileName, char delimiter)
        : this(fileName, delimiter, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelimitedTextFileParser"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="ignoreHeader">if set to <c>true</c> [ignore header row].</param>
        public DelimitedTextFileParser(string fileName, char delimiter, bool ignoreHeader)
        {
            _fileName = fileName;
            _delimiter = delimiter;
            _ignoreHeader = ignoreHeader;

            var fileInfo = new FileInfo(_fileName);

            if (!fileInfo.Exists) {
                throw (new InvalidOperationException(string.Format("Source File [{0}] does not exist", _fileName)));
            }

            TotalSize = fileInfo.Length;

            if (TotalSize == 0) {
                throw (new InvalidOperationException(string.Format("Source File [{0}] has no data", _fileName)));
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the file line by line, returning a string array.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<string[]> GetEnumerator()
        {
            using (var reader = new StreamReader(_fileName))
            {
                if (_ignoreHeader)
                {
                    var headerLine = reader.ReadLine();
                    TotalCompleted += headerLine.Length + 2;
                }

                var line = new List<string>();

                while (reader.Peek() != -1)
                {
                    foreach (string s in parseLine(reader))
                    {
                        line.Add(s);
                    }

                    yield return line.ToArray();
                    line = new List<string>();
                }
            }
        }

        private IEnumerable<string> parseLine(TextReader reader)
        {
            var insideQuotes = false;
            var item = new StringBuilder();

            while (reader.Peek() != -1)
            {
                var ch = (char)reader.Read();
                char? nextCh = reader.Peek() > -1 ? (char)reader.Peek() : (char?)null;

                if (!insideQuotes && ch == _delimiter)
                {
                    yield return item.ToString();
                    item.Length = 0;
                }
                else
                    if (!insideQuotes && ch == '\r' && nextCh == '\n') //CRLF
                    {
                        reader.Read(); // skip LF
                        break;
                    }
                    else
                        if (!insideQuotes && ch == '\n') { //LF for *nix-style line endings
                            break;
                        }
                        else
                            if (ch == '"' && nextCh == '"') // escaped quotes ""
                            {
                                item.Append('"');
                                reader.Read(); // skip next "
                            }
                            else
                                if (ch == '"') {
                                    insideQuotes = !insideQuotes;
                                }
                                else {
                                    item.Append(ch);
                                }
            }

            // last one
            yield return item.ToString();
        }
    }
}