namespace LibraryProxy.Application.Common.Notifications;

public sealed record AuthorCreatedNotification(AuthorDto Author) : INotification;
