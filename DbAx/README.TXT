DB-AX V1.0.15 Developer Edition

Eksekver DBAX-D.EXE for at udpakke filerne.

Placeringen af filerne afh�nger af operativ system.

F�r Vista (WinNT/2000/XP/2003):

Bortset fra dokumentations filer (DB-AX.CNT og DB-AX.HLP) skal alle
filer (inkl. DSI.REG) placeres i Windows system bibliotek (WINDOWS\SYSTEM for
Win95/98 og WINDOWS\SYSTEM32 for WinNT/2000/XP/2003).

Vista og fremefter (Vista, Server 2008, Windows 7):
Opret et bibliotek et sted hvor en bruger med normale rettigheder kan tilg�
filerne og placer alle filer i dette bibliotek (inkl. DSI.REG).

N�r dette er gjort eksekveres f�lgende kommandolinie i det bibliotek filerne er placeret i:

  REGSVR32 DBAX.OCX

for at registrere DB-AX overfor Windows.

NB: Dokumentation til DB-AX best�r nu udelukkende af en standard windows hj�lpe fil, 
som til geng�ld indeholder eksempler p� brug af de forskellige funktioner.