using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BrainfuckRunner.Library.Executors;
using BrainfuckRunner.Library.Validation;

namespace BrainfuckRunner.Library
{
    /// <summary>
    /// Validation engine to test Brainfuck code for validity
    /// </summary>
    internal static class BfValidator
    {
        private static readonly string EscapedCommandSet;

        static BfValidator()
        {
            EscapedCommandSet = string.Concat(
                BfParser.CommandSet
                    .Select(cmd => $@"\{cmd}")
            );
        }

        internal static BfValidateResult Validate(char[] code, BfValidateTolerance tolerance)
        {
            var result = new BfValidateResult();

            var plainText = new StringBuilder();
            var openedLoops = new Stack<BfLoop>();

            var errors = result.GetErrorsList();
            var i = 0;

            for (; i < code.Length; i++)
            {
                var ch = code[i];

                if (!BfParser.IsBrainfuckCommand(ch, out var cmd))
                {
                    plainText.Append(ch);
                    continue;
                }

                if (plainText.Length > 0)
                {
                    ValidatePlainContent(
                        plainText.ToString(), 
                        i - plainText.Length, 
                        tolerance, errors);

                    plainText.Clear();
                }

                switch (cmd)
                {
                    case BfCommand.OpenLoop:
                        openedLoops.Push(new BfLoop
                        {
                            StartPosition = i,
                            EndPosition = -1,
                            ZeroState = default,
                            ScanStep = default,
                            Offsets = default,
                            SubLoops = default
                        });
                        continue;

                    case BfCommand.CloseLoop when !openedLoops.TryPop(out _):
                        errors.Add(
                            new BfValidateError
                            {
                                Code = BfValidateErrorCode.NoOpenLoopToken, 
                                Position = i, 
                                Length = 1, 
                                Content = default
                            });
                        continue;
                }
            }

            if (openedLoops.Count > 0)
            {
               openedLoops
                   .Select(loop => new BfValidateError
                   {
                       Code = BfValidateErrorCode.NoCloseLoopToken,
                       Position = loop.StartPosition,
                       Length = 1,
                       Content = default
                   }).ToList()
                   .ForEach(error => errors.Add(error));
            }

            if (plainText.Length > 0)
            {
                ValidatePlainContent(
                    plainText.ToString(),
                    i - plainText.Length,
                    tolerance, errors);
            }

            openedLoops.Clear();
            plainText.Clear();

            return result;
        }

        /// <summary>
        /// Validates a plain text literal against tolerance policy
        /// </summary>
        private static void ValidatePlainContent(
            string content, int pos, 
            BfValidateTolerance tolerance, 
            SortedSet<BfValidateError> errors)
        {
            if (!tolerance.HasFlag(BfValidateTolerance.ToNonBrainfuckContent))
            {
                // no tolerance to non-Brainfuck content
                var matches = Regex.Matches(content, 
                    $"[^\r\n {EscapedCommandSet}]+", RegexOptions.Compiled);

                foreach (Match match in matches)
                {
                    errors.Add(new BfValidateError
                    {
                        Code = BfValidateErrorCode.NonBrainfuckContent,
                        Position = pos + match.Index,
                        Length = match.Length,
                        Content = match.Value
                    });
                }
            }

            if (!tolerance.HasFlag(BfValidateTolerance.ToWhiteSpaceContent))
            {
                // no tolerance to whitespace
                var matches = Regex.Matches(content, "[ ]+", RegexOptions.Compiled);

                foreach (Match match in matches)
                {
                    errors.Add(new BfValidateError
                    {
                        Code = BfValidateErrorCode.WhiteSpaceContent,
                        Position = pos + match.Index,
                        Length = match.Length,
                        Content = match.Value
                    });
                }
            }

            if (!tolerance.HasFlag(BfValidateTolerance.ToNewLines))
            {
	            // no tolerance to new lines
	            var matches = Regex.Matches(content, "[\r\n]+", RegexOptions.Compiled);

	            foreach (Match match in matches)
	            {
		            errors.Add(new BfValidateError
		            {
			            Code = BfValidateErrorCode.NewLine,
			            Position = pos + match.Index,
			            Length = match.Length,
			            Content = match.Value
		            });
	            }
            }
        }
    }
}