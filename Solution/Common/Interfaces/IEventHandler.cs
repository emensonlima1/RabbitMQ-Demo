namespace Common.Interfaces;

public interface IEventHandler
{
    Task Handle(string message);
}