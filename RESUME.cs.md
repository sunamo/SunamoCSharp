---
schema_version: 1
type: library
file_count: 23
delete_recommendation_percent: 5
generated_date: 2026-09-29
---

## Description

Pomocné nástroje pro práci s C# zdrojovým kódem (parsování, generování), vyčleněné z monolitu `SunamoDevCode`.

Obsahuje `CSharpHelper`/`CSharpGenerator` třídy rozdělené do několika partial souborů pro různé oblasti generování/analýzy C# kódu.

Publikováno na NuGet 2026-09-29. Dlouhodobě padající test `IndentAsPreviousLineTest` opraven 2026-09-29 (skutečná chyba byla v `SunamoString.SH.IndentAsPreviousLine`, ne v tomhle repu).
