[![Actions Status](https://github.com/ludwigdn/secret-santa/workflows/.NET%20Core/badge.svg)](https://github.com/ludwigdn/secret-santa/actions)

# Secret Santa

**.NET Core 6 required.**

Config file (json) :

- Update the "locale" with either 'en' or 'fr'.
- The field `MailSubject` can be left as they are, but you can put whatever you want.
- The field `GiftIdeas` (into "Participants") is not mandatory. You can add it if the person has provided a list of gift ideas.
- The optional field `Partner` can be sent to prevent two people offering gifts to each other (ie, two people in a relationship)
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
      "Partner": "Bob",
      "GiftIdeas" : [
        "The album Appetite for Destruction from Guns n' Roses",
        "A new pillow"
      ],
      "locale": "fr",
    },
    {
      "Name": "Bob",
      "Partner": "Alice",
      "Email": "bob@mailbox.com",
      "GiftIdeas" : []
    },
    {
      "Name": "Foobar",
      "Email": "foobar@mailbox.com"
    },
    {
      "Name": "Barfoo",
      "Email": "barfoo@mailbox.com"
    }
  ]
}
```

Then:

```
dotnet run -- -c c://path/to/secret_santa_config.json
```
