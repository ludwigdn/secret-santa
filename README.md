[![Actions Status](https://github.com/ludwigdn/secret-santa/workflows/.NET%20Core/badge.svg)](https://github.com/ludwigdn/secret-santa/actions)

# Secret Santa

### Easy Mode / Mode facile

- [Français](https://github.com/ludwigdn/secret-santa/blob/main/README/fr.md)
- [English](https://github.com/ludwigdn/secret-santa/blob/main/README/en.md)

### Advanced Mode / Mode avancé

**.NET Core 5 required.**

Config file (json) :

- Update the "locale" with either 'en' or 'fr'.
- The field 'MailSubject' can be left as they are, but you can put whatever you want.
- The field "GiftIdeas" (into "Participants") is not mandatory. You can add it if the person has provided a list of gift ideas.
- Other fields must bu updated according to your configuration.

```
{
  "locale": "en",
  "MailSubject": "Secret Santa",
  "SmtpEmail": "email@mailbox.com",
  "SmtpPassword": "ABCD1234",
  "SmtpHost": "smtp.XXXX.XXX",
  "SmtpPort": "465",
  "Participants": [
    {
      "Name": "Alice",
      "Email": "alice@mailbox.com",
      "GiftIdeas" : [
        "The album Appetite for Destruction from Guns n' Roses",
        "A new pillow"
      ]
    },
    {
      "Name": "Bob",
      "Email": "bob@mailbox.com",
      "GiftIdeas" : []
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