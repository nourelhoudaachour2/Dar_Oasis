<div align="center">

# Dar Oasis

**Application web de gestion d'établissement d'hébergement : chambres, clients, réservations, facturation et personnel**

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-MVC_.NET_8-512BD4?logo=dotnet&logoColor=white)
![Entity Framework](https://img.shields.io/badge/EF_Core-Code_First-512BD4?logo=dotnet&logoColor=white)
![Razor](https://img.shields.io/badge/Razor-Views-512BD4?logo=dotnet&logoColor=white)
![Status](https://img.shields.io/badge/status-en_développement-orange)

</div>

---

## À propos

**Dar Oasis** est une application ASP.NET Core MVC qui digitalise la gestion quotidienne d'un établissement d'hébergement : suivi des chambres, enregistrement des clients et de leurs accompagnants, réservations, services supplémentaires, facturation et gestion du personnel avec affectation des tâches.

L'accès est protégé par authentification, et un tableau de bord donne une vue d'ensemble de l'activité.

## Fonctionnalités

| Module | Description |
|---|---|
| **Authentification** | Connexion sécurisée, comptes utilisateurs liés aux employés |
| **Dashboard** | Vue synthétique : réservations, occupation, activité |
| **Chambres** | CRUD complet, statut et notes par chambre |
| **Clients** | Fiches clients et gestion des accompagnants |
| **Réservations** | Création, suivi du statut, nombre de personnes, dates de séjour |
| **Services supplémentaires** | Catalogue de services ajoutables à un séjour |
| **Factures** | Génération et suivi des factures par réservation |
| **Employés** | Fiches employés avec photo, poste et informations personnelles |
| **Affectations** | Attribution des employés aux tâches et chambres |

## Stack technique

- **Backend** : ASP.NET Core MVC (.NET 8), C#
- **ORM** : Entity Framework Core (Code First, migrations)
- **Frontend** : Razor Views, HTML, CSS, JavaScript
- **Sécurité** : authentification, secrets via User Secrets

## Structure du projet

```
DarOasis/
├── Controllers/      # Affectations, Auth, Chambres, Clients, Dashboard,
│                     # Employes, Factures, Reservations, ServiceSupplementaires
├── Models/           # Entités (Chambre, Client, Accompagnant, Reservation...)
├── ViewModels/       # Modèles de présentation
├── Data/             # DbContext
├── Migrations/       # Migrations EF Core
├── Views/            # Vues Razor par module
└── wwwroot/          # CSS, JS, images
```

## Installation

### Prérequis

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Serveur de base de données configuré dans la chaîne de connexion
- Visual Studio 2022 (recommandé)

### Étapes

```bash
# 1. Cloner le dépôt
git clone https://github.com/nourelhoudaachour2/Dar_Oasis.git
cd Dar_Oasis/DarOasis

# 2. Configurer la chaîne de connexion (non commitée)
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "VOTRE_CHAINE_DE_CONNEXION"

# 3. Créer la base de données
dotnet ef database update

# 4. Lancer l'application
dotnet run
```

L'application est ensuite accessible sur `https://localhost:xxxx` (port indiqué dans la console).

## Captures d'écran

<!-- Ajoutez vos images dans docs/screenshots puis décommentez -->
<!--
| Dashboard | Réservations |
|---|---|
| ![Dashboard](docs/screenshots/dashboard.png) | ![Réservations](docs/screenshots/reservations.png) |
-->

## Feuille de route

- [x] Gestion des chambres, clients et accompagnants
- [x] Réservations et services supplémentaires
- [x] Facturation
- [x] Gestion des employés et affectations
- [x] Authentification et dashboard
- [ ] Export PDF des factures
- [ ] Tests unitaires
- [ ] Déploiement

## Auteure

**Nour El Houda Achour**
Étudiante ingénieure en Génie Logiciel et Applications, IT Business School (ITBS) Nabeul

[![GitHub](https://img.shields.io/badge/GitHub-nourelhoudaachour2-181717?logo=github)](https://github.com/nourelhoudaachour2)

---

<div align="center">
Projet académique ASP.NET Core MVC
</div>
