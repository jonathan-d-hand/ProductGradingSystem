namespace ProductGradingTool.Model;



public class ProductScan
{
    public string ProductId { get; set; }
    public string UserId { get; set; }
    public DateTime TimeStamp { get; set; }
    public GradeScan Grade { get; set; }
    public string Weight { get; set; }
    public bool SetInvalid { get; set; } = false;
}

public class GradeScan
{
    public string FatValue { get; set; }
    public string LeanValue { get; set; }
    public string Grade { get; set; }
}