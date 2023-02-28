using System;
using System.Linq;

namespace GGroupp.Infra;

internal static class CreateEmailExtension
{
    internal static DataverseEmailCreateJsonIn MapInput(this DataverseEmailCreateIn input)
        =>
        new()
        {
            Subject = input.Subject,
            Description = input.Body,
            ActivityParties = input.MapSenderRecipient(),
            ExtensionData = input.ExtensionData.AsEnumerable().ToDictionary(
                static kv => kv.Key, 
                static kv => kv.Value)
        };
    
    private static FlatArray<DataverseEmailActivityPartyJson> MapSenderRecipient(this DataverseEmailCreateIn input)
    {
        var builder = FlatArray<DataverseEmailActivityPartyJson>.Builder.OfLength(input.Recipients.Length + 1);
        builder[0] = input.Sender.MapSender();

        for (int i = 0; i < input.Recipients.Length; i++)
        {
            builder[i + 1] = input.Recipients[i].MapRecipient();
        }

        return builder.MoveToFlatArray();
    }

    private static DataverseEmailActivityPartyJson MapSender(this DataverseEmailSender sender)
        =>
        new DataverseEmailActivityPartyJson()
        {
            ParticipationTypeMask = 1,
            AddressUsed = string.IsNullOrEmpty(sender.SenderEmail) ? null : sender.SenderEmail
        }
        .WithPartyId(sender.SenderMember);

    private static DataverseEmailActivityPartyJson MapRecipient(
        this DataverseEmailRecipient recipient)
        =>
        new DataverseEmailActivityPartyJson()
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