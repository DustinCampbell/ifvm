using System;
using Xunit;

namespace IFVM.Core.Tests
{
    public class StackTests
    {
        [Fact]
        public void create_with_less_than_zero_fails()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Stack(-1));
        }

        [Fact]
        public void size_equals_the_size_that_the_stack_was_created_with()
        {
            var stack = new Stack(1024);
            Assert.Equal(1024, stack.Size);
        }

        [Fact]
        public void pointer_is_zero_for_new_stack()
        {
            var stack = new Stack(1024);
            Assert.Equal(0, stack.Pointer);
        }

        [Fact]
        public void pointer_increments_by_one_for_byte_push()
        {
            var stack = new Stack(1024);
            stack.PushByte(42);
            Assert.Equal(1, stack.Pointer);
        }

        [Fact]
        public void pointer_increments_by_two_for_word_push()
        {
            var stack = new Stack(1024);
            stack.PushWord(42);
            Assert.Equal(2, stack.Pointer);
        }

        [Fact]
        public void pointer_increments_by_four_for_dword_push()
        {
            var stack = new Stack(1024);
            stack.PushDWord(42);
            Assert.Equal(4, stack.Pointer);
        }

        [Fact]
        public void pointer_decrements_by_one_for_byte_pop()
        {
            const int StackSize = 1024;
            var stack = new Stack(StackSize);
            for (int i = 0; i < StackSize; i++)
            {
                stack.PushByte(42);
            }

            Assert.Equal(StackSize, stack.Pointer);

            var value = stack.PopByte();
            Assert.Equal(42, value);

            Assert.Equal(StackSize - sizeof(byte), stack.Pointer);
        }

        [Fact]
        public void pointer_decrements_by_two_for_word_pop()
        {
            const int StackSize = 1024;
            var stack = new Stack(StackSize);
            for (int i = 0; i < StackSize; i += sizeof(ushort))
            {
                stack.PushWord(42);
            }

            Assert.Equal(StackSize, stack.Pointer);

            var value = stack.PopWord();
            Assert.Equal(42, value);

            Assert.Equal(StackSize - sizeof(ushort), stack.Pointer);
        }

        [Fact]
        public void pointer_decrements_by_four_for_dword_pop()
        {
            const int StackSize = 1024;
            var stack = new Stack(StackSize);
            for (int i = 0; i < StackSize; i += sizeof(uint))
            {
                stack.PushDWord(42);
            }

            Assert.Equal(StackSize, stack.Pointer);

            var value = stack.PopDWord();
            Assert.Equal(42u, value);

            Assert.Equal(StackSize - sizeof(uint), stack.Pointer);
        }

        [Fact]
        public void pop_byte_with_pointer_equal_to_zero_underflows()
        {
            var stack = new Stack(1024);
            Assert.Equal(0, stack.Pointer);

            Assert.Throws<StackUnderflowException>(() => stack.PopByte());
        }

        [Fact]
        public void pop_byte_with_pointer_equal_to_one_does_not_underflow()
        {
            var stack = new Stack(1024);
            stack.PushByte(0x12);
            Assert.Equal(1, stack.Pointer);

            var value = stack.PopByte();
            Assert.Equal(0x12, value);
        }

        [Fact]
        public void pop_word_with_pointer_equal_to_zero_underflows()
        {
            var stack = new Stack(1024);
            Assert.Equal(0, stack.Pointer);

            Assert.Throws<StackUnderflowException>(() => stack.PopWord());
        }

        [Fact]
        public void pop_word_with_pointer_equal_to_one_underflows()
        {
            var stack = new Stack(1024);
            stack.PushByte(0x12);
            Assert.Equal(1, stack.Pointer);

            Assert.Throws<StackUnderflowException>(() => stack.PopWord());
        }

        [Fact]
        public void pop_word_with_pointer_equal_to_two_does_not_underflow()
        {
            var stack = new Stack(1024);
            stack.PushByte(0x12);
            stack.PushByte(0x34);
            Assert.Equal(2, stack.Pointer);

            var value = stack.PopWord();
            Assert.Equal(0x1234, value);
        }

        [Fact]
        public void pop_dword_with_pointer_equal_to_zero_underflows()
        {
            var stack = new Stack(1024);
            Assert.Equal(0, stack.Pointer);

            Assert.Throws<StackUnderflowException>(() => stack.PopDWord());
        }

        [Fact]
        public void pop_dword_with_pointer_equal_to_one_underflows()
        {
            var stack = new Stack(1024);
            stack.PushByte(0x12);
            Assert.Equal(1, stack.Pointer);

            Assert.Throws<StackUnderflowException>(() => stack.PopDWord());
        }

        [Fact]
        public void pop_dword_with_pointer_equal_to_two_underflows()
        {
            var stack = new Stack(1024);
            stack.PushByte(0x12);
            stack.PushByte(0x34);
            Assert.Equal(2, stack.Pointer);

            Assert.Throws<StackUnderflowException>(() => stack.PopDWord());
        }

        [Fact]
        public void pop_dword_with_pointer_equal_to_three_underflows()
        {
            var stack = new Stack(1024);
            stack.PushByte(0x12);
            stack.PushByte(0x34);
            stack.PushByte(0x56);
            Assert.Equal(3, stack.Pointer);

            Assert.Throws<StackUnderflowException>(() => stack.PopDWord());
        }

        [Fact]
        public void pop_dword_with_pointer_equal_to_four_does_not_underflow()
        {
            var stack = new Stack(1024);
            stack.PushByte(0x12);
            stack.PushByte(0x34);
            stack.PushByte(0x56);
            stack.PushByte(0x78);
            Assert.Equal(4, stack.Pointer);

            var value = stack.PopDWord();
            Assert.Equal(0x12345678u, value);
        }

        [Fact]
        public void push_byte_with_pointer_equal_to_size_overflows()
        {
            const int StackSize = 1024;

            var stack = new Stack(StackSize);
            for (int i = 0; i < StackSize; i++)
            {
                stack.PushByte(0x42);
            }

            Assert.Equal(StackSize, stack.Pointer);

            Assert.Throws<StackOverflowException>(() => stack.PushByte(0x42));
        }

        [Fact]
        public void push_byte_with_pointer_equal_to_size_minus_one_does_not_overflow()
        {
            const int StackSize = 1024;

            var stack = new Stack(StackSize);
            for (int i = 0; i < StackSize - 1; i++)
            {
                stack.PushByte(0x42);
            }

            Assert.Equal(StackSize - 1, stack.Pointer);

            stack.PushByte(0x42);
            Assert.Equal(StackSize, stack.Pointer);
        }

        [Fact]
        public void push_word_with_pointer_equal_to_size_overflows()
        {
            const int StackSize = 1024;

            var stack = new Stack(StackSize);
            for (int i = 0; i < StackSize; i++)
            {
                stack.PushByte(0x42);
            }

            Assert.Equal(StackSize, stack.Pointer);

            Assert.Throws<StackOverflowException>(() => stack.PushWord(0x42));
        }

        [Fact]
        public void push_word_with_pointer_equal_to_size_minus_one_overflows()
        {
            const int StackSize = 1024;

            var stack = new Stack(StackSize);
            for (int i = 0; i < StackSize - 1; i++)
            {
                stack.PushByte(0x42);
            }

            Assert.Equal(StackSize - 1, stack.Pointer);

            Assert.Throws<StackOverflowException>(() => stack.PushWord(0x42));
        }

        [Fact]
        public void push_word_with_pointer_equal_to_size_minus_two_does_not_overflow()
        {
            const int StackSize = 1024;

            var stack = new Stack(StackSize);
            for (int i = 0; i < StackSize - 2; i++)
            {
                stack.PushByte(0x42);
            }

            Assert.Equal(StackSize - 2, stack.Pointer);

            stack.PushWord(0x42);
            Assert.Equal(StackSize, stack.Pointer);
        }

        [Fact]
        public void push_dword_with_pointer_equal_to_size_overflows()
        {
            const int StackSize = 1024;

            var stack = new Stack(StackSize);
            for (int i = 0; i < StackSize; i++)
            {
                stack.PushByte(0x42);
            }

            Assert.Equal(StackSize, stack.Pointer);

            Assert.Throws<StackOverflowException>(() => stack.PushDWord(0x42));
        }

        [Fact]
        public void push_dword_with_pointer_equal_to_size_minus_one_overflows()
        {
            const int StackSize = 1024;

            var stack = new Stack(StackSize);
            for (int i = 0; i < StackSize - 1; i++)
            {
                stack.PushByte(0x42);
            }

            Assert.Equal(StackSize - 1, stack.Pointer);

            Assert.Throws<StackOverflowException>(() => stack.PushDWord(0x42));
        }

        [Fact]
        public void push_dword_with_pointer_equal_to_size_minus_two_overflows()
        {
            const int StackSize = 1024;

            var stack = new Stack(StackSize);
            for (int i = 0; i < StackSize - 2; i++)
            {
                stack.PushByte(0x42);
            }

            Assert.Equal(StackSize - 2, stack.Pointer);

            Assert.Throws<StackOverflowException>(() => stack.PushDWord(0x42));
        }

        [Fact]
        public void push_dword_with_pointer_equal_to_size_minus_three_overflows()
        {
            const int StackSize = 1024;

            var stack = new Stack(StackSize);
            for (int i = 0; i < StackSize - 3; i++)
            {
                stack.PushByte(0x42);
            }

            Assert.Equal(StackSize - 3, stack.Pointer);

            Assert.Throws<StackOverflowException>(() => stack.PushDWord(0x42));
        }

        [Fact]
        public void push_dword_with_pointer_equal_to_size_minus_four_does_not_overflow()
        {
            const int StackSize = 1024;

            var stack = new Stack(StackSize);
            for (int i = 0; i < StackSize - 4; i++)
            {
                stack.PushByte(0x42);
            }

            Assert.Equal(StackSize - 4, stack.Pointer);

            stack.PushDWord(0x42);
            Assert.Equal(StackSize, stack.Pointer);
        }
    }
}
