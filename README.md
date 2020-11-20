[![Actions Status](https://github.com/ludwigdn/secret-santa/workflows/.NET%20Core/badge.svg)](https://github.com/ludwigdn/secret-santa/actions)

# Secret Santa

### Easy Mode / Mode facile

- [Français](https://github.com/ludwigdn/secret-santa/blob/main/README/fr.md)
- [English](https://github.com/ludwigdn/secret-santa/blob/main/README/en.md)

### Advanced Mode / Mode avancé

**.NET Core 5 required.**

Config file (json) :

- Update the "lang" with either 'en' or 'fr'.
- In case you wish to change the "MailBody", please leave a "{0}" where the name of the receiver of the gift will appear.
- Fields 'MailSubject', 'MailBodyTitle', or 'MailBody' can be left as they are, but you can put whatever you want.
- Other fields must bu updated according to your configuration

```
{
  "MailSubject": "Secret Santa",
  "MailBodyTitle": "Secret Santas have been chosen!",
  "MailBody":  "You were chosen to give a present to <span style=\"font-weight: bold; color: yellow;\">{0}</span>.</br></br> Try to fit to what they like! If you have no clue, just ask around :-)</br></br><div>Happy Holidays to all, et keep the secret until the last minute !</div>",
  "SmtpEmail": "email@mailbox.com",
  "SmtpPassword": "ABCD1234",
  "SmtpHost": "smtp.XXXX.XXX",
  "SmtpPort": "465",
  "Participants": [
    {
      "Name": "Alice",
      "Email": "alice@mailbox.com"
    },
    {
      "Name": "Bob",
      "Email": "bob@mailbox.com"
    },
    {
      "Name": "Foobar",
      "Email": "foobar@mailbox.com"
    }
  ]
}
```

Then:

```
dotnet run -- -c c://path/to/secret_santa_config.json
```