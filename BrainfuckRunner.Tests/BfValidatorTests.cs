using System.Collections.Generic;
using System.Linq;
using BrainfuckRunner.Library;
using BrainfuckRunner.Library.Validation;
using Xunit;

namespace BrainfuckRunner.Tests
{ 
    public class BfValidatorTests
    {
        [Fact]
        public void validate_result_should_fail()
        {
            const string script = "   a<b>c+++q.w.e,rty";
            const BfValidateTolerance tolerance = BfValidateTolerance.ToWhiteSpaceContent;

            BfValidateResult vr = BfEngine.ValidateScript(script, tolerance);

            Assert.NotNull(vr);
            Assert.False(vr.IsValid);

            Assert.Collection(vr.Errors,
                error =>
                {
                    Assert.Equal(BfValidateErrorCode.NonBrainfuckContent, error.Code);
                    Assert.Equal(1, error.Length);
                    Assert.Equal(3, error.Position);
                    Assert.Equal("a", error.Content);
                },
                error =>
                {
                    Assert.Equal(BfValidateErrorCode.NonBrainfuckContent, error.Code);
                    Assert.Equal(1, error.Length);
                    Assert.Equal(5, error.Position);
                    Assert.Equal("b", error.Content);
                },
                error =>
                {
                    Assert.Equal(BfValidateErrorCode.NonBrainfuckContent, error.Code);
                    Assert.Equal(1, error.Length);
                    Assert.Equal(7, error.Position);
                    Assert.Equal("c", error.Content);
                },
                error =>
                {
                    Assert.Equal(BfValidateErrorCode.NonBrainfuckContent, error.Code);
                    Assert.Equal(1, error.Length);
                    Assert.Equal(11, error.Position);
                    Assert.Equal("q", error.Content);
                },
                error =>
                {
                    Assert.Equal(BfValidateErrorCode.NonBrainfuckContent, error.Code);
                    Assert.Equal(1, error.Length);
                    Assert.Equal(13, error.Position);
                    Assert.Equal("w", error.Content);
                },
                error =>
                {
                    Assert.Equal(BfValidateErrorCode.NonBrainfuckContent, error.Code);
                    Assert.Equal(1, error.Length);
                    Assert.Equal(15, error.Position);
                    Assert.Equal("e", error.Content);
                },
                error =>
                {
                    Assert.Equal(BfValidateErrorCode.NonBrainfuckContent, error.Code);
                    Assert.Equal(3, error.Length);
                    Assert.Equal(17, error.Position);
                    Assert.Equal("rty", error.Content);
                });
        }

        [Fact]
        public void when_no_tolerance_to_non_brainfuck_content()
        {
            const string script = ">>>> abc .++<[-]++QWERTY";
            BfValidateResult vr;

            vr = BfEngine.ValidateScript(script, BfValidateTolerance.ToWhiteSpaceContent);

            Assert.NotNull(vr);
            Assert.False(vr.IsValid);
            Assert.Collection(vr.Errors,
                error =>
                {
                    Assert.Equal(BfValidateErrorCode.NonBrainfuckContent, error.Code);
                    Assert.Equal(5, error.Position);
                    Assert.Equal(3, error.Length);
                },
                error =>
                {
                    Assert.Equal(BfValidateErrorCode.NonBrainfuckContent, error.Code);
                    Assert.Equal(18, error.Position);
                    Assert.Equal(6, error.Length);
                });
        }

        [Fact]
        public void when_tolerance_to_new_lines_applied_result_result_should_not_be_valid()
        {
            const string script = " >>>++[.>>>+<-]  >>> ";
            BfValidateResult vr;

            vr = BfEngine.ValidateScript(script, BfValidateTolerance.ToNewLines);

            Assert.NotNull(vr);
            Assert.False(vr.IsValid);

            Assert.Collection(vr.Errors,
                error =>
                {
                    Assert.Equal(BfValidateErrorCode.WhiteSpaceContent, error.Code);
                    Assert.Equal(0, error.Position);
                    Assert.Equal(1, error.Length);
                },
                error =>
                {
                    Assert.Equal(BfValidateErrorCode.WhiteSpaceContent, error.Code);
                    Assert.Equal(15, error.Position);
                    Assert.Equal(2, error.Length);
                },
                error =>
                {
                    Assert.Equal(BfValidateErrorCode.WhiteSpaceContent, error.Code);
                    Assert.Equal(20, error.Position);
                    Assert.Equal(1, error.Length);
                });
        }

        [Fact]
        public void when_tolerance_to_white_space_content_applied_result_should_be_valid()
        {
            const string script = "   >>>><++++++-->><. ";
            BfValidateResult vr;

            vr = BfEngine.ValidateScript(script, BfValidateTolerance.ToWhiteSpaceContent);

            Assert.NotNull(vr);
            Assert.Empty(vr.Errors);
            Assert.True(vr.IsValid);
        }

        [Fact]
        public void when_no_tolerance_applied_must_be_white_space_content_errors()
        {
            const string script = " ++++.,   ";
            BfValidateResult vr;

            vr = BfEngine.ValidateScript(script, BfValidateTolerance.None);

            Assert.NotNull(vr);
            Assert.False(vr.IsValid);

            Assert.Collection(vr.Errors,
                error =>
                {
                    Assert.Equal(BfValidateErrorCode.WhiteSpaceContent, error.Code);
                    Assert.Equal(0, error.Position);
                    Assert.Equal(1, error.Length);
                }, error =>
                {
                    Assert.Equal(BfValidateErrorCode.WhiteSpaceContent, error.Code);
                    Assert.Equal(7, error.Position);
                    Assert.Equal(3, error.Length);
                });
        }

        [Fact]
        public void when_no_tolerance_to_newline_chars_validation_result_should_fail()
        {
            const BfValidateTolerance tolerance = BfValidateTolerance.ToNonBrainfuckContent | BfValidateTolerance.ToWhiteSpaceContent;
            const string scriptLF = ">>>  ++[--].[+<+] \n++[-.]";
            const string scriptCR = "+++.[->>>>+]  \r-.[+>>+.]";
            const string scriptCRLF = "++[>>--++<+-.] \r\n+++[-.]++[>>>.]";

            BfValidateResult vr;

            vr = BfEngine.ValidateScript(scriptLF, tolerance);
            Assert.NotNull(vr);
            Assert.False(vr.IsValid);

            vr = BfEngine.ValidateScript(scriptCR, tolerance);
            Assert.NotNull(vr);
            Assert.False(vr.IsValid);

            vr = BfEngine.ValidateScript(scriptCRLF, tolerance);
            Assert.NotNull(vr);
            Assert.False(vr.IsValid);
        }
    }
}