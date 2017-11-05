using System;
using Gtk;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Bestellungen_cs
{
	public class Bestellung_person_fenster
	{
		/* Die Klasse Bestellung_person_fenster beinhaltet ein Fenster, in das eine Kundennummer eingegeben werden kann.
		 * Wenn der Knopf "Suchen" gedrueckt wird, dann werden in einer Datenbank alle Bestellungen gesucht, die der Kunde
		 * mit der eingegebenen Kundennummer ausgefuehrt hat. Pro Bestellung wird der Name des bestellten Artikels, die 
		 * Anzahl, wie oft der Artikel bestellt wurde, und der Gesamtpreis der Bestellung angezeigt. Zudem wird der Gesamtpreis
		 * aller gefundenen Bestellungen gesucht. */

		//ein Fenster wird angelegt
		private Window fenster;

		//Knopf, Beschriftungen und Textfelder fuer das Fenster werden angelegt 
		private Label ueberschrift_label;

		private Label knr_label;
		private Entry knr_text;



		private Button bestaetigung;
		private Label ergebnisse_label;
		private TextView ausgabe;
		private TextBuffer tb;

		//Konstruktor
		public Bestellung_person_fenster ()
		{

			//Knopf, Beschriftungen und Textfelder werdem zum Fenster hinzugefuegt
			this.fenster = new Window("Bestellungen einer Person suchen");
			this.fenster.Resize (500, 300);

			this.ueberschrift_label = new Label ("Kundennummer eingeben:");

			this.knr_label = new Label ("Kundennummer:");
			this.knr_text = new Entry ();

			this.ergebnisse_label = new Label("Suchergebnisse");
			this.ausgabe = new TextView();

			this.bestaetigung = new Button ("Suchen");
			//Wenn der Knopf bestaetigung gedrueckt wird, dann wird die Funktion suchen ausgefuehrt
			bestaetigung.Clicked += new EventHandler(suchen);


			VBox vbox= new VBox ();

			vbox.Add (ueberschrift_label);
			vbox.Add (knr_label);
			vbox.Add (knr_text);
			vbox.Add (bestaetigung);
			vbox.Add (ergebnisse_label);
			vbox.Add (ausgabe);


			this.fenster.Add (vbox);

			this.fenster.ShowAll ();

		}

		private  void suchen(object sender, EventArgs e)
		{
			/*Diese Funktion sucht alle Bestellungen, die von einem Kunden mit der ins Fenster eingegebenen
			 * Kundennummer ausgefuehrt wurden, in einer Datenbank. */ 

			string knr = this.knr_text.Text;

			//Verbindung zu mysql herstellen
			string connetionString = "server=localhost;uid=root;pwd=;database=Bestellungen";
			MySqlConnection conn = new MySqlConnection(connetionString);

			try
			{
				conn.Open();


				MySqlCommand suche_query = conn.CreateCommand();

				/*Wenn eine Kundennummer ins Fenster eingegeben wurde, werden alle Bestellungen in der
				 * Datenbank Bestellungen gesucht, die der Kunde mit der eingegebenen Kundennummer ausgefuehrt hat */
				if(!knr.Equals(""))
				{
					/*Pro Bestellung werden der Artikelname, der Artikelpreis, die Anzahl, wie oft der Artikel bestellt
					 * wurde, und der Gesamtpreis der Bestellung angezeigt */

					string suche = "select artikel_db.name, artikel_db.preis, bestellungen_db.anzahl, " +
									"round(artikel_db.preis*bestellungen_db.anzahl,2) as gesamtpreis from " +
									"artikel_db left outer join bestellungen_db on " +
									"bestellungen_db.artikelnummer = artikel_db.nummer where bestellungen_db.kundennummer = ?knr " +
									"order by artikel_db.name";
					suche_query.CommandText = suche;
					suche_query.Parameters.AddWithValue("?knr", knr);


					MySqlDataReader reader = suche_query.ExecuteReader();

					//in summe wird der Gesamtpreis aller gefunden Bestellungen gespeichert
					float summe=0;
					string ergebnis="";
					while (reader.Read()) 
					{
						summe += reader.GetFloat("gesamtpreis");
						ergebnis = ergebnis + "Artikelname: " + reader.GetString("name") + " Einzelreis: " +
							reader.GetString("preis") + " Anzahl: " + reader.GetString("anzahl") +
							" Gesamtpreis: " + reader.GetString("gesamtpreis") + System.Environment.NewLine;
					}
					//Summe wird auf 2 Nachkommastellen gerundet
					summe = (float) Math.Round(summe,2);
					//Suchergebnisse werden in dem Textfeld ausgabe ausgegeben
					this.tb = this.ausgabe.Buffer;
					this.tb.Text = ergebnis + System.Environment.NewLine + "Summe: " + summe;

				}
				//Fall, wenn keine Kundennummer ins Fenster eingegeben wurde
				else
				{
					this.tb = this.ausgabe.Buffer;
					this.tb.Text = "Es wurde keine Kundennummer eingegeben";

				}


				conn.Close();
			}
			catch (Exception ex)
			{
				//mysql-Fehlermeldungen werden ausgegeben
				Console.WriteLine("MySQL Fehlermeldung: " + ex.ToString());
			}

			this.knr_text.Text = "";


		}

	}
}

