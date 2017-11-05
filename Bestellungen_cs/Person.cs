using System;

namespace Bestellungen_cs
{
	public class Person : Objekt
	{
		/*Die Klasse Person erbt die Attribute name und nummer von der Klasse Objekt.
		 * Hier ist name der Nachname einer Person und nummer die Kundennummer einer Person */
		/*Zudem hat die Klasse Person die Attribute vorname (Vorname einer Person), strasse (Strasse,
		in der eine Person lebt) und wohnort (Wohnort, in der eine Person lebt) */
		protected string vorname;
		protected string strasse;
		protected string wohnort;

		//Konstruktor
		public Person (string vorname,string nachname, string strasse, string wohnort,string knr)
		{
			this.vorname = vorname;
			this.name = nachname;
			this.strasse = strasse;
			this.wohnort = wohnort;
			this.nummer = knr;
		}

		//Daten einer Person werden ausgegeben
		public string to_string()
		{
			string ausgabe = "";
			if (!this.vorname.Equals ("")) 
			{
				ausgabe += this.vorname;
			}

			if (!this.name.Equals ("")) 
			{
				ausgabe = ausgabe + " " + this.name;
			}
			if (!this.strasse.Equals ("")) 
			{
				ausgabe = ausgabe + " Strasse: " + this.strasse;
			}
			if (!this.wohnort.Equals ("")) 
			{
				ausgabe = ausgabe + " Wohnort: " + this.wohnort;
			}
			if (!this.nummer.Equals ("")) 
			{
				ausgabe = ausgabe + " Kundennummer: " + this.nummer;
			}
			return ausgabe;
		}
	}
}

