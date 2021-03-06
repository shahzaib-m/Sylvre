﻿using System.Collections.Generic;

using static SylvreParser;

namespace Sylvre.Core.Models
{
    /// <summary>
    /// Represents a Sylvre program generated by parsing an input.
    /// </summary>
    public class SylvreProgram
    {
        /// <summary>
        /// The root of a Sylvre program concrete syntax tree generated by the parser.
        /// </summary>
        public ProgramContext SylvreCstRoot { get; private set; }
        
        /// <summary>
        /// The collection of all parse errors that may have been encountered during parsing.
        /// </summary>
        public ICollection<SylvreErrorBase> ParseErrors { get; private set; }
        /// <summary>
        /// Returns true if parse errors were encountered, false otherwise.
        /// </summary>
        public bool HasParseErrors
        {
            get { return ParseErrors.Count != 0; }
        }

        /// <summary>
        /// Initialises a new instance of a SylvreProgram with the given root context and collection of parse errors.
        /// </summary>
        /// <param name="sylvreCstRoot">The root context of the concrete syntax tree generated by the parser.</param>
        /// <param name="parseErrors">The collection of parse errors encountered during parsing.</param>
        public SylvreProgram(ProgramContext sylvreCstRoot, ICollection<SylvreErrorBase> parseErrors)
        {
            SylvreCstRoot = sylvreCstRoot;
            ParseErrors = parseErrors;
        }
    }
}
