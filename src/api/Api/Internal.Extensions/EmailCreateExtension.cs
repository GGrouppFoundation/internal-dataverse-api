using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace GarageGroup.Infra;

internal static class EmailCreateExtension
{
    internal static DataverseEmailCreateJsonIn MapInput(this DataverseEmailCreateIn input)
    {
        return new()
        {
            Subject = input.Subject,
            Description = input.Body,
            ActivityParties = MapSender(input.Sender).AsFlatArray().Concat(input.Recipients.Map(MapRecipient)),
            ExtensionData = input.ExtensionData.AsEnumerable().ToDictionary(GetKey, GetValue)
        };

        static string GetKey(KeyValuePair<string, JsonElement> item)
            =>
            item.Key;

        static JsonElement GetValue(KeyValuePair<string, JsonElement> item)
            =>
            item.Value;
    }

    private static DataverseEmailActivityPartyJson MapSender(DataverseEmailSender sender)
        =>
        new DataverseEmailActivityPartyJson
        {
            ParticipationTypeMask = 1,
            AddressUsed = string.IsNullOrEmpty(sender.SenderEmail) ? null : sender.SenderEmail
        }
        .WithPartyId(sender.SenderMember);

    private static DataverseEmailActivityPartyJson MapRecipient(DataverseEmailRecipient recipient)
        =>
        new DataverseEmailActivityPartyJson
        {
            AddressUsed = string.IsNullOrEmpty(recipient.SenderRecipientEmail) ? null : recipient.SenderRecipientEmail,
            ParticipationTypeMask = recipient.EmailRecipientType.MapRecipientType()
        }
        .WithPartyId(recipient.EmailMember);

    private static int MapRecipientType(this DataverseEmailRecipientType recipientType)
        =>
        recipientType switch
        {
            DataverseEmailRecipientType.CcRecipient => 3,
            DataverseEmailRecipientType.BccRecipient => 4,
            _ => 2
        };

    private static DataverseEmailActivityPartyJson WithPartyId(
        this DataverseEmailActivityPartyJson activityPartyJson,
        DataverseEmailMember? member)
        =>
        member?.MemberType switch
        {
            DataverseEmailMemberType.Account => activityPartyJson with { AccountIdParty = $"/accounts({member.MemberId:D})" },
            DataverseEmailMemberType.Contact => activityPartyJson with { ContactIdParty = $"/contacts({member.MemberId:D})" },
            DataverseEmailMemberType.SystemUser => activityPartyJson with { SystemUserIdParty = $"/systemusers({member.MemberId:D})"},
            _ => activityPartyJson
        };
}