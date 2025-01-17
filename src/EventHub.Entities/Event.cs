﻿using System.Text;

namespace EventHub.Entities;

public sealed class Event : Entity
{
    private readonly List<EventSubscription> subscriptions = new();

    public required Guid OwnerId { get; init; }

    public required Guid CategoryId { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public required DateTimeOffset StartDate { get; init; }

    public required DateTimeOffset EndDate { get; init; }

    public required int Capacity { get; init; }

    public EventStatus Status { get; init; } = EventStatus.Draft;

    public IReadOnlyList<EventSubscription> Subscriptions => subscriptions.AsReadOnly();

    public bool IsFull() => subscriptions.Count >= Capacity;

    public void Subscribe(Guid subscriberId) => subscriptions.Add(new EventSubscription
    {
        EventId = Id,
        SubscriberId = subscriberId
    });

    public bool Unsubscribe(Guid subscriberId)
    {
        var subscription = subscriptions.FirstOrDefault(s => s.SubscriberId == subscriberId);
        return subscriptions.Remove(subscription!);
    }

    public Event CopyWith(
        string? name = null,
        string? description = null,
        DateTimeOffset? startDate = null,
        DateTimeOffset? endDate = null,
        int? capacity = null,
        EventStatus? status = null) => new()
        {
            Id = Id,
            OwnerId = OwnerId,
            CategoryId = CategoryId,
            Name = name ?? Name,
            Description = description ?? Description,
            StartDate = startDate ?? StartDate,
            EndDate = endDate ?? EndDate,
            Capacity = capacity ?? Capacity,
            Status = status ?? Status
        };

    public override string ToString() =>
        new StringBuilder()
            .AppendLine($"Owner Id: {OwnerId}")
            .AppendLine($"Category Id: {CategoryId}")
            .AppendLine($"Name: {Name}")
            .AppendLine($"Description: {Description}")
            .AppendLine($"Start: {StartDate}")
            .AppendLine($"End: {EndDate}")
            .AppendLine($"Capacity: {Capacity}")
            .AppendLine($"Status: {Status}")
            .AppendLine($"Subscriptions: {Subscriptions.Count}")
            .ToString();
}
