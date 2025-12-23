namespace Servino.Domain.Core.RequestAgg.Enum;

public enum RequestStatus
{
    WaitingForExperts, 
    WaitingForSelection, 
    Selected, 
    Started, 
    Done, 
    Paid, 
    Canceled 
}