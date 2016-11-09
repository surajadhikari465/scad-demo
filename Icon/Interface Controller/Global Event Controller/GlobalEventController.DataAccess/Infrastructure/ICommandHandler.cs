﻿
namespace GlobalEventController.DataAccess.Infrastructure
{
    public interface ICommandHandler<TCommand>
    {
        void Handle(TCommand command);
    }
}
