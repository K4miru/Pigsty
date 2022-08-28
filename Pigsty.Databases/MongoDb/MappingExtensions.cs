namespace Pigsty.Databases.MongoDb;

public static class MappingExtensions
{
    internal static int? AsDaysSinceEpoch(this DateTime? dateTime) => dateTime is null ? null : AsDaysSinceEpoch(dateTime.Value);
    internal static int AsDaysSinceEpoch(this DateTime dateTime) => (dateTime - new DateTime()).Days;
    internal static DateTime? AsDateTime(this int? daysSinceEpoch) => daysSinceEpoch is null ? null : AsDateTime(daysSinceEpoch.Value);
    internal static DateTime AsDateTime(this int daysSinceEpoch) => new DateTime().AddDays(daysSinceEpoch);

    internal static double? AsMillisecondsSinceEpoch(this DateTime? dateTime) => dateTime is null ? null : AsMillisecondsSinceEpoch(dateTime.Value);
    internal static double AsMillisecondsSinceEpoch(this DateTime dateTime) => (dateTime - new DateTime()).TotalMilliseconds;
    internal static DateTime? AsDateTime(this double? millisecondsSinceEpoch) => millisecondsSinceEpoch is null ? null : AsDateTime(millisecondsSinceEpoch.Value);
    internal static DateTime AsDateTime(this double millisecondsSinceEpoch) => new DateTime().AddMilliseconds(millisecondsSinceEpoch);
}