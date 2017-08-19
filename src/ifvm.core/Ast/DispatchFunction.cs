namespace IFVM.Ast
{
    public struct DispatchFunction
    {
        public int Id { get; }
        public string Name { get; }

        public DispatchFunction(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
