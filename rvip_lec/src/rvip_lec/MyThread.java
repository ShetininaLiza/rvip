package rvip_lec;

public class MyThread extends Thread{

	public int id;
	
	public MyThread(int _id)
	{
		id=_id;
	}
	/*
	 * только одному потоку удастся выйти из цикла 
	 * и он сохранит блокировку до тех пор, пока не запустит метод разблокировки.
	 */
	public void run()
	{
		int scale = 2;

		for (int i = 0; i < Main.count; i++) 
		{
			lock(id);
			// Начало критического 
			Main.count = Main.count + 1;
			System.out.println("Процесс №: " + id);
			//Ждем, чтобы создать состояние гонки между потоками
			try {
				sleep((int) (Math.random() * scale));
			} catch (InterruptedException e) { /* nothing */ }
			// Конец критического
			unlock(id);
		}
	}
	public void lock(int id)
	{
		//Данный поток попадает в КС
		Main.choosing[id] = true;
		System.out.println("Процесс №: " + id+" попал в КС.");
		//Находим максимальное значение и добавьте 1, чтобы получить следующий доступный билет.
		Main.ticket[id] = findMax() + 1;
		Main.choosing[id] = false;
		for (int j = 0; j < Main.N; j++) 
		{
			//Если поток j является текущим потоком, перейдите к следующему потоку.
			if (j == id)
				continue;
			// Wait if thread j is choosing right now.
			while (Main.choosing[j]) { /* nothing */ }
			while (Main.ticket[j] != 0 && 
					(Main.ticket[id] > Main.ticket[j] || (Main.ticket[id] == Main.ticket[j] && id > j))) 
			{ /* nothing */ }
								 
		}
	}
	public void unlock(int id)
	{
		Main.ticket[id] = 0;
		System.out.println("Процесс №: " + id+" вышел из КС.");
	}
	private int findMax() 
	{
		
		int m = Main.ticket[0];

		for (int i = 1; i < Main.ticket.length; i++) {
			if (Main.ticket[i] > m)
				m = Main.ticket[i];
		}
		return m;
	}
}
