namespace Entity
{
    public class Node
    {
        public string id { get; set; }
        public string? label { get; set; }
        public List<Node> children { get; set; }

        public Node()
        {
            id = "";
            children = new List<Node>();
        }
    }
}
