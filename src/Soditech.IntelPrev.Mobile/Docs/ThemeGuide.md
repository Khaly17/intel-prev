# Guide de thème IntelPrev

Ce document présente la palette de couleurs officielle d'IntelPrev et explique comment l'utiliser de manière cohérente dans l'application.

## Palette de couleurs

### Couleurs du logo

| Nom | Hex | RGB | Usage |
|-----|-----|-----|-------|
| Fire Red | `#D62828` | (214, 40, 40) | Bande gauche du logo (accent) |
| Deep Sky Blue | `#003F88` | (0, 63, 136) | Contour hexagonal "P" |
| Pure White | `#FFFFFF` | (255, 255, 255) | Arrière-plan général |
| Charcoal Gray | `#2F2F2F` | (47, 47, 47) | Texte "IntelPrev" |
| Light Gray | `#E8E8E8` | (232, 232, 232) | Ombres et bordures |

### Couleurs fonctionnelles

| Nom | Hex | Usage |
|-----|-----|-------|
| Primary | `#003F88` | Couleur principale (bleu foncé) |
| Secondary | `#D62828` | Couleur secondaire (rouge) |
| Success | `#28A745` | Messages de succès (vert) |
| Warning | `#FFC107` | Avertissements (jaune) |
| Danger | `#D62828` | Erreurs et dangers (rouge) |
| Info | `#17A2B8` | Informations (bleu-vert) |

## Utilisation dans XAML

### Références statiques

```xml
<Label Text="Mon texte" TextColor="{StaticResource Primary}" />
<Button BackgroundColor="{StaticResource Secondary}" />
```

### Styles prédéfinis

```xml
<Label Style="{StaticResource TitleStyle}" Text="Mon titre" />
<Button Style="{StaticResource SecondaryButton}" Text="Action" />
<Border Style="{StaticResource CardStyle}" />
```

### Contrôles personnalisés

```xml
<controls:ThemedButton Text="Mon bouton" ButtonStyleType="Primary" />
<controls:ThemedCard CardStyleType="Elevated">
    <Label Text="Contenu de la carte" />
</controls:ThemedCard>
```

## Utilisation en C#

### Via ThemeHelper

```csharp
using Soditech.IntelPrev.Mobile.Helpers;

// Utilisation directe des couleurs
myLabel.TextColor = ThemeHelper.Primary;
myButton.BackgroundColor = ThemeHelper.Secondary;

// Utilisation des couleurs du logo
myCard.BorderColor = ThemeHelper.DeepSkyBlue;
```

### Via ResourceExtensions

```csharp
using Soditech.IntelPrev.Mobile.Extensions;

// Obtenir une couleur par son nom
myLabel.TextColor = ResourceExtensions.GetColor("Primary");
myButton.BackgroundColor = ResourceExtensions.GetColor("Secondary");
```

### Extensions de couleur

```csharp
using Soditech.IntelPrev.Mobile.Extensions;

// Créer une version plus claire
Button.BackgroundColor = ThemeHelper.Primary.Lighter();

// Créer une version avec opacité réduite
Overlay.BackgroundColor = ThemeHelper.CharcoalGray.WithOpacity(0.5f);
```

## Bonnes pratiques

1. **Cohérence** : Utilisez toujours les couleurs définies dans la palette. N'introduisez pas de nouvelles couleurs arbitraires.
   
2. **Sémantique** : Utilisez les couleurs selon leur signification fonctionnelle :
   - `Primary` pour les actions principales
   - `Secondary` pour les actions secondaires
   - `Success`, `Warning`, `Danger` pour les messages correspondants
   
3. **Accessibilité** : Assurez-vous d'un contraste suffisant entre le texte et l'arrière-plan.
   
4. **Mode sombre** : Utilisez les AppThemeBinding pour prendre en charge à la fois le mode clair et le mode sombre.

5. **Contrôles thématiques** : Privilégiez l'utilisation des contrôles personnalisés thématiques (ThemedButton, ThemedCard) qui appliquent automatiquement les couleurs appropriées.
