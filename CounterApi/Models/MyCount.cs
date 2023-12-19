namespace CounterApi.Models
{
    public class MyCount
    {
        public int Id { get; set; } // note only id 1 allowed.
        public int GetCount { get; set; }
        public int PostCount { get; set; }
        public int PutCount { get; set; }
        public int DeleteCount { get; set; }
    }
}
