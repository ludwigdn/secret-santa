[![Actions Status](https://github.com/ludwigdn/secret-santa/workflows/.NET%20Core/badge.svg)](https://github.com/ludwigdn/secret-santa/actions)

# Secret Santa

Oganize a Secret Santa easily among your friends or family.

PS: this README has been edited for the beginners in computer environment.

### Install .NET 5

Install on your dedicated platform: https://dotnet.microsoft.com/download

### Dowload the project

Dowload it here: https://github.com/ludwigdn/secret-santa/archive/main.zip

The unzip it anywhere you want on yor computer.

### Fill out the configuration file

Download the configuration file below, according to you mail provider, and replace each time you are asked to:

- [Gmail](https://raw.githubusercontent.com/ludwigdn/secret-santa/main/README/configs/gmail/config_en.json)
- [Outlook](https://raw.githubusercontent.com/ludwigdn/secret-santa/main/README/configs/outlook/config_en.json)
- [Laposte.net](https://raw.githubusercontent.com/ludwigdn/secret-santa/main/README/configs/laposte/config_en.json)
- [Yahoo](https://raw.githubusercontent.com/ludwigdn/secret-santa/main/README/configs/yahoo/config_en.json)
- [Zoho](https://raw.githubusercontent.com/ludwigdn/secret-santa/main/README/configs/zoho/config_en.json)
- [AOL](https://raw.githubusercontent.com/ludwigdn/secret-santa/main/README/configs/aol/config_en.json)

Careful, in "mailBody", if you wish to change the message, please leave the "{0}" because it will be au tomatically to insert the receiver of the present. So type your message accordingly (For instance, "Hi, you've been chosen to give a present to {0}").

### Command prompt

Open a command prompt, according to your computer type, and type "cd " followed by the path of the project you unziped before (c://PATH/TO/secret-santa-main).

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

And voilà !
