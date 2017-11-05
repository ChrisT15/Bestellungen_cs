using System;
using Gtk;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Bestellungen_cs
{
	public class Bestellung_artikel_fenster
	{
		/* Die Klasse Bestellung_artikel_fenster beinhaltet ein Fenster, in das die Artikelnummer eines 
		 * Artikels eingegeben werden kann. Wenn der Knopf "Suchen" gedrueckt wird, werden alle Bestellungen
		 * in einer Datenbank gesucht, in welchen der Artikel mit dieser Artikelnummer bestellt wurde. Pro 
		 * Bestellung wird der Name der Person, die bestellt hat, die Anzahl, wie oft der Artikel bestellt 
		 * wurde, und der Gesamtpreis der Bestellung angezeigt. Zudem wird der Gesamtpreis aller gefunden 
		 * Bestellungen ausgegeben */

		//ein Fenster wird angelegt
		private Window fenster;


		//Knopf, Beschriftungen und Textfelder fuer das Fenster werden angelegt
		private Label ueberschrift_label;

		private Label anr_label;
		private Entry anr_text;



		private Button bestaetigung;
		private Label ergebnisse_label;
		private TextView ausgabe;
		private TextBuffer tb;


		//Konstruktor
		public Bestellung_artikel_fenster ()
		{

			//Knopf, Beschriftungen und Textfelder werden zum Fenster hinzugefuegt
			this.fenster = new Window("Bestellungen eines Artikels suchen");
			this.fenster.Resize (500, 300);

			this.ueberschrift_label = new Label ("Artikelnummer eingeben:");

			this.anr_label = new Label ("Artikelnummer:");
			this.anr_text = new Entry ();

			this.ergebnisse_label = new Label("Suchergebnisse");
			this.ausgabe = new TextView();

			this.bestaetigung = new Button ("Suchen");
			//Wenn der Knopf bestaetigung gedrueckt wird, dann wird die Funktion suchen ausgefuehrt
			bestaetigung.Clicked += new EventHandler(suchen);


			VBox vbox= new VBox ();

			vbox.Add (ueberschrift_label);
			vbox.Add (anr_label);
			vbox.Add (anr_text);
			vbox.Add (bestaetigung);
			vbox.Add (ergebnisse_label);
			vbox.Add (ausgabe);


			this.fenster.Add (vbox);

			this.fenster.ShowAll ();

		}

		private  void suchen(object sender, EventArgs e)
		{
			/*In dieser Funktion werden alle Bestellungen gesucht, in der der Artikel mit der
			 * eingegebenen Artikelnummer, bestellt wurde */

			string anr = this.anr_text.Text;

			//Verbindung zu mysql wird hergestellt
			string connetionString = "server=localhost;uid=root;pwd=;database=Bestellungen;";
			MySqlConnection conn = new MySqlConnection(connetionString);

			try
			{
				
				conn.Open();
				MySqlCommand suche_query = conn.CreateCommand();

				/*Wenn eine Artikelnummer eingegeben wurde, werden alle Bestellungen in der Datenbank
				 * Bestellungen gesucht, in welchen der Artikel mit dieser Artikelnummer bestellt wurde */

				if(!anr.Equals(""))
				{
					//Preis des Artikels mit eingegebener Artikelnummer wird in der Tabelle artikel_db gesucht
					suche_query.CommandText = "select preis from artikel_db where nummer = ?anr1";
					suche_query.Parameters.AddWithValue("?anr1",anr);
					MySqlDataReader s_preis = suche_query.ExecuteReader();
					//Preis des Artikels mit eingegebener Artikelnummer wird in der Variablen Preis gespeichert
					float preis = 0;
					if(s_preis.Read())
					{
						preis = s_preis.GetFloat("preis");
					}
					s_preis.Close();
					/*Es werden in der Datenbank Bestellungen alle Bestellungen gesucht, in welchen der Artikel
					 * mit der eingegebenen Artikelnummer bestellt wurde. Pro Bestellung wird der Name des Kunden,
					 * der bestellt hat, die Anzahl, wie oft der Artikel bestellt wurde, angezeigt */

					string suche = "select personen_db.nachname, personen_db.vorname, bestellungen_db.anzahl from " +
									"personen_db left outer join bestellungen_db on personen_db.kundennummer " +
									"= bestellungen_db.kundennummer where bestellungen_db.artikelnummer like ?anr";
					suche_query.CommandText = suche;
					suche_query.Parameters.AddWithValue("?anr", anr);


					MySqlDataReader reader = suche_query.ExecuteReader();

					//in summe wird der Gesamtpreis aller gefundenen Bestellungen gespeichert 
					float summe=0;
					//in gesamtpreis wird der Gesamtpreis einer Bestellung gespeichert
					float gesamtpreis = 1;
					string ergebnis="";
					while (reader.Read()) 
					{
						gesamtpreis = reader.GetFloat("anzahl") * preis;
						summe += gesamtpreis;
						ergebnis = ergebnis + reader.GetString("nachname") + " " + reader.GetString("vorname") + " Anzahl: " +
							reader.GetString("anzahl") + " Gesamtpreis: " + gesamtpreis + System.Environment.NewLine;
					}
					//Summe wird auf zwei Nachkommastellen gerundet
					summe = (float) Math.Round(summe,2);
					//Suchergebnisse werden in dem Textfeld ausgabe gespeichert
					this.tb = this.ausgabe.Buffer;
					this.tb.Text = ergebnis + System.Environment.NewLine + "Summe: " + summe;

				}
				//Fall, wenn keine Artikelnummer eingegeben wurde
				else
				{
					this.tb = this.ausgabe.Buffer;
					this.tb.Text = "Es wurde keine Artikelnummer eingegeben";

				}


				conn.Close();
			}
			catch (Exception ex)
			{
				//mysql-Fehlermeldungen werden ausgegeben
				Console.WriteLine("MySQL Fehlermeldung: " + ex.ToString());
			}

			//Textfeld wird geleert
			this.anr_text.Text = "";


		}

	}
}

