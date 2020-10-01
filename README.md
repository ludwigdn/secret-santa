[![Actions Status](https://github.com/ludwigdn/secret-santa/workflows/.NET%20Core/badge.svg)](https://github.com/ludwigdn/secret-santa/actions)

# Secret Santa

Oganize a Secret Santa easily among your friends or family. Just fill a file with each person's name and email, and you're good to go (Please leave {0} in the body where to insert the receiver of the present):

```
{
  "MailSubject": "You are now ready for the Secret Santa !",
  "MailBodyTitle": "Title",
  "MailBody": "Body {0}",
  "SmtpEmail": "email@mail.com",
  "SmtpHost": "smtp.mail.com",
  "SmtpPassword": "12345",
  "SmtpPort": "111",
  "Participants": [
    {
      "Name": "name1",
      "Email": "mail1@mail.com"
    },
    {
      "Name": "name2",
      "Email": "mail2@mail.com"
    }
  ]
}
```

Then, a simple command will do the magic:

```
dotnet run -- -c "c://path/to/my-config-file.json"
```
