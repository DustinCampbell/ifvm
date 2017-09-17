using IFVM.Collections;

namespace IFVM.FlowAnalysis
{
    public partial class ControlFlowGraph
    {
        public class Builder : Builder<Block, Block.Builder>
        {
            protected override Block.Builder CreateBlockBuilder(BlockId id)
            {
                return new Block.Builder(id);
            }
        }
    }
}
