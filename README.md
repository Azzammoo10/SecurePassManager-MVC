# SecurePassManager

**SecurePassManager** est une application web sécurisée de gestion de mots de passe, développée avec **ASP.NET Core 8 MVC**. Cette solution permet aux utilisateurs de gérer, chiffrer et sécuriser leurs mots de passe tout en offrant une interface intuitive et des fonctionnalités robustes de sécurité.

---

## Fonctionnalités

- **Gestion des Comptes** : Ajout, mise à jour, suppression et affichage des comptes utilisateurs.
- **Chiffrement Sécurisé** : Utilisation de l'algorithme AES pour chiffrer les mots de passe avant stockage en base de données, avec un déchiffrement uniquement lors de la consultation.
- **Sécurisation des Clés de Chiffrement** : Intégration possible avec **Azure Key Vault**, **AWS Secrets Manager** ou **HashiCorp Vault**.
- **Validation des Données** : Garantie de la validité des entrées grâce à **FluentValidation**.
- **Gestion Centralisée des Erreurs** : Middleware global pour capturer et gérer les erreurs de manière centralisée.
- **Journalisation des Activités** : Mise en place de **Serilog** pour suivre les opérations, analyser les erreurs et auditer les actions.
- **Pagination et Versioning des API** : Support des grandes listes de données et évolutivité des API grâce au versioning.
- **Sécurité Renforcée** : Protection contre les attaques courantes telles que CSRF, XSS et SQL Injection.
- **Authentification et Autorisation** : Système basé sur **ASP.NET Core Identity** avec gestion des rôles pour contrôler l'accès.

---

## Technologies Utilisées

- **Framework** : ASP.NET Core 8
- **Langage** : C#
- **Base de Données** : SQL Server ou SQLite (configuration flexible)
- **Chiffrement** : AES (Advanced Encryption Standard) avec génération dynamique des clés
- **Frontend** : Razor Pages intégré à **Bootstrap** pour une expérience utilisateur fluide et responsive
- **Validation** : FluentValidation pour des formulaires fiables
- **Gestion des Secrets** : Intégration optionnelle avec Azure Key Vault, AWS Secrets Manager ou HashiCorp Vault
- **Journalisation** : Serilog pour les logs et l'audit des activités

---

## Installation

### Prérequis

Avant de commencer, assurez-vous que les éléments suivants sont installés sur votre machine :

- [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server) ou [SQLite](https://sqlite.org/download.html)
- Un gestionnaire de secrets tel que **Azure Key Vault**, **AWS Secrets Manager** ou **HashiCorp Vault** (optionnel)

### Étapes d'Installation

1. Clonez le dépôt GitHub :
   ```bash
   git clone https://github.com/Azzammoo10/SecurePassManager-MVC.git
   cd SecurePassManager-MVC
   ```

2. Configurez la chaîne de connexion dans le fichier `appsettings.json` :
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "VotreChaîneDeConnexionSQLServer"
     }
   }
   ```

3. Restaurez les dépendances :
   ```bash
   dotnet restore
   ```

4. Appliquez les migrations et mettez à jour la base de données :
   ```bash
   dotnet ef database update
   ```

5. Lancez l'application en mode développement :
   ```bash
   dotnet run
   ```

6. Accédez à l'application dans votre navigateur à l'adresse suivante :
   [http://localhost:5000](http://localhost:5000)

---

## Contribution

Les contributions sont les bienvenues ! Pour contribuer :

1. Forkez ce dépôt.
2. Créez une branche pour votre fonctionnalité (`feature/nouvelle-fonctionnalite`).
3. Effectuez vos modifications et commitez-les (`git commit -m "Ajout d'une nouvelle fonctionnalité"`).
4. Poussez sur votre branche (`git push origin feature/nouvelle-fonctionnalite`).
5. Ouvrez une Pull Request.

---

## Licence

Ce projet est sous licence **MIT**. Consultez le fichier [LICENSE](LICENSE) pour plus de détails.

---

## Auteur

Développé par [Azzammoo10](https://github.com/Azzammoo10). N'hésitez pas à me contacter pour toute question ou suggestion à [azzam.moo10@gmail.com](mailto:azzam.moo10@gmail.com).
```

### Étapes pour le Push sur GitHub

Après avoir créé et ajouté ce fichier `README.md`, procédez ainsi pour le pusher sur GitHub :

1. **Ajoutez les modifications** :
   ```bash
   git add README.md
   git commit -m "Ajout du fichier README.md"
   ```

2. **Poussez les changements** :
   ```bash
   git push origin main
   ```

Et voilà ! Votre projet sur GitHub sera bien présenté avec ce fichier.
![image](https://github.com/user-attachments/assets/d347ce6a-5c9e-45fe-a339-c4da51d81b50)
![image](https://github.com/user-attachments/assets/bdc96703-ea9e-46f5-be68-5b8c4253722b)
![image](https://github.com/user-attachments/assets/4c8f374f-0872-4842-b6e3-84ced3393733)

