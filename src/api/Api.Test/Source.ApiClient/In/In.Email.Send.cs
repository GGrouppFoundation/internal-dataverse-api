using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.Json;
using AutoFixture;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> EmailSendInputTestData
    {
        get
        {
            var fixture = new Fixture();
            for (int i = 0; i < 5; i++)
            {
                var emailId = fixture.Create<Guid>();

                yield return new object?[]
                {
                    new DataverseEmailSendIn(emailId),
                    null,
                    new DataverseJsonRequest(
                        verb: DataverseHttpVerb.Post,
                        url: $"/api/data/v9.2/emails({emailId:D})/Microsoft.Dynamics.CRM.SendEmail",
                        headers: FlatArray<DataverseHttpHeader>.Empty,
                        content: new DataverseEmailSendJsonIn
                        {
                            IssueSend = true
                        }.InnerToJsonContentIn()),
                    null
                };
            }

            for (int i = 0; i < 20; i++)
            {
                var emailMessage = fixture.Create<MailMessage>();
                var emails = fixture.CreateMany<MailAddress>(3).Select(m => m.Address).ToArray();
                var memberIds = fixture.CreateMany<Guid>(3).ToArray();
                var emailGuid = fixture.Create<Guid>();
                yield return new object[]
                {
                    new DataverseEmailSendIn(
                        subject: emailMessage.Subject,
                        body: emailMessage.Body,
                        sender: new(emails[0]),
                        recipients: new FlatArray<DataverseEmailRecipient>(
                            new(emails[1], DataverseEmailRecipientType.ToRecipient),
                            new(emailMember: new(memberIds[0], DataverseEmailMemberType.Account), DataverseEmailRecipientType.ToRecipient),
                            new(emailMember: new(memberIds[1], DataverseEmailMemberType.Contact), DataverseEmailRecipientType.CcRecipient),
                            new(emailMember: new(memberIds[2], DataverseEmailMemberType.SystemUser), DataverseEmailRecipientType.BccRecipient),
                            new(emails[2], DataverseEmailRecipientType.ToRecipient))),
                    new DataverseJsonRequest(
                        verb: DataverseHttpVerb.Post,
                        url: "/api/data/v9.2/emails?$select=activityid",
                        headers: DefaultSendEmailHeaders,
                        content: new DataverseEmailCreateJsonIn
                        {
                            Description = emailMessage.Body,
                            Subject = emailMessage.Subject,
                            ActivityParties = new DataverseEmailActivityPartyJson[]
                            {
                                new()
                                {
                                    ParticipationTypeMask = 1,
                                    AddressUsed = emails[0]
                                },
                                new()
                                {
                                    ParticipationTypeMask = 2,
                                    AddressUsed = emails[1]
                                },
                                new ()
                                {
                                    AccountIdParty = $"/accounts({memberIds[0]:D})",
                                    ParticipationTypeMask = 2,
                                },
                                new ()
                                {
                                    ContactIdParty = $"/contacts({memberIds[1]:D})",
                                    ParticipationTypeMask = 3,
                                },
                                new ()
                                {
                                    SystemUserIdParty = $"/systemusers({memberIds[2]:D})",
                                    ParticipationTypeMask = 4,
                                },
                                new()
                                {
                                    ParticipationTypeMask = 2,
                                    AddressUsed = emails[2]
                                }
                            },
                            ExtensionData = new()
                        }.InnerToJsonContentIn()),
                new DataverseJsonRequest(
                    verb: DataverseHttpVerb.Post,
                    url: $"/api/data/v9.2/emails({emailGuid:D})/Microsoft.Dynamics.CRM.SendEmail",
                    headers: default,
                    content: new DataverseEmailSendJsonIn
                    {
                        IssueSend = true
                    }.InnerToJsonContentIn()),
                new DataverseEmailCreateJsonOut()
                {
                    ActivityId = emailGuid
                }
                };
            }

            fixture.Register(() => JsonSerializer.SerializeToElement(fixture.Create<string>()));

            for (int i = 0; i < 20; i++)
            {
                var senderEmail = fixture.Create<MailAddress>().Address;
                var recipientEmails = fixture.CreateMany<MailAddress>(i + 1).Select(ma => ma.Address).ToArray();
                var recipients = recipientEmails
                    .Select(email => new DataverseEmailRecipient(email, DataverseEmailRecipientType.ToRecipient))
                    .ToFlatArray();

                var activityParties = new List<DataverseEmailActivityPartyJson>()
                {
                    new()
                    {
                        AddressUsed = senderEmail,
                        ParticipationTypeMask = 1
                    }
                };

                activityParties.AddRange(recipientEmails.Select(e => new DataverseEmailActivityPartyJson()
                {
                    AddressUsed = e,
                    ParticipationTypeMask = 2
                }));

                var emailMessage2 = fixture.Create<MailMessage>();
                var createdEmailId = fixture.Create<Guid>();

                var extensionData = fixture
                    .CreateMany<KeyValuePair<string, JsonElement>>(i + 1)
                    .DistinctBy(static k => k.Key)
                    .ToDictionary(static k => k.Key, static k => k.Value);

                yield return new object[]
                {
                    new DataverseEmailSendIn(
                        subject: emailMessage2.Subject,
                        body: emailMessage2.Body,
                        sender: new(senderEmail),
                        recipients: recipients,
                        extensionData: extensionData.ToFlatArray()),
                    new DataverseJsonRequest(
                        verb: DataverseHttpVerb.Post,
                        url: "/api/data/v9.2/emails?$select=activityid",
                        headers: DefaultSendEmailHeaders,
                        content: new DataverseEmailCreateJsonIn
                        {
                            Description = emailMessage2.Body,
                            Subject = emailMessage2.Subject,
                            ActivityParties = activityParties.ToFlatArray(),
                            ExtensionData = extensionData
                        }.InnerToJsonContentIn()),
                    new DataverseJsonRequest(
                        verb: DataverseHttpVerb.Post,
                        url: $"/api/data/v9.2/emails({createdEmailId:D})/Microsoft.Dynamics.CRM.SendEmail",
                        headers: FlatArray<DataverseHttpHeader>.Empty,
                        content: new DataverseEmailSendJsonIn
                        {
                            IssueSend = true
                        }.InnerToJsonContentIn()),
                    new DataverseEmailCreateJsonOut()
                    {
                        ActivityId = createdEmailId
                    }
                };
            }
        }
    }
}