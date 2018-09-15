DB-AX V1.0.15 Developer Edition

Eksekver DBAX-D.EXE for at udpakke filerne.

Placeringen af filerne afhænger af operativ system.

Før Vista (WinNT/2000/XP/2003):

Bortset fra dokumentations filer (DB-AX.CNT og DB-AX.HLP) skal alle
filer (inkl. DSI.REG) placeres i Windows system bibliotek (WINDOWS\SYSTEM for
Win95/98 og WINDOWS\SYSTEM32 for WinNT/2000/XP/2003).

Vista og fremefter (Vista, Server 2008, Windows 7):
Opret et bibliotek et sted hvor en bruger med normale rettigheder kan tilgå
filerne og placer alle filer i dette bibliotek (inkl. DSI.REG).

Når dette er gjort eksekveres følgende kommandolinie i det bibliotek filerne er placeret i:

  REGSVR32 DBAX.OCX

for at registrere DB-AX overfor Windows.

NB: Dokumentation til DB-AX består nu udelukkende af en standard windows hjælpe fil, 
som til gengæld indeholder eksempler på brug af de forskellige funktioner.