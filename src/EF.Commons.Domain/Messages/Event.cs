using MediatR;

namespace EF.Domain.Commons.Messages;

public abstract class Event : Message, INotification
{
}