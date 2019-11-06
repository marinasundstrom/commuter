namespace Commuter.Data
{
    public class StopArea
    {
        public int StopAreaId { get; set; }
        public string? Name { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public ushort Distance { get; set; }
    }
}
