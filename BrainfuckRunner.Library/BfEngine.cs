using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
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
        public static BfValidateResult Validate(TextReader reader, BfTolerance tolerance)
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
                BfTolerance.ToNonBrainfuckContent | 
                BfTolerance.ToWhiteSpaceContent | 
                BfTolerance.ToNewLines);
        }

        /// <summary>
        /// Validates Brainfuck code from file specified by path using tolerance policy
        /// </summary>
        public static BfValidateResult ValidateFile(string path, BfTolerance tolerance)
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
                BfTolerance.ToNonBrainfuckContent |
                BfTolerance.ToWhiteSpaceContent |
                BfTolerance.ToNewLines);
        }

        /// <summary>
        /// Validates code from specified Brainfuck script using tolerance policy
        /// </summary>
        public static BfValidateResult ValidateScript(string script, BfTolerance tolerance)
        {
            using (StringReader sr = new(script))
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
                BfTolerance.ToNonBrainfuckContent | 
                BfTolerance.ToWhiteSpaceContent | 
                BfTolerance.ToNewLines);
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
            using (StringReader sr = new(script))
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

            int ch;
            while ((ch = reader.Read()) != BfParser.Eof)
            {
                yield return (char) ch;
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
                throw new ArgumentException(
                    "Value of options is not provided (null)",
                    nameof(optionsAccessor));
            }

            if (options.TapeSize <= 0)
            {
                throw new ArgumentException(
                    "Size of internal memory tape must be a positive integer",
                    nameof(optionsAccessor));
            }

            if (options.Input == null)
            {
                throw new ArgumentException(
                    "Input is not defined for engine (null)",
                    nameof(optionsAccessor));
            }

            if (options.Output == null)
            {
                throw new ArgumentException(
                    "Output is not defined for engine (null)",
                    nameof(optionsAccessor));
            }

            if (options.CommentToken != null)
            {
                if (options.CommentToken.Length < 1)
                {
                    throw new ArgumentException(
                        "Invalid comment token. It should have a non-zero length",
                        nameof(optionsAccessor));
                }

                string trimmed = options.CommentToken.Trim();
                if (trimmed.Length != options.CommentToken.Length)
                {
                    throw new ArgumentException(
                        "Invalid comment token. It contains whitespace characters",
                        nameof(optionsAccessor));
                }

                if (options.CommentToken.Any(ch => BfParser.IsBrainfuckCommand(ch, out _)))
                {
                    throw new ArgumentException(
                        "Invalid comment token. At least a single character is a valid Brainfuck command",
                        nameof(optionsAccessor));
                }
            } 

            return options;
        }
        #endregion

        private int _ptr;
        private BfCommand[] _commands;
        private TextReader _input;
        private TextWriter _output;

        private readonly BfParser _parser;
        private readonly byte[] _cells;
        private readonly BfCellOverflowBehavior _onCellOverflow;
        private readonly BfMemoryOverflowBehavior _onMemoryOverflow;
        private readonly bool _isOptimized;
        private readonly string _commentToken;

        public BfEngine(IOptions<BfEngineOptions> optionsAccessor)
        {
            BfEngineOptions options = ValidateOptions(optionsAccessor);

            _parser = new BfParser();
            _cells = new byte[options.TapeSize];
            _input = options.Input;
            _output = options.Output;
            _isOptimized = options.UseOptimizedExecutor;
            _onCellOverflow = options.OnCellOverflow;
            _onMemoryOverflow = options.OnMemoryOverflow;
            _commentToken = options.CommentToken;
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

            set
            {
                _output = value ?? throw new ArgumentNullException(nameof(value));
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

            set
            {
                _input = value ?? throw new ArgumentNullException(nameof(value));
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

            ResetState();
            ReadBrainfuckCommands(reader);

            return ExecuteCore();
        }

        public TimeSpan Execute(TextReader reader, TimeSpan timeout)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            ResetState();
            ReadBrainfuckCommands(reader);

            return ExecuteCoreWithTimeout(timeout);
        }

        /// <summary>
        /// Executes all Brainfuck commands parsed from file specified by path
        /// </summary>
        public TimeSpan ExecuteFile(string path)
        {
            using (StreamReader sr = File.OpenText(path))
            {
                ResetState();
                ReadBrainfuckCommands(sr);
            }

            return ExecuteCore();
        }

        public TimeSpan ExecuteFile(string path, TimeSpan timeout)
        {
            using (StreamReader sr = File.OpenText(path))
            {
                ResetState();
                ReadBrainfuckCommands(sr);
            }

            return ExecuteCoreWithTimeout(timeout);
        }

        /// <summary>
        /// Executes all Brainfuck commands from specified script
        /// </summary>
        public TimeSpan ExecuteScript(string script)
        {
            using (StringReader sr = new(script))
            {
                return Execute(sr);
            }
        }

        public TimeSpan ExecuteScript(string script, TimeSpan timeout)
        {
            using (StringReader sr = new(script))
            {
                return Execute(sr, timeout);
            }
        }

        private void ReadBrainfuckCommands(TextReader text)
        {
            _parser.SetState(text, _commentToken);
            BfCommand[] parsedCommands = _parser.ParseCommands();

            EnsureLoops(_parser.LoopState);
            _commands = parsedCommands;
        }

        private static void EnsureLoops(int loopState)
        {
            if (loopState != 0)
            {
                if (loopState > 0)
                {
                    throw new BfException(
                        BfRuntimeError.HasUnclosedLoops,
                        $"Unclosed loop(s) encountered. Count of unclosed loops: {loopState}");
                }

                throw new BfException(
                    BfRuntimeError.HasUnopenedLoops,
                    $"Unopened loop(s) encountered. Count of unopened loops: {loopState}");
            }
        }

        private void ResetState()
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

            // need local variables here to gain a faster access to Brainfuck commands
            BfCommand cmd;
            Span<BfCommand> commands = _commands;
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

        private TimeSpan ExecuteCoreWithTimeout(TimeSpan timeout)
        {
            if (timeout < TimeSpan.Zero)
            {
                throw new ArgumentException(
                    "Value of timeout cannot be negative",
                    nameof(timeout));
            }

            if (timeout == TimeSpan.Zero)
            {
                // as timeout was not specified
                return ExecuteCore();
            }

            object isRunning = new();
            CancellationTokenSource cts = new();
            CancellationToken ct = cts.Token;

            // ReSharper disable once MethodSupportsCancellation
            Task<TimeSpan> execTask = Task.Run(() =>
            {
                BfExecutor executor = BfExecutor.CreateInstance(this);
                executor.Initialize();

                // need local variables here to gain a faster access to Brainfuck commands
                BfCommand cmd;
                Span<BfCommand> commands = _commands;
                int iNextCmd = 0, iEndCmd = commands.Length;

                lock (isRunning)
                {
                    // notify main thread
                    Monitor.Pulse(isRunning);
                }

                Stopwatch stopwatch = Stopwatch.StartNew();

                while (iNextCmd < iEndCmd && !ct.IsCancellationRequested)
                {
                    cmd = commands[iNextCmd];
                    executor.RunCommand(cmd, ref iNextCmd);
                }

                stopwatch.Stop();
                return stopwatch.Elapsed;
            });

            lock (isRunning)
            {
                // wait until executor completes initialization
                // before actual execution
                Monitor.Wait(isRunning);
            }

            if (!execTask.Wait(timeout) || !execTask.IsCompletedSuccessfully)
            {
                cts.Cancel();
                throw new TimeoutException($"Timeout! Failed to execute code successfully within duration of {timeout:g}");
            }

            return execTask.Result;
        }

        /// <summary>
        /// Gets current position of the memory pointer
        /// within internal tape
        /// </summary>
        internal int Pointer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _ptr;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _ptr = value;
            }
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