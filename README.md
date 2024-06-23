# KING_Zadatak
Ulazni zadatak za akademiju KING ICT

# 1. Konfiguracija i podešavanje baze, servisa

Baza podataka ne postoji jer zadatak je bio dohvaćati podatke s REST API. Npravljene su samo male pripreme za korištenje baze u budućnosti.
Ostatak konfiguracija i potrebnih paketa se automatski dobija kloniranjem projekta!

# 2. Autentifikacija i autorizacija

Nijedan controlller (endpoint) se ne može pokrenuti bez logiranja i autorizacije! Logiranjem se dobije token koji se šalje na autorizaciju pri pozivu controllera (User rola).
Prvo je potrebno napraviti logiranje putem Login contorellera s username i password podacima:

# Testni korisnik: 
              username: emilys
              password: emilyspass

Napomena: valjan je bilo koji korisnik s dummyjson REST API-a.

# 3. Testiranje aplikacije

Testirati se može korištenjem Swaggera ili Postmana. Za ovaj primjer koristim Swagger. 
Prvo se obavlja logiranje upisivanjem usernamea i passworda u Login controller. Potom se mogu koristiti 4 endpointa za proizvode:
  - Prvi endpoint dohvaća sve proizvode i ne zahtjeva nikakav input
  - Drugi endpoint dohvaća jedan proizvod prema Id-u, te je potrebno unijeti Id (integer od 0 do 50)
  - Treći endpoint obavlja filtriranje prema kategoriji (unijeti string npr. beauty) i prema maksimalnoj cijeni (unijeti double tip npr. 10)
  - Četvrti obavlja pretragu prema ključnoj riječi (unijeti string npr. laptop)
