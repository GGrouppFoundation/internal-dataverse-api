using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.Json;
using AutoFixture;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{   
    public static IEnumerable<object?[]> EmailCreateInputTestData
    {
        get
        {
            var fixture = new Fixture();

            for (int i = 0; i < 20; i++)
            {
                var emailMessage = fixture.Create<MailMessage>();
                var emails = fixture.CreateMany<MailAddress>(3).Select(m => m.Address).ToArray();
                var memberIds = fixture.CreateMany<Guid>(3).ToArray();

                yield return new object[]
                {
                    new DataverseEmailCreateIn(
                    subject: emailMessage.Subject,
                    body: emailMessage.Body,
                    sender: new(emails[0]),
                    recipients: new FlatArray<DataverseEmailRecipient>(
                        new(emails[1], DataverseEmailRecipientType.ToRecipient),
                        new(emailMember: new(memberIds[0], DataverseEmailMemberType.Account), DataverseEmailRecipientType.ToRecipient),
                        new(emailMember: new(memberIds[1], DataverseEmailMemberType.Contact), DataverseEmailRecipientType.CcRecipient),
                        new(emailMember: new(memberIds[2], DataverseEmailMemberType.SystemUser), DataverseEmailRecipientType.BccRecipient),
                        new(emails[2], DataverseEmailRecipientType.ToRecipient)),
                    extensionData: default
                ),
                    new DataverseJsonRequest(
                    verb: DataverseHttpVerb.Post,
                    url: "/api/data/v9.2/emails?$select=activityid",
                    headers: DefaultSendEmailHeaders,
                    content: new DataverseEmailCreateJsonIn
                    {
                        Description = emailMessage.Body,
                        Subject = emailMessage.Subject,
                        ActivityParties = new FlatArray<DataverseEmailActivityPartyJson>(
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
                            new()
                            {
                                AccountIdParty = $"/accounts({memberIds[0]:D})",
                                ParticipationTypeMask = 2,
                            },
                            new()
                            {
                                ContactIdParty = $"/contacts({memberIds[1]:D})",
                                ParticipationTypeMask = 3,
                            },
                            new()
                            {
                                SystemUserIdParty = $"/systemusers({memberIds[2]:D})",
                                ParticipationTypeMask = 4,
                            },
                            new()
                            {
                                ParticipationTypeMask = 2,
                                AddressUsed = emails[2]
                            }),
                        ExtensionData = new()
                    }.InnerToJsonContentIn())
                };
            }

            fixture.Register<JsonElement>(() => JsonSerializer.SerializeToElement(fixture.Create<string>()));

            for (int i = 0; i < 20; i++)
            {
                var senderEmail = fixture.Create<MailAddress>().Address;
                var recipientEmails = fixture.CreateMany<MailAddress>(i + 1).Select(static ma => ma.Address).ToArray();
                var recipients = recipientEmails
                    .Select(static email => new DataverseEmailRecipient(email, DataverseEmailRecipientType.ToRecipient))
                    .ToFlatArray();

                var activityParties = new List<DataverseEmailActivityPartyJson>()
                {
                    new()
                    {
                        AddressUsed = senderEmail,
                        ParticipationTypeMask = 1
                    }
                };
                activityParties.AddRange(recipientEmails.Select(static e => new DataverseEmailActivityPartyJson()
                {
                    AddressUsed = e,
                    ParticipationTypeMask = 2
                }));

                var emailMessage2 = fixture.Create<MailMessage>();
                var extensionData = fixture
                    .CreateMany<KeyValuePair<string, JsonElement>>(i + 1)
                    .DistinctBy(k => k.Key)
                    .ToDictionary(k => k.Key, k => k.Value);

                yield return new object[]
                {
                    new DataverseEmailCreateIn(
                    subject: emailMessage2.Subject,
                    body: emailMessage2.Body,
                    sender: new(senderEmail),
                    recipients: recipients,
                    extensionData: extensionData.ToFlatArray()
                ),
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
                    }.InnerToJsonContentIn())
                };
            }
        }
    }

    public static IEnumerable<object?[]> InvalidEmailCreateInputTestData
    {
        get
        {
            var fixture = new Fixture();

            var noSenderInput = new DataverseEmailCreateIn(
                subject: fixture.Create<MailMessage>().Subject,
                body: fixture.Create<MailMessage>().Body,
                sender: null!,
                recipients: new FlatArray<DataverseEmailRecipient>(
                    new(fixture.Create<MailAddress>().Address, DataverseEmailRecipientType.ToRecipient),
                    new(emailMember: new(new Fixture().Create<Guid>(), DataverseEmailMemberType.Account),
                        DataverseEmailRecipientType.ToRecipient)),
                extensionData: default);

            var noSenderFailure = Failure.Create(DataverseFailureCode.Unknown, "Input sender is missing");

            yield return new object[] { noSenderInput, noSenderFailure };

            var invalidSenderInput = new DataverseEmailCreateIn(
                subject: fixture.Create<MailMessage>().Subject,
                body: fixture.Create<MailMessage>().Body,
                sender: new(string.Empty),
                recipients: new FlatArray<DataverseEmailRecipient>(
                    new(fixture.Create<MailAddress>().Address, DataverseEmailRecipientType.ToRecipient),
                    new(emailMember: new(new Fixture().Create<Guid>(), DataverseEmailMemberType.Account),
                        DataverseEmailRecipientType.ToRecipient)),
                extensionData: default);

            var invalidSenderFailure = Failure.Create(DataverseFailureCode.Unknown, "Input sender is invalid");

            yield return new object[] { invalidSenderInput, invalidSenderFailure };

            var noRecipientInput = new DataverseEmailCreateIn(
                subject: fixture.Create<MailMessage>().Subject,
                body: fixture.Create<MailMessage>().Body,
                sender: new(fixture.Create<MailAddress>().Address),
                recipients: FlatArray<DataverseEmailRecipient>.Empty,
                extensionData: default);

            var noRecipientFailure = Failure.Create(DataverseFailureCode.Unknown, "Input recipients are missing");

            yield return new object[] { noRecipientInput, noRecipientFailure };

            var invalidRecipient = new DataverseEmailCreateIn(
                subject: fixture.Create<MailMessage>().Subject,
                body: fixture.Create<MailMessage>().Body,
                sender: new(fixture.Create<MailAddress>().Address),
                recipients: new FlatArray<DataverseEmailRecipient>(
                    new DataverseEmailRecipient(string.Empty, DataverseEmailRecipientType.ToRecipient)),
                extensionData: default);

            var invalidRecipientFailure = Failure.Create(DataverseFailureCode.Unknown, "Input recipients are invalid");

            yield return new object[] { invalidRecipient, invalidRecipientFailure };
        }
    }
}