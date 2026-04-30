namespace LibraryProxy.Application.Common.Notifications;

public sealed record BookCreatedNotification(BookDto Book) : INotification;
