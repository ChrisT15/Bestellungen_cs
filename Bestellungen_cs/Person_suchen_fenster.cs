using System;
using Gtk;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Bestellungen_cs
{
	public class Person_suchen_fenster
	{
		/* Die Klasse Person_suchen_fenster enthaelt ein Fenster, in das der Vorname und Nachname eines Kunden eingegeben 
		 * werden kann. Wenn der Knopf "Suchen" gedrueckt wird, dann werden in einer Datenbank alle Personen gesucht, die
		 * den eingegebenen Vornamen oder den eingegebenen Nachnamen haben */

		//ein Fenster wird angelegt
		private Window fenster;

		//Knopf, Beschriftungen und Textfelder fuer das Fenster werden angelegt
		private Label ueberschrift_label;
		private Label vorname_label;
		private Entry vorname_text;

		private Label nachname_label;
		private Entry nachname_text;

		private Button bestaetigung;
		private Label ergebnisse_label;
		private TextView ausgabe;
		private TextBuffer tb;

		//Konstruktor
		public Person_suchen_fenster ()
		{

			//Knopf, Beschriftungen und Textfelder werden zum Fenster hinzugefuegt
			this.fenster = new Window("Person suchen");
			this.fenster.Resize (500, 300);

			this.ueberschrift_label = new Label ("Personendaten eingeben:");

			this.vorname_label = new Label ("Vorname:");
			this.vorname_text = new Entry ();

			this.nachname_label = new Label ("Nachname:");
			this.nachname_text = new Entry ();

	
			this.ergebnisse_label = new Label("Suchergebnisse");
			this.ausgabe = new TextView();

			this.bestaetigung = new Button ("Suchen");
			//Wenn der Knopf bestaetigung gedrueckt wird, dann wird die Funktion suchen ausgefuehrt
			bestaetigung.Clicked += new EventHandler(suchen);


			VBox vbox= new VBox ();

			vbox.Add (ueberschrift_label);
			vbox.Add (vorname_label);
			vbox.Add (vorname_text);
			vbox.Add (nachname_label);
			vbox.Add (nachname_text);
			vbox.Add (bestaetigung);
			vbox.Add (ergebnisse_label);
			vbox.Add (ausgabe);
	

			this.fenster.Add (vbox);
	
			this.fenster.ShowAll ();

		}

		private  void suchen(object sender, EventArgs e)
		{
			/*In dieser Funktion werden alle Kunden in einer Datenbank gesucht, die den eingegebenen Vornamen oder den eingegebenen
			 * Nachnamen haben. */
			string vorname = this.vorname_text.Text;
			string nachname = this.nachname_text.Text;

			//Verbindung zu mysql wird hergestellt
			string connetionString = "server=localhost;uid=root;pwd=;database=Bestellungen;";
			MySqlConnection conn = new MySqlConnection(connetionString);

			try
			{
				conn.Open();

				MySqlCommand sup_query = conn.CreateCommand();

				/*Wenn ein Vorname und ein Nachname eingegeben wurden, dann werden alle Personen in der Tabelle personen_db gesucht,
				 * die diesen Vornamen und diesen Nachnamen haben */
				if(!vorname.Equals("") && !nachname.Equals(""))
				{
					string sup = "select vorname, nachname,strasse,wohnort,kundennummer from personen_db where vorname = ?vorname and nachname = ?nachname";
					sup_query.CommandText = sup;
					sup_query.Parameters.AddWithValue("?vorname", vorname);
					sup_query.Parameters.AddWithValue("?nachname", nachname);

					MySqlDataReader reader = sup_query.ExecuteReader();

					string ergebnis="";
					while (reader.Read()) 
					{
						Person p = new Person(reader.GetString("vorname"),reader.GetString("nachname"),reader.GetString("strasse"),
									reader.GetString("wohnort"),reader.GetString("kundennummer"));
						ergebnis = ergebnis + p.to_string() + System.Environment.NewLine;
					}
					//Suchergebnisse werden in einem Textfeld ausgegeben
					this.tb = this.ausgabe.Buffer;
					this.tb.Text = ergebnis;

				}

				/* Wenn nur ein Vorname eingegeben wurde, dann werden alle Personen in der Tabelle personen_db gesucht, welche
				 * diesen Vornamen haben */
				if(!vorname.Equals("") && nachname.Equals(""))
				{
					string sup = "select vorname, nachname,strasse,wohnort,kundennummer from personen_db where vorname = ?vorname";
					sup_query.CommandText = sup;
					sup_query.Parameters.AddWithValue("?vorname", vorname);

					MySqlDataReader reader = sup_query.ExecuteReader();

					string ergebnis="";
					while (reader.Read()) 
					{
						Person p = new Person(reader.GetString("vorname"),reader.GetString("nachname"),reader.GetString("strasse"),
							reader.GetString("wohnort"),reader.GetString("kundennummer"));
						ergebnis = ergebnis + p.to_string() + System.Environment.NewLine;
					}
					//Suchergebnisse werden in dem Textfeld ausgabe ausgegeben
					this.tb = this.ausgabe.Buffer;
					this.tb.Text = ergebnis;

				}

				/* Wenn nur ein Nachname eingegeben wurde, dann werden alle Personen in der Tabelle personen_db gesucht, welche
				 * diesen Nachnamen haben */
				if(vorname.Equals("") && !nachname.Equals(""))
				{
					string sup = "select vorname, nachname,strasse,wohnort,kundennummer from personen_db where nachname = ?nachname";
					sup_query.CommandText = sup;
					sup_query.Parameters.AddWithValue("?nachname", nachname);

					MySqlDataReader reader = sup_query.ExecuteReader();

					string ergebnis="";
					while (reader.Read()) 
					{
						Person p = new Person(reader.GetString("vorname"),reader.GetString("nachname"),reader.GetString("strasse"),
							reader.GetString("wohnort"),reader.GetString("kundennummer"));
						ergebnis = ergebnis + p.to_string() + System.Environment.NewLine;
					}
					//Suchergebnisse werden in dem Textfeld ausgegeben
					this.tb = this.ausgabe.Buffer;
					this.tb.Text = ergebnis;

				}

				/*Wenn weder ein Vorname noch ein Nachname eingegeben wurde, dann werden alle Personen in der Tabelle personen_db
				 * angezeigt */
				if(vorname.Equals("") && nachname.Equals(""))
				{
					string sup = "select vorname, nachname,strasse,wohnort,kundennummer from personen_db";
					sup_query.CommandText = sup;
					MySqlDataReader reader = sup_query.ExecuteReader();

					string ergebnis="";
					while (reader.Read()) 
					{
						Person p = new Person(reader.GetString("vorname"),reader.GetString("nachname"),reader.GetString("strasse"),
							reader.GetString("wohnort"),reader.GetString("kundennummer"));
						ergebnis = ergebnis + p.to_string() + System.Environment.NewLine;
					}
					//Suchergebnisse werden in dem Textfeld ausgegeben
					this.tb = this.ausgabe.Buffer;
					this.tb.Text = ergebnis;

				}

				conn.Close();
			}
			catch (Exception ex)
			{
				//mysql-Fehlermeldungen werden ausgegeben
				Console.WriteLine("MySQL Fehlermeldung: " + ex.ToString());
			}

			//Textfelder leeren
			this.vorname_text.Text = "";
			this.nachname_text.Text = "";

		
		}

	}
}

