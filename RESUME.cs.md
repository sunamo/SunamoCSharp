---
schema_version: 2
type: library
file_count: 64
delete_recommendation_percent: 5
generated_date: 2026-09-30
generated_time: 16:42:57
---

## Description

Pomocné nástroje pro práci s C# zdrojovým kódem (parsování, generování), vyčleněné z monolitu `SunamoDevCode`. Obsahuje `CSharpHelper`/`CSharpGenerator` rozdělené do několika partial souborů pro různé oblasti analýzy a generování C# kódu.
Balíček je self-contained: kód dříve referencovaného balíčku DevCodeBase je zkopírován (jen skutečně používané členy) do `_sunamo\SunamoDevCodeBase\` jako internal a jiné Sunamo balíčky nereferencuje (testovací projekt si referencuje zdrojové projekty String/StringGetLines).
