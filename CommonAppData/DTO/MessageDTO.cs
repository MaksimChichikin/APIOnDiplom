namespace MyProApiDiplom.CommonAppData.DTO
{
    public class MessageDTO
    {
        
        public int? SenderId { get; set; }
        public int? ReceiverId { get; set; }
        public string? Contents { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
