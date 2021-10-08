﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using BrainfuckRunner.Library.Behaviors;
using BrainfuckRunner.Library.Executors;
using BrainfuckRunner.Library.Tokens;
using BrainfuckRunner.Library.Validation;
using Microsoft.Extensions.Options;

namespace BrainfuckRunner.Library
{
    /// <summary>
    /// Interpreting engine of Brainfuck language.
    /// Engine is able to execute Brainfuck commands, validate Brainfuck code
    /// and analyze semantic structure of Brainfuck code 
    /// </summary>
    public sealed class BfEngine
    {
        #region === STATIC STUFF & CONSTANTS ===
        /// <summary>
        /// Tokenize specified text source into Brainfuck tokens
        /// </summary>
        public static BfToken[] Tokenize(TextReader reader)
        {
            char[] chars = ReadChars(reader).ToArray();
            RootBfToken root = BfTokenizer.Tokenize(chars);

            return root.Children.ToArray();
        }

        /// <summary>
        /// Validates Brainfuck code from specified text source using tolerance policy
        /// </summary>
        public static BfValidateResult Validate(TextReader reader, BfValidateTolerance tolerance)
        {
            char[] chars = ReadChars(reader).ToArray();

            return BfValidator.Validate(chars, tolerance);
        }

        /// <summary>
        /// Validates Brainfuck code from specified text source with maximal tolerance policy
        /// </summary>
        public static BfValidateResult Validate(TextReader reader)
        {
            return Validate(reader,
                BfValidateTolerance.ToNonBrainfuckContent | 
                BfValidateTolerance.ToWhiteSpaceContent | 
                BfValidateTolerance.ToNewLines);
        }

        /// <summary>
        /// Validates Brainfuck code from file specified by path using tolerance policy
        /// </summary>
        public static BfValidateResult ValidateFile(string path, BfValidateTolerance tolerance)
        {
            using (StreamReader sr = File.OpenText(path))
            {
                return Validate(sr, tolerance);
            }
        }

        /// <summary>
        /// Validates Brainfuck code from file specified by path using maximal tolerance policy 
        /// </summary>
        public static BfValidateResult ValidateFile(string path)
        {
            return ValidateFile(path,
                BfValidateTolerance.ToNonBrainfuckContent |
                BfValidateTolerance.ToWhiteSpaceContent |
                BfValidateTolerance.ToNewLines);
        }

        /// <summary>
        /// Validates code from specified Brainfuck script using tolerance policy
        /// </summary>
        public static BfValidateResult ValidateScript(string script, BfValidateTolerance tolerance)
        {
            using (StringReader sr = new StringReader(script))
            {
                return Validate(sr, tolerance);
            }
        }

        /// <summary>
        /// Validates code from specified Brainfuck script using maximal tolerance policy
        /// </summary>
        public static BfValidateResult ValidateScript(string script)
        {
            return ValidateScript(script,
                BfValidateTolerance.ToNonBrainfuckContent | 
                BfValidateTolerance.ToWhiteSpaceContent | 
                BfValidateTolerance.ToNewLines);
        }

        /// <summary>
        /// Tokenize Brainfuck code from file specified by path 
        /// </summary>
        public static BfToken[] TokenizeFile(string path)
        {
            using (StreamReader sr = File.OpenText(path))
            {
                return Tokenize(sr);
            }
        }

        /// <summary>
        /// Tokenize specified script into Brainfuck tokens
        /// </summary>
        public static BfToken[] TokenizeScript(string script)
        {
            using (StringReader sr = new StringReader(script))
            {
                return Tokenize(sr);
            }
        }
        
        /// <summary>
        /// Reads and enumerates characters from specified text source
        /// </summary>
        private static IEnumerable<char> ReadChars(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            int nextChar;
            while ((nextChar = reader.Read()) != BfParser.Eof)
            {
                yield return (char) nextChar;
            }
        }

        private static BfEngineOptions ValidateOptions(IOptions<BfEngineOptions> optionsAccessor)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }

            BfEngineOptions options = optionsAccessor.Value;

            if (options == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor.Value));
            }

            if (options.TapeSize <= 0)
            {
                throw new ArgumentException(
                    "Size of internal memory tape must be a positive integer",
                    nameof(optionsAccessor.Value.TapeSize));
            }

            if (options.Input == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor.Value.Input));
            }

            if (options.Output == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor.Value.Output));
            }

            if (options.CommentTokens is {Count: > 0})
            {
                char[] tokens = options.CommentTokens.ToArray();
                if (tokens.Any(ch => BfParser.IsBrainfuckCommand(ch, out _)))
                {
                    throw new ArgumentException(
                        "At least a single comment token is a valid Brainfuck command",
                        nameof(optionsAccessor.Value.CommentTokens));
                }
            } 

            return options;
        }
        #endregion

        private int _ptr;
        private BfCommand[] _commands;
        private readonly byte[] _cells;
        private readonly BfCellOverflowBehavior _onCellOverflow;
        private readonly BfMemoryOverflowBehavior _onMemoryOverflow;
        private readonly bool _isOptimized;
        private readonly TextReader _input;
        private readonly TextWriter _output;
        private readonly char[] _commentTokens;

        public BfEngine(IOptions<BfEngineOptions> optionsAccessor)
        {
            BfEngineOptions options = ValidateOptions(optionsAccessor);

            _cells = new byte[options.TapeSize];
            _input = options.Input;
            _output = options.Output;
            _isOptimized = options.UseOptimizedExecutor;
            _onCellOverflow = options.OnCellOverflow;
            _onMemoryOverflow = options.OnMemoryOverflow;

            if (options.CommentTokens is {Count: > 0})
            {
                _commentTokens = options.CommentTokens.ToArray();
            }
        }

        public BfEngine() : this(new BfEngineOptions())
        {
        }

        /// <summary>
        /// Returns a boolean value whether this engine uses optimized execution logic
        /// </summary>
        public bool IsOptimized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _isOptimized;
            }
        }


        /// <summary>
        /// Gets or sets writer to render Brainfuck PRINT commands
        /// </summary>
        public TextWriter Output
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _output;
            }
        }

        /// <summary>
        /// Gets or sets textSource to process Brainfuck READ commands
        /// </summary>
        public TextReader Input
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _input;
            }
        }

        /// <summary>
        /// Gets or sets behavior to apply in case if value of cell is about to overflow
        /// </summary>
        public BfCellOverflowBehavior OnCellOverflow
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _onCellOverflow;
            }
        }

        /// <summary>
        /// Gets or sets behavior to apply when program pointer goes out
        /// of internal tape's memory range
        /// </summary>
        public BfMemoryOverflowBehavior OnMemoryOverflow
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _onMemoryOverflow;
            }
        }

        /// <summary>
        /// Gets current position of the memory pointer
        /// within internal tape
        /// </summary>
        public int Pointer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _ptr;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal set
            {
                _ptr = value;
            }
        }

        /// <summary>
        /// Returns size of internal tape
        /// </summary>
        public int TapeSize
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _cells.Length;
            }
        }

        /// <summary>
        /// Executes all Brainfuck commands from specified text source
        /// </summary>
        public TimeSpan Execute(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            Reset();
            ReadBrainfuckCommands(reader);

            return ExecuteCore();
        }

        /// <summary>
        /// Executes all Brainfuck commands parsed from file specified by path
        /// </summary>
        public TimeSpan ExecuteFile(string path)
        {
            using (StreamReader sr = File.OpenText(path))
            {
                Reset();
                ReadBrainfuckCommands(sr);
            }

            return ExecuteCore();
        }

        /// <summary>
        /// Executes all Brainfuck commands from specified script
        /// </summary>
        public TimeSpan ExecuteScript(string script)
        {
            using (StringReader sr = new StringReader(script))
            {
                return Execute(sr);
            }
        }

        private void ReadBrainfuckCommands(TextReader text)
        {
            int loops = 0;
            BfCommand cmd;
            BfParser parser = new BfParser(text, _commentTokens);
            List<BfCommand> commands = new List<BfCommand>();

            while ((cmd = parser.ParseNextCommand()) != BfCommand.Eof)
            {
                commands.Add(cmd);
                cmd.TryChangeLoopsRef(ref loops);
            }

            _commands = commands.ToArray();

            if (loops != 0)
            {
                if (loops > 0)
                {
                    throw new BfException(
                        BfRuntimeError.HasUnclosedLoops,
                        $"Unclosed loop(s) encountered. Count of unclosed loops: {loops}");
                }

                throw new BfException(
                    BfRuntimeError.HasUnopenedLoops,
                    $"Unopened loop(s) encountered. Count of unopened loops: {loops}");
            }
        }

        private void Reset()
        {
            _ptr = 0;
            _commands = null;
            Array.Clear(_cells, 0, _cells.Length);
        }

        /// <summary>
        /// Main execution loop
        /// </summary>
        private TimeSpan ExecuteCore()
        {
            BfExecutor executor = BfExecutor.CreateInstance(this);
            executor.Initialize();

            // need local variables here to increase execution speed
            BfCommand cmd;
            BfCommand[] commands = _commands;
            int iNextCmd = 0, iEndCmd = commands.Length;

            Stopwatch stopwatch = Stopwatch.StartNew();

            while (iNextCmd < iEndCmd)
            {
                cmd = commands[iNextCmd];
                executor.RunCommand(cmd, ref iNextCmd);
            }

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        /// <summary>
        /// Returns tape of cells for internal use
        /// </summary>
        internal byte[] Cells
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _cells;
            }
        }

        /// <summary>
        /// Returns list of parsed Brainfuck commands for internal use
        /// </summary>
        internal BfCommand[] Commands
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _commands;
            }
        }

        /// <summary>
        /// Returns current position of a pointer along with recently parsed Brainfuck commands
        /// and size of tape as a single method call
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal (int Pointer, BfCommand[] Commands, int TapeSize) GetBaseTuple()
        {
            return (_ptr, _commands, _cells.Length);
        }

        /// <summary>
        /// Returns current position of a pointer along with recently parsed commands and tape of cells
        /// as a single method call
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal (int Pointer, BfCommand[] Commands, byte[] Cells) GetCellsTuple()
        {
            return (_ptr, _commands, _cells);
        }

        /// <summary>
        /// Returns current position of a pointer along with tape of cells
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal (int Pointer, byte[] Cells) GetPointerCellsTuple()
        {
            return (_ptr, _cells);
        }
    }
}