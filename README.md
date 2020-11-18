[![Actions Status](https://github.com/ludwigdn/secret-santa/workflows/.NET%20Core/badge.svg)](https://github.com/ludwigdn/secret-santa/actions)

# Secret Santa

Oganize a Secret Santa easily among your friends or family.

### Install .NET 5

Install on your dedicated platform: https://dotnet.microsoft.com/download

### Fill out the configuration file

Just fill a file with each person's name and email, and you're good to go (Please leave {0} in the body where to insert the receiver of the present):

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

Then save the file as "secret-santa-config.json" anywhere you want on your computer.

### Dowload the project

Dowload it here: https://github.com/ludwigdn/secret-santa/archive/main.zip

The unzip it anywhere you want on yor computer.

### Open a command prompt

Open a command prompt and type "cd " followed by the path of the project you unziped before (c://PATH/TO/secret-santa-main).

Example: 
```
"cd c://users/ludwig/desktop/secret-santa-main"
```

### Do the magic

Then, a simple command will execute the process.

```
dotnet run -- -c "c://PATH/TO/MY/secret-santa-config.json"
```

Example:
```
dotnet run -- -c "c://users/ludwig/desktop/secret-santa-config.json"
```

Et voilà !
