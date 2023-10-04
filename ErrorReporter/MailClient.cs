using System.Diagnostics;

namespace UGC_App.ErrorReporter;
using MailKit.Net.Smtp;
using MimeKit;

public static class MailClient
{
    internal static bool IsDelLog { get; private set; }
    internal static bool IsDelError { get; private set; }

    internal static bool Send(string additional)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("UGC App Log Sender", "reporter@united-german-commanders.de"));
        message.To.Add(new MailboxAddress("Empfänger", "lord@asrothear.de"));
        message.Subject = $"UGC App User:{Config.Instance.Cmdr}";
        var bodyBuilder = new BodyBuilder();
        if (string.IsNullOrWhiteSpace(additional))
        {
            additional = "Keine weiteren Details";
        }

        bodyBuilder.TextBody =
            $"User: {Config.Instance.Cmdr}\nUGC App Version: {Config.Instance.Version}{Config.Instance.VersionMeta}\nGame Version: {Config.Instance.GameVersion}-{Config.Instance.GameBuild}\nOS: {Environment.OSVersion.VersionString}\nContent:\n{additional}";
        message.Body = bodyBuilder.ToMessageBody();


        var multipart = new Multipart("mixed");
        multipart.Add(bodyBuilder.ToMessageBody());
        FileStream? file = null;
        FileStream? file2 = null;
        if (File.Exists(Path.Combine(Config.Instance.PathLogs, "error_log.json")))
        {
            file = File.OpenRead(Path.Combine(Config.Instance.PathLogs, "error_log.json"));
            var attachment1 = new MimePart("application", "json")
            {
                Content = new MimeContent(file),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = "error_log.json"
            };
            multipart.Add(attachment1);
        }

        if (Config.Instance.Debug && File.Exists(Path.Combine(Config.Instance.PathLogs, "log.json")))
        {
            file2 = File.OpenRead(Path.Combine(Config.Instance.PathLogs, "log.json"));
            var attachment2 = new MimePart("application", "json")
            {
                Content = new MimeContent(file2),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = "log.json"
            };
            multipart.Add(attachment2);
        }

        try
        {
            message.Body = multipart;
            using var client = new SmtpClient();
            client.Connect("mail.united-german-commanders.de", 465, true);
            client.Authenticate("support",
                "%kC2Ak9Q3@0&C%GXrf!F1J3Puh3h4$etv%7STS^04VzTR*005!Zw0Il$13@iY2J52!N7wk4770LKl!giJ1R*0IIPfZx&e8Wx0TYUSLZi%MpUSr@&qYBL*!gZQc&!Gm%#");
            var res = client.Send(message);
            client.Disconnect(true);
            Program.Log(res);
            file?.Close();
            file2?.Close();
            if (!res.Contains("Ok:")) return false;
            IsDelLog = true;
            IsDelError = true;
            Thread.Sleep(1000);
            File.Delete(Path.Combine(Config.Instance.PathLogs, "log.json"));
            File.Delete(Path.Combine(Config.Instance.PathLogs, "error_log.json"));
            IsDelLog = false;
            IsDelError = false;
            return true;
        }
        catch (Exception ex)
        {
            Program.LogException(ex);
            return false;
        }
    }
}