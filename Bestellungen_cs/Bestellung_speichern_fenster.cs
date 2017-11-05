using System;
using Gtk;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Bestellungen_cs
{
	public class Bestellung_speichern_fenster
	{
		/* Die Klasse Bestellung_speichern_fenster beinhaltet ein Fenster, in das die Kundennummer des Kunden,
		 * der bestellt, die Artikelnummer des Artikels, der bestellt wird, und die Anzahl, wie oft der Artikel
		 * bestellt wird, eingegeben werden koennen. */

		//ein Fenster wird angelegt
		private Window fenster;

		//Knopf, Beschriftungen und Textfelder fuer das Fenster werden angelegt
		private Label ueberschrift_label;

		private Label knr_label;
		private Entry knr_text;

		private Label anr_label;
		private Entry anr_text;

		private Label anzahl_label;
		private Entry anzahl_text;

		private Button bestaetigung;

		//Konstruktor
		public Bestellung_speichern_fenster ()
		{

			//Knopf, Beschriftungen und Textfelder werdem zum Fenster hinzugefuegt
			this.fenster = new Window("Bestellung speichern");
			this.fenster.Resize (400, 400);

			this.ueberschrift_label = new Label ("Daten einer Bestellung eingeben:");

			this.knr_label = new Label ("Kundennummer:");
			this.knr_text = new Entry ();

			this.anr_label = new Label ("Artikelnummer:");
			this.anr_text = new Entry ();

			this.anzahl_label = new Label ("Anzahl:");
			this.anzahl_text = new Entry ();


			this.bestaetigung = new Button ("Speichern");
			//Wenn der Knopf bestaetigung gedrueckt wird, dann wird die Funktion speichern ausgefuehrt
			bestaetigung.Clicked += new EventHandler(speichern);


			VBox vbox= new VBox ();

			vbox.Add (ueberschrift_label);
			vbox.Add (knr_label);
			vbox.Add (knr_text);
			vbox.Add (anr_label);
			vbox.Add (anr_text);
			vbox.Add (anzahl_label);
			vbox.Add (anzahl_text);
			vbox.Add (bestaetigung);



			this.fenster.Add (vbox);
			this.fenster.ShowAll ();

		}

		private  void speichern(object sender, EventArgs e)
		{
			/*In dieser Funktion werden die Bestellungsdaten, die ins Fenster eingegeben wurden, 
			 * in einer Datenbank gespeichert */
			string knr = this.knr_text.Text;
			string anr = this.anr_text.Text;
			string anzahl = this.anzahl_text.Text;

			//Verbindung zu mysql wird hergestellt
			string connetionString = "server=localhost;uid=root;pwd=;database=Bestellungen;";
			MySqlConnection conn = new MySqlConnection(connetionString);

			try
			{
				conn.Open();

				MySqlCommand sp_query = conn.CreateCommand();
				//Bestellungsdaten, die ins Fenster eingegeben wurden, werden in der Tabelle bestellungen_db gespeichert
				string sb = "insert into bestellungen_db (kundennummer,artikelnummer,anzahl) values (?knr,?anr,?anzahl)";
				sp_query.CommandText = sb;
				sp_query.Parameters.AddWithValue("?knr", knr);
				sp_query.Parameters.AddWithValue("?anr", anr);
				sp_query.Parameters.AddWithValue("?anzahl", anzahl);
				sp_query.ExecuteNonQuery();

				conn.Close();
			}
			catch (Exception ex)
			{
				//mysql-Fehlermeldungen werden ausgegeben
				Console.WriteLine("MySQL-Fehlermeldung: " + ex.ToString());
			}

			//Textfelder werden geleert
			this.knr_text.Text = "";
			this.anr_text.Text = "";
			this.anzahl_text.Text = "";

		}


	}
}

