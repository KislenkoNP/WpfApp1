//Простой пример запуска потока в WPF на основе BackgroundWorker
using System;
using System.ComponentModel; /* BackgroundWorker */
using System.Windows;

namespace WpfApp1 {
 public partial class MainWindow : Window {
  private BackgroundWorker worker = null;
  public MainWindow () {
   InitializeComponent ();
   worker = new BackgroundWorker ();
   worker.WorkerSupportsCancellation = true; //можно прерывать
   worker.WorkerReportsProgress = true; //сообщать прогресс
   worker.DoWork += DoWork; //обработка
   worker.ProgressChanged += ProgressChanged; //изменение прогресса
   worker.RunWorkerCompleted += Completed; //завершение
  }
		void DoWork (object sender, DoWorkEventArgs e) {
			for (int i = 0; i <= 100000; i++) {
				if (worker.CancellationPending == true) {
					e.Cancel = true;
					return;
				}
				if (i % 1000 == 0) {
					e.Result = i;
					worker.ReportProgress (i); //вызывает событие ProgressChanged
					System.Threading.Thread.Sleep (100); //остановка на 100 мс
				}
			}
		}

		void ProgressChanged (object sender, ProgressChangedEventArgs e) {
			table.AppendText(String.Format ("{0}\n", e.ProgressPercentage));
			 //или table.Text += "строка";
			if (table.Visibility == Visibility.Visible) { //Чтобы прокручивалось
				//table.SelectionStart = table.Text.Length;
				table.ScrollToEnd ();
			}
		}

		void Completed (object sender, RunWorkerCompletedEventArgs e) {
			if (e.Cancelled) {
				table.AppendText ("Операция прервана пользователем\n");
			}
			else {
				table.AppendText (String.Format ("Операция выполнена, результат = {0}\n", e.Result));
			}
		}
		private void Button_Click (object sender, RoutedEventArgs e) { //Очистить
			worker.CancelAsync ();
		}
  private void Button_Click_1 (object sender, RoutedEventArgs e) { //Выполнить
			table.Clear ();
			worker.RunWorkerAsync ();
		}
////
 }
}