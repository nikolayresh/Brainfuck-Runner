using System.Collections.Generic;
using System.Text;
using BrainfuckRunner.Library.Tokens;

namespace BrainfuckRunner.Library
{
    /// <summary>
    /// Tokenizer engine of Brainfuck code
    /// </summary>
    internal static class BfTokenizer
    { 
        internal static RootBfToken Tokenize(char[] code)
        {
            var root = new RootBfToken();

            var stack = new Stack<TreeBfToken>();
            var plainText = new StringBuilder();
            var i = 0;

            stack.Push(root);

            for (; i < code.Length; i++)
            {
                var ch = code[i];

                if (!BfParser.IsBrainfuckCommand(ch, out var cmd))
                {
                    plainText.Append(ch);
                    continue;
                }

                var parent = stack.Peek();

                if (plainText.Length > 0)
                {
                    parent.AcceptChild(new PlainTextBfToken
                    {
                        Text = plainText.ToString(),
                        Position = i - plainText.Length
                    });

                    plainText.Clear();
                }

                var token = BfToken.FromBrainfuckCommand(cmd, i);
                parent.AcceptChild(token);

                switch (cmd)
                {
	                case BfCommand.OpenLoop:
		                stack.Push(token as LoopBfToken);
		                continue;

	                case BfCommand.CloseLoop when !ReferenceEquals(parent, root):
		                stack.Pop();
		                continue;
                }
            }

            if (plainText.Length > 0)
            {
                var parent = stack.Peek();

                parent.AcceptChild(new PlainTextBfToken
                {
                    Text = plainText.ToString(),
                    Position = i - plainText.Length
                });
            }

            plainText.Clear();
            stack.Clear();

            return root;
        }
    }
}