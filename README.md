# GroceryApp sprint3  
    
## Gitflow: Inrichting en Gebruik

  Branchstructuur
- **main**: bevat altijd de stabiele, productieklare code.
- **develop**: hier wordt de nieuwste ontwikkelcode samengevoegd voordat het live gaat.
- **feature**/*: voor nieuwe features, aftakkend vanaf develop.
- **release**/*: voor voorbereiden en testen van releases, ook vanaf develop.
- **hotfix**/*: voor snelle fixes in productie, aftakkend vanaf main.
Aanvullende Items voor Gitflow in je README/CONTRIBUTING.md

## Workflow Stappen

-    Start altijd vanaf een up-to-date develop (of main bij hotfixes)
-    Gebruik altijd git pull voordat je een branch aanmaakt om lokale en remote wijzigingen te synchroniseren.
-    Naast git merge kun je git rebase gebruiken bij het bijwerken van je feature branch, maar alleen als je hier ervaring mee hebt (anders kans op conflicten).

**Branch Naming Convention**

-    Gebruik duidelijke namen voor branches zoals:
-    feature/<onderwerp>, release/<versie>, hotfix/<omschrijving>
-    de naam altijd kleine letters, koppeltekens als separator (bijv. feature/zoek-functionaliteit).

**Releases en Hotfixes**

-    Release wordt altijd vanaf develop gestart en na afronding naar zowel main als develop gemerged (inclusief taggen).

-    Hotfix direct van main aftakken, na afronden mergen naar zowel main als develop.

**Pull Requests en Code Reviews**
    Elke feature/release/hotfix gaat via een Pull Request (PR).

**Bij elke PR:**
 - Voeg een bondige beschrijving van de wijzigingen toe.

 - Link relevante issues (bijvoorbeeld: "Fixes #12").

 - Los commentaar op en reageer actief bij discussies.

**Branch Opruimen**
-    Verwijder feature-, release- en hotfix-branches na de merge uit de remote repo (git push origin --delete branchnaam).



## Regels en Best Practices

- Maak voor elke feature, release of hotfix een aparte branch met duidelijke naamgeving.

- Merge nooit direct naar main of develop, gebruik altijd Pull Requests voor code reviews.

- Verwijder gebackmergde branches na afronden van het werk.

- Documenteer elke PR volgens het sjabloon; vermeld altijd relevante issues en context.

- Tag releases op main met het versienummer (v1.2.3).

  

