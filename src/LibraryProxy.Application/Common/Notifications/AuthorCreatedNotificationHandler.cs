namespace LibraryProxy.Application.Common.Notifications;

public sealed class AuthorCreatedNotificationHandler : INotificationHandler<AuthorCreatedNotification>
{
    public Task Handle(AuthorCreatedNotification notification, CancellationToken cancellationToken)
    {
        Log.Information("Author created - Id: {Id}, Name: {FullName}",
            notification.Author.Id,
            $"{notification.Author.FirstName} {notification.Author.LastName}");
        return Task.CompletedTask;
    }
}
