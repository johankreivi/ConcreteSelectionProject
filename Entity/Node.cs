namespace Entity
{
    public class Node(string id, string fileName)
    {
        public string Id { get; set; } = id;

        public string FileName { get; set; } = fileName;
        public List<Node> Children { get; set; } = new List<Node>();
    }
}
