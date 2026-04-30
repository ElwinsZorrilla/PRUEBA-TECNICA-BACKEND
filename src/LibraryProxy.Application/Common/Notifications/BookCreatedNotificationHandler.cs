namespace LibraryProxy.Application.Common.Notifications;

public sealed class BookCreatedNotificationHandler : INotificationHandler<BookCreatedNotification>
{
    public Task Handle(BookCreatedNotification notification, CancellationToken cancellationToken)
    {
        Log.Information("Book created - Id: {Id}, Title: {Title}", notification.Book.Id, notification.Book.Title);
        return Task.CompletedTask;
    }
}
