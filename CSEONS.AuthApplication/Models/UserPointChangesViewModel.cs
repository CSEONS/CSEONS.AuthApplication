using CSEONS.AuthApplication.Service;

namespace CSEONS.AuthApplication.Models
{
    public class UserPointChanges
    {
        public string Id { get; set; }
        public PointOperations? Operation { get; set; }
        public int? Value { get; set; }

        public enum PointOperations
        {
            Add,
            Remove,
            Assign
        }
    }
}
