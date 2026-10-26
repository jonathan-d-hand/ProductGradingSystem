namespace ProductGradingTool.WpfClient;

using CommunityToolkit.Mvvm.Messaging.Messages;
using ProductGradingTool.Model;

public class WeightChangedMessage : ValueChangedMessage<string>
{
    public WeightChangedMessage(string weight) : base(weight) { }
}

public class UserIdChangedMessage : ValueChangedMessage<string>
{
    public UserIdChangedMessage(string userId) : base(userId) { }
}

public class ProductIdChangedMessage : ValueChangedMessage<string>
{
    public ProductIdChangedMessage(string productId) : base(productId) { }
}

public class GradeChangedMessage : ValueChangedMessage<GradeScan>
{
    public GradeChangedMessage(GradeScan grade) : base(grade) { }
}

public class SubmitScanMessage : ValueChangedMessage<bool>
{
    public SubmitScanMessage(bool value) : base(value) { }
}

public class ResetTerminalMessage : ValueChangedMessage<bool>
{
    public ResetTerminalMessage(bool value) : base(value) { }
}