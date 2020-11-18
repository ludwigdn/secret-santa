[![Actions Status](https://github.com/ludwigdn/secret-santa/workflows/.NET%20Core/badge.svg)](https://github.com/ludwigdn/secret-santa/actions)

# Secret Santa

Organisez facilement un secret santa avec vos amis ou votre famille.

PS: Ce README a été rédigé à l'attention des néophytes de l'informatique.

### Installation de .NET 5

Installez le sdk sur votre ordinateur selon son type: https://dotnet.microsoft.com/download

### Téléchargez le projet

A télécharger ici: https://github.com/ludwigdn/secret-santa/archive/main.zip

Ensuite, dézippez-le n'importe où sur votre ordinateur.

### Fichier de configuration

Téléchargez le fichier de configuration ci dessous, selon votre boite mail, et remplacez chaque indication l'indiquant :

- [Gmail](https://raw.githubusercontent.com/ludwigdn/secret-santa/main/README/configs/gmail/config_fr.json)
- [Outlook](https://raw.githubusercontent.com/ludwigdn/secret-santa/main/README/configs/outlook/config_fr.json)
- [Laposte.net](https://raw.githubusercontent.com/ludwigdn/secret-santa/main/README/configs/laposte/config_fr.json)
- [Yahoo](https://raw.githubusercontent.com/ludwigdn/secret-santa/main/README/configs/yahoo/config_fr.json)
- [Zoho](https://raw.githubusercontent.com/ludwigdn/secret-santa/main/README/configs/zoho/config_fr.json)
- [AOL](https://raw.githubusercontent.com/ludwigdn/secret-santa/main/README/configs/aol/config_fr.json)

Attention, dans "mailBody", si vous désirez changer le message, le "{0}" est à laisser car  il sera automatiquement remplacé par le nom du destinataire du cadeau. Saisissez donc votre message en conséquence (Par exemple "Salut, tu as été choisi pour faire un cadeau à {0}").

### Fenêtre console

Ouvrez ensuite une fenêtre console (Powershell, Terminal,... tout dépend de votre ordinateur), et tapez "cd " followed by the path of the project you unziped before (c://PATH/TO/secret-santa-main).

Exemple: 
```
"cd c://users/ludwig/desktop/secret-santa-main"
```

Cela vous permettre d'aller dans ce dossier.

### Do the magic

Enfin, une simple commande exécutera le programme.

```
dotnet run -- -c "c://PATH/TO/MY/config.json"
```

Exemple:
```
dotnet run -- -c "c://users/ludwig/desktop/config.json"
```

Et voilà !
