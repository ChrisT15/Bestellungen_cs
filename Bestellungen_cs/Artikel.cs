using System;

namespace Bestellungen_cs
{
	public class Artikel : Objekt
	{
		/*die Klasse Artikel erbt die Attribute name und nummer von Objekt.
		 * name ist hier der Artikelname und nummer ist hier die Artikelnummer */
		//Preis des Artikels
		private string preis;

		//Konstruktor
		public Artikel (string aname, string anr, string preis)
		{
			this.name = aname;
			this.nummer = anr;
			this.preis = preis;
		}

		//Daten eines Artikels werden ausgegeben
		public string to_string()
		{
			string ausgabe = "";
			if (!this.name.Equals ("")) 
			{
				ausgabe = ausgabe + "Artikelname: " + this.name;		
			}
			if (!this.nummer.Equals ("")) 
			{
				ausgabe = ausgabe + " Artikelnummer: " + this.nummer;		
			}
			if (!this.preis.Equals ("")) 
			{
				ausgabe = ausgabe + " Preis: " + this.preis;		
			}
			return ausgabe;
		}
	}
}

