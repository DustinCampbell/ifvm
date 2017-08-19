using System.Collections.Generic;
using System.Collections.Immutable;

namespace IFVM.Glulx.Functions
{
    public static class Opcodes
    {
        private static readonly Dictionary<uint, OpcodeDescriptor> s_table = new Dictionary<uint, OpcodeDescriptor>();

        public static bool TryGetOpcode(uint number, out OpcodeDescriptor opcode)
        {
            return s_table.TryGetValue(number, out opcode);
        }

        private static OpcodeDescriptor Add(uint number, string name, uint argumentSize, params OperandKind[] operandKinds)
        {
            var descriptor = new OpcodeDescriptor(number, name, argumentSize, operandKinds.ToImmutableList());
            s_table.Add(number, descriptor);

            return descriptor;
        }

        private const OperandKind Load = OperandKind.Load;
        private const OperandKind Store = OperandKind.Store;

        internal const uint op_nop = 0x00;

        internal const uint op_add = 0x10;
        internal const uint op_sub = 0x11;
        internal const uint op_mul = 0x12;
        internal const uint op_div = 0x13;
        internal const uint op_mod = 0x14;
        internal const uint op_neg = 0x15;

        internal const uint op_bitand = 0x18;
        internal const uint op_bitor = 0x19;
        internal const uint op_bitxor = 0x1a;
        internal const uint op_bitnot = 0x1b;
        internal const uint op_shiftl = 0x1c;
        internal const uint op_sshiftr = 0x1d;
        internal const uint op_ushiftr = 0x1e;

        internal const uint op_jump = 0x20;

        internal const uint op_jz = 0x22;
        internal const uint op_jnz = 0x23;
        internal const uint op_jeq = 0x24;
        internal const uint op_jne = 0x25;
        internal const uint op_jlt = 0x26;
        internal const uint op_jge = 0x27;
        internal const uint op_jgt = 0x28;
        internal const uint op_jle = 0x29;
        internal const uint op_jltu = 0x2a;
        internal const uint op_jgeu = 0x2b;
        internal const uint op_jgtu = 0x2c;
        internal const uint op_jleu = 0x2d;

        internal const uint op_call = 0x30;
        internal const uint op_return = 0x31;
        internal const uint op_catch = 0x32;
        internal const uint op_throw = 0x33;
        internal const uint op_tailcall = 0x34;

        internal const uint op_copy = 0x40;
        internal const uint op_copys = 0x41;
        internal const uint op_copyb = 0x42;

        internal const uint op_sexs = 0x44;
        internal const uint op_sexb = 0x45;

        internal const uint op_aload = 0x48;
        internal const uint op_aloads = 0x49;
        internal const uint op_aloadb = 0x4a;
        internal const uint op_aloadbit = 0x4b;
        internal const uint op_astore = 0x4c;
        internal const uint op_astores = 0x4d;
        internal const uint op_astoreb = 0x4e;
        internal const uint op_astorebit = 0x4f;

        internal const uint op_stkcount = 0x50;
        internal const uint op_stkpeek = 0x51;
        internal const uint op_stkswap = 0x52;
        internal const uint op_stkroll = 0x53;
        internal const uint op_stkcopy = 0x54;

        internal const uint op_streamchar = 0x70;
        internal const uint op_streamnum = 0x71;
        internal const uint op_streamstr = 0x72;
        internal const uint op_streamunichar = 0x73;

        internal const uint op_gestault = 0x100;
        internal const uint op_debugtrap = 0x101;
        internal const uint op_getmemsize = 0x102;
        internal const uint op_setmemsize = 0x103;
        internal const uint op_jumpabs = 0x104;

        internal const uint op_random = 0x110;
        internal const uint op_setrandom = 0x111;

        internal const uint op_quit = 0x120;
        internal const uint op_verify = 0x121;
        internal const uint op_restart = 0x122;
        internal const uint op_save = 0x123;
        internal const uint op_restore = 0x124;
        internal const uint op_saveundo = 0x125;
        internal const uint op_restoreundo = 0x126;
        internal const uint op_protect = 0x127;

        internal const uint op_glk = 0x130;

        internal const uint op_getstringtbl = 0x140;
        internal const uint op_setstringtbl = 0x141;

        internal const uint op_getiosys = 0x148;
        internal const uint op_setiosys = 0x149;

        internal const uint op_linearsearch = 0x150;
        internal const uint op_binarysearch = 0x151;
        internal const uint op_linkedsearch = 0x152;

        internal const uint op_callf = 0x160;
        internal const uint op_callfi = 0x161;
        internal const uint op_callfii = 0x162;
        internal const uint op_callfiii = 0x163;

        internal const uint op_mzero = 0x170;
        internal const uint op_mcopy = 0x171;

        internal const uint op_malloc = 0x178;
        internal const uint op_mfree = 0x179;

        internal const uint op_accelfunc = 0x180;
        internal const uint op_accelparam = 0x181;

        internal const uint op_numtof = 0x190;
        internal const uint op_ftonumz = 0x191;
        internal const uint op_ftonumn = 0x192;

        internal const uint op_ceil = 0x198;
        internal const uint op_floor = 0x199;

        internal const uint op_fadd = 0x1a0;
        internal const uint op_fsub = 0x1a1;
        internal const uint op_fmul = 0x1a2;
        internal const uint op_fdiv = 0x1a3;
        internal const uint op_fmod = 0x1a4;

        internal const uint op_sqrt = 0x1a8;
        internal const uint op_exp = 0x1a9;
        internal const uint op_log = 0x1aa;
        internal const uint op_pow = 0x1ab;

        internal const uint op_sin = 0x1b0;
        internal const uint op_cos = 0x1b1;
        internal const uint op_tan = 0x1b2;
        internal const uint op_asin = 0x1b3;
        internal const uint op_acos = 0x1b4;
        internal const uint op_atan = 0x1b5;
        internal const uint op_atan2 = 0x1b6;

        internal const uint op_jfeq = 0x1c0;
        internal const uint op_jfne = 0x1c1;
        internal const uint op_jflt = 0x1c2;
        internal const uint op_jfle = 0x1c3;
        internal const uint op_jfgt = 0x1c4;
        internal const uint op_jfge = 0x1c5;

        internal const uint op_jisnan = 0x1c8;
        internal const uint op_jisinf = 0x1c9;

        public readonly static OpcodeDescriptor nop = Add(op_nop, "nop", 4);

        public readonly static OpcodeDescriptor add = Add(op_add, "add", 4, Load, Load, Store);
        public readonly static OpcodeDescriptor sub = Add(op_sub, "sub", 4, Load, Load, Store);
        public readonly static OpcodeDescriptor mul = Add(op_mul, "mul", 4, Load, Load, Store);
        public readonly static OpcodeDescriptor div = Add(op_div, "div", 4, Load, Load, Store);
        public readonly static OpcodeDescriptor mod = Add(op_mod, "mod", 4, Load, Load, Store);
        public readonly static OpcodeDescriptor neg = Add(op_neg, "neg", 4, Load, Store);

        public readonly static OpcodeDescriptor bitand = Add(op_bitand, "bitand", 4, Load, Load, Store);
        public readonly static OpcodeDescriptor bitor = Add(op_bitor, "bitor", 4, Load, Load, Store);
        public readonly static OpcodeDescriptor bitxor = Add(op_bitxor, "bitxor", 4, Load, Load, Store);
        public readonly static OpcodeDescriptor bitnot = Add(op_bitnot, "bitnot", 4, Load, Store);
        public readonly static OpcodeDescriptor shiftl = Add(op_shiftl, "shiftl", 4, Load, Load, Store);
        public readonly static OpcodeDescriptor sshiftr = Add(op_sshiftr, "sshiftr", 4, Load, Load, Store);
        public readonly static OpcodeDescriptor ushiftr = Add(op_ushiftr, "ushiftr", 4, Load, Load, Store);

        public readonly static OpcodeDescriptor jump = Add(op_jump, "jump", 4, Load);

        public readonly static OpcodeDescriptor jz = Add(op_jz, "jz", 4, Load, Load);
        public readonly static OpcodeDescriptor jnz = Add(op_jnz, "jnz", 4, Load, Load);
        public readonly static OpcodeDescriptor jeq = Add(op_jeq, "jeq", 4, Load, Load, Load);
        public readonly static OpcodeDescriptor jne = Add(op_jne, "jne", 4, Load, Load, Load);
        public readonly static OpcodeDescriptor jlt = Add(op_jlt, "jlt", 4, Load, Load, Load);
        public readonly static OpcodeDescriptor jge = Add(op_jge, "jge", 4, Load, Load, Load);
        public readonly static OpcodeDescriptor jgt = Add(op_jgt, "jgt", 4, Load, Load, Load);
        public readonly static OpcodeDescriptor jle = Add(op_jle, "jle", 4, Load, Load, Load);
        public readonly static OpcodeDescriptor jltu = Add(op_jltu, "jltu", 4, Load, Load, Load);
        public readonly static OpcodeDescriptor jgeu = Add(op_jgeu, "jgeu", 4, Load, Load, Load);
        public readonly static OpcodeDescriptor jgtu = Add(op_jgtu, "jgtu", 4, Load, Load, Load);
        public readonly static OpcodeDescriptor jleu = Add(op_jleu, "jleu", 4, Load, Load, Load);

        public readonly static OpcodeDescriptor call = Add(op_call, "call", 4, Load, Load, Store);
        public readonly static OpcodeDescriptor @return = Add(op_return, "return", 4, Load);
        public readonly static OpcodeDescriptor @catch = Add(op_catch, "catch", 4, Store, Load);
        public readonly static OpcodeDescriptor @throw = Add(op_throw, "throw", 4, Load, Load);
        public readonly static OpcodeDescriptor tailcall = Add(op_tailcall, "tailcall", 4, Load, Load);

        public readonly static OpcodeDescriptor copy = Add(op_copy, "copy", 4, Load, Store);
        public readonly static OpcodeDescriptor copys = Add(op_copys, "copys", 2, Load, Store);
        public readonly static OpcodeDescriptor copyb = Add(op_copyb, "copyb", 1, Load, Store);

        public readonly static OpcodeDescriptor sexs = Add(op_sexs, "sexs", 4, Load, Store);
        public readonly static OpcodeDescriptor sexb = Add(op_sexb, "sexb", 4, Load, Store);

        public readonly static OpcodeDescriptor aload = Add(op_aload, "aload", 4, Load, Load, Store);
        public readonly static OpcodeDescriptor aloads = Add(op_aloads, "aloads", 4, Load, Load, Store);
        public readonly static OpcodeDescriptor aloadb = Add(op_aloadb, "aloadb", 4, Load, Load, Store);
        public readonly static OpcodeDescriptor aloadbit = Add(op_aloadbit, "aloadbit", 4, Load, Load, Store);
        public readonly static OpcodeDescriptor astore = Add(op_astore, "astore", 4, Load, Load, Load);
        public readonly static OpcodeDescriptor astores = Add(op_astores, "astores", 4, Load, Load, Load);
        public readonly static OpcodeDescriptor astoreb = Add(op_astoreb, "astoreb", 4, Load, Load, Load);
        public readonly static OpcodeDescriptor astorebit = Add(op_astorebit, "astorebit", 4, Load, Load, Load);

        public readonly static OpcodeDescriptor stkcount = Add(op_stkcount, "stkcount", 4, Store);
        public readonly static OpcodeDescriptor stkpeek = Add(op_stkpeek, "stkpeek", 4, Load, Store);
        public readonly static OpcodeDescriptor stkswap = Add(op_stkswap, "stkswap", 4);
        public readonly static OpcodeDescriptor stkroll = Add(op_stkroll, "stkroll", 4, Load, Load);
        public readonly static OpcodeDescriptor stkcopy = Add(op_stkcopy, "stkcopy", 4, Load);

        public readonly static OpcodeDescriptor streamchar = Add(op_streamchar, "streamchar", 4, Load);
        public readonly static OpcodeDescriptor streamnum = Add(op_streamnum, "streamnum", 4, Load);
        public readonly static OpcodeDescriptor streamstr = Add(op_streamstr, "streamstr", 4, Load);
        public readonly static OpcodeDescriptor streamunichar = Add(op_streamunichar, "streamunichar", 4, Load);

        public readonly static OpcodeDescriptor gestault = Add(op_gestault, "gestault", 4, Load, Load, Store);
        public readonly static OpcodeDescriptor debugtrap = Add(op_debugtrap, "debugtrap", 4, Load);
        public readonly static OpcodeDescriptor getmemsize = Add(op_getmemsize, "getmemsize", 4, Store);
        public readonly static OpcodeDescriptor setmemsize = Add(op_setmemsize, "setmemsize", 4, Load, Store);
        public readonly static OpcodeDescriptor jumpabs = Add(op_jumpabs, "jumpabs", 4, Load);

        public readonly static OpcodeDescriptor random = Add(op_random, "random", 4, Load, Store);
        public readonly static OpcodeDescriptor setrandom = Add(op_setrandom, "setrandom", 4, Load);

        public readonly static OpcodeDescriptor quit = Add(op_quit, "quit", 4);
        public readonly static OpcodeDescriptor verify = Add(op_verify, "verify", 4, Store);
        public readonly static OpcodeDescriptor restart = Add(op_restart, "restart", 4);
        public readonly static OpcodeDescriptor save = Add(op_save, "save", 4, Load, Store);
        public readonly static OpcodeDescriptor restore = Add(op_restore, "restore", 4, Load, Store);
        public readonly static OpcodeDescriptor saveundo = Add(op_saveundo, "saveundo", 4, Store);
        public readonly static OpcodeDescriptor restoreundo = Add(op_restoreundo, "restoreundo", 4, Store);
        public readonly static OpcodeDescriptor protect = Add(op_protect, "protect", 4, Load, Load);

        public readonly static OpcodeDescriptor glk = Add(op_glk, "glk", 4, Load, Load, Store);

        public readonly static OpcodeDescriptor getstringtbl = Add(op_getstringtbl, "getstringtbl", 4, Store);
        public readonly static OpcodeDescriptor setstringtbl = Add(op_setstringtbl, "setstringtbl", 4, Load);

        public readonly static OpcodeDescriptor getiosys = Add(op_getiosys, "getiosys", 4, Store, Store);
        public readonly static OpcodeDescriptor setiosys = Add(op_setiosys, "setiosys", 4, Load, Load);

        public readonly static OpcodeDescriptor linearsearch = Add(op_linearsearch, "linearsearch", 4, Load, Load, Load, Load, Load, Load, Load, Store);
        public readonly static OpcodeDescriptor binarysearch = Add(op_binarysearch, "binarysearch", 4, Load, Load, Load, Load, Load, Load, Load, Store);
        public readonly static OpcodeDescriptor linkedsearch = Add(op_linkedsearch, "linkedsearch", 4, Load, Load, Load, Load, Load, Load, Store);

        public readonly static OpcodeDescriptor callf = Add(op_callf, "callf", 4, Load, Store);
        public readonly static OpcodeDescriptor callfi = Add(op_callfi, "callfi", 4, Load, Load, Store);
        public readonly static OpcodeDescriptor callfii = Add(op_callfii, "callfii", 4, Load, Load, Load, Store);
        public readonly static OpcodeDescriptor callfiii = Add(op_callfiii, "callfiii", 4, Load, Load, Load, Load, Store);

        public readonly static OpcodeDescriptor mzero = Add(op_mzero, "mzero", 4, Load, Load);
        public readonly static OpcodeDescriptor mcopy = Add(op_mcopy, "mcopy", 4, Load, Load, Load);

        public readonly static OpcodeDescriptor malloc = Add(op_malloc, "malloc", 4, Load, Store);
        public readonly static OpcodeDescriptor mfree = Add(op_mfree, "mfree", 4, Load);

        public readonly static OpcodeDescriptor accelfunc = Add(op_accelfunc, "accelfunc", 4, Load, Load);
        public readonly static OpcodeDescriptor accelparam = Add(op_accelparam, "accelparam", 4, Load, Load);

        public readonly static OpcodeDescriptor numtof = Add(op_numtof, "numtof", 4, Load, Store);
        public readonly static OpcodeDescriptor ftonumz = Add(op_ftonumz, "ftonumz", 4, Load, Store);
        public readonly static OpcodeDescriptor ftonumn = Add(op_ftonumn, "ftonumn", 4, Load, Store);

        public readonly static OpcodeDescriptor ceil = Add(op_ceil, "ceil", 4, Load, Store);
        public readonly static OpcodeDescriptor floor = Add(op_floor, "floor", 4, Load, Store);

        public readonly static OpcodeDescriptor fadd = Add(op_fadd, "fadd", 4, Load, Load, Store);
        public readonly static OpcodeDescriptor fsub = Add(op_fsub, "fsub", 4, Load, Load, Store);
        public readonly static OpcodeDescriptor fmul = Add(op_fmul, "fmul", 4, Load, Load, Store);
        public readonly static OpcodeDescriptor fdiv = Add(op_fdiv, "fdiv", 4, Load, Load, Store);
        public readonly static OpcodeDescriptor fmod = Add(op_fmod, "fmod", 4, Load, Load, Store, Store);

        public readonly static OpcodeDescriptor sqrt = Add(op_sqrt, "sqrt", 4, Load, Store);
        public readonly static OpcodeDescriptor exp = Add(op_exp, "exp", 4, Load, Store);
        public readonly static OpcodeDescriptor log = Add(op_log, "log", 4, Load, Store);
        public readonly static OpcodeDescriptor pow = Add(op_pow, "pow", 4, Load, Load, Store);

        public readonly static OpcodeDescriptor sin = Add(op_sin, "sin", 4, Load, Store);
        public readonly static OpcodeDescriptor cos = Add(op_cos, "cos", 4, Load, Store);
        public readonly static OpcodeDescriptor tan = Add(op_tan, "tan", 4, Load, Store);
        public readonly static OpcodeDescriptor asin = Add(op_asin, "asin", 4, Load, Store);
        public readonly static OpcodeDescriptor acos = Add(op_acos, "acos", 4, Load, Store);
        public readonly static OpcodeDescriptor atan = Add(op_atan, "atan", 4, Load, Store);
        public readonly static OpcodeDescriptor atan2 = Add(op_atan2, "atan2", 4, Load, Load, Store);

        public readonly static OpcodeDescriptor jfeq = Add(op_jfeq, "jfeq", 4, Load, Load, Load, Load);
        public readonly static OpcodeDescriptor jfne = Add(op_jfne, "jfne", 4, Load, Load, Load, Load);
        public readonly static OpcodeDescriptor jflt = Add(op_jflt, "jflt", 4, Load, Load, Load);
        public readonly static OpcodeDescriptor jfle = Add(op_jfle, "jfle", 4, Load, Load, Load);
        public readonly static OpcodeDescriptor jfgt = Add(op_jfgt, "jfgt", 4, Load, Load, Load);
        public readonly static OpcodeDescriptor jfge = Add(op_jfge, "jfge", 4, Load, Load, Load);

        public readonly static OpcodeDescriptor jisnan = Add(op_jisnan, "jisnan", 4, Load, Load);
        public readonly static OpcodeDescriptor jisinf = Add(op_jisinf, "jisinf", 4, Load, Load);
    }
}
